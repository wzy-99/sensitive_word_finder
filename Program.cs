using System;
using System.IO;
using System.Threading.Tasks.Dataflow;
using System.Threading.Tasks;
using Serilog;
using Serilog.Events;

namespace sensitive_word_finder
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day) // restrictedToMinimumLevel 限制信息最小等级 rollingInterval 
                .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Information) // Log event sinks 将事件记录到外部表示
                .CreateLogger();

            //定义一个 文本处理流程
            Log.Debug("Creating transformBlock");
            TransformBlock<string, string> transformBlock = new TransformBlock<string, string>((input) =>
            {
                return input.Replace("Hello world", "Hello wangziyi");
            });
            // TransformBlock 接收message，并对每个message调用委托。TransformBlock有输出，可以连接到其他Block。

            //定义一个 命令行输出流程
            Log.Debug("Creating ConsoleBlock");
            ActionBlock<string> consoleBlock = new ActionBlock<string>((input) =>
            {
                Log.Information("Processed string: {Input}", input);
            });
            // ActionBlock 接收message，并对每个message调用委托。ActionBlock没有输出，因此不能连接到其他Block。

            //定义一个 文件输出流程
            Log.Debug("Creating fileBlock");
            ActionBlock<string> fileBlock = new ActionBlock<string>((input) =>
            {
                String filePath = "D:/Program/homework/sensitive_word_finder/output.txt";
                StreamWriter sw = new StreamWriter(filePath, true);
                try
                {
                    sw.WriteLine(input);
                }
                catch
                {
                    Log.Error("Can't write {FilePath}", filePath);
                }
                finally
                {
                    if (sw != null) ((IDisposable)sw).Dispose();
                }
            });

            Log.Debug("Creating broadcastBlock");
            BroadcastBlock<string> broadcastBlock = new BroadcastBlock<string>(p => p);
            // BroadcastBlock 提供缓冲区，最多储存一个message，当有新的message到来时，会覆盖原来的message。

            // 连接数据流
            Log.Debug("Linking blocks");
            transformBlock.LinkTo(broadcastBlock);
            broadcastBlock.LinkTo(consoleBlock);
            broadcastBlock.LinkTo(fileBlock);

            // 命令行读取
            Log.Debug("Creating consoleReadBlock");
            ConsoleReadBlock consoleReadBlock = new ConsoleReadBlock();

            consoleReadBlock.DataArrived += (e) =>
            {
                transformBlock.Post(e);
                // Post方法可以实现并行处理message，需要在构造示例的时候设置ExecutionDataflowBlockOptions参数。
                // 其中MaxDegreeOfParallelism就是最大并行度。
            };

            consoleReadBlock.Start();
        }
    }
}
