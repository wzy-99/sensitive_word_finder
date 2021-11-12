using System;
using System.IO;

namespace sensitive_word_finder
{
    class Program
    {
        static void Main(string[] args)
        {
            //构建了一个命令行输入,进行文本替换，最后存储到文件并且显示的类
            ConsoleReadBlock consoleReadBlock = new ConsoleReadBlock();
            SensitiveReplaceBlock sensitiveReplaceBlock = new SensitiveReplaceBlock("Hello world", "Hello wangziyi");
            FileWriteBlock fileWriteBlock = new FileWriteBlock("D:/Program/homework/sensitive_word_finder/output.txt");
            ConsoleWriteBlock consoleWriteBlock = new ConsoleWriteBlock();

            consoleReadBlock.DataArrived += (e) =>
            {
                sensitiveReplaceBlock.Enqueue(e);
            };
            sensitiveReplaceBlock.DataArrived += (e) =>
            {
                consoleWriteBlock.Enqueue(e);
                fileWriteBlock.Enqueue(e);
            };
            consoleReadBlock.Start();
        }
    }
}
