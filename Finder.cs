using System;
using System.IO;
using System.Threading.Tasks.Dataflow;
using System.Threading.Tasks;
using Serilog;
using Serilog.Events;
using CommandDotNet;
using Spectre.Console;
using System.Threading;

namespace sensitive_word_finder
{
    [Command(Description = "Find and replace string or file")]
    class Finder
    {
        [DefaultCommand,
        Command(Name = "findreplace",
        Usage = "findreplace <int>",
        Description = "find and replace string or file",
        ExtendedHelpText = "more details and examples")]
        public void FindAndReplace(
            [Option(LongName = "type", ShortName = "t",
                    Description = "type of input, 0 for string, 1 for text file")]
            int type = 0,
            [Option(LongName = "log_path", ShortName = "l",
                    Description = "the path of logger file")]
            string logPath = "logs/log.txt",
            [Option(LongName = "input_path", ShortName = "i",
                    Description = "the path of input file")]
            string inputPath = "input.txt",
            [Option(LongName = "output_path", ShortName = "o",
                    Description = "the path of output file")]
            string outputPath = "output.txt")
        {

            AnsiConsole.Progress()
                .Start(ctx =>
                {
                    // Define tasks
                    var task = ctx.AddTask("[green]loading[/]");

                    while (!ctx.IsFinished)
                    {
                        task.Increment(1.5);
                        Thread.Sleep(50);
                    }
                });

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(logPath, rollingInterval: RollingInterval.Day) // restrictedToMinimumLevel 限制信息最小等级 rollingInterval 
                .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Information) // Log event sinks 将事件记录到外部表示
                .CreateLogger();

            var table = new Table();
            table.AddColumn(new TableColumn("[rgb(255,255,0)]Author[/]"));
            table.AddColumn(new TableColumn("[rgb(255,0,255)]School[/]"));
            table.AddColumn(new TableColumn("[rgb(0,255,255)]Subject[/]"));
            table.AddRow(new Markup("[red]Wangziyi[/]"), new Markup("[blue]Hust[/]"), new Markup("[green]AIA[/]"));
            AnsiConsole.Write(table);

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
                String filePath = outputPath;
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

            IReadStrategy strategy;
            if (type == 1)
            {
                strategy = new FileReadStrategy(inputPath);
            }
            else
            {
                strategy = new ConsoleReadStrategy();
            }
            ReadBlock readBlock = new ReadBlock(strategy);

            readBlock.DataArrived += (e) =>
            {
                transformBlock.Post(e);
                // Post方法可以实现并行处理message，需要在构造示例的时候设置ExecutionDataflowBlockOptions参数。
                // 其中MaxDegreeOfParallelism就是最大并行度。
            };

            readBlock.Start();

            Log.CloseAndFlush();
        }
    }
}
