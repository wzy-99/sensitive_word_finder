using System;
using System.IO;

namespace sensitive_word_finder
{
    class Program
    {
        static void Main(string[] args)
        {
            StreamReader sr = new StreamReader("D:/Program/homework/sensitive_word_finder/input.txt");
            StreamWriter sw = new StreamWriter("D:/Program/homework/sensitive_word_finder/output.txt");
            string input = new string("");
            // 读取每行，并用空格连接
            string line = sr.ReadLine();
            while (line != null)
            {
                input += line + ' ';
                line = sr.ReadLine();
            }
            // 删除最后一个空格
            if (input.Length - 2 > 0)
            {
                input = input.Remove(input.Length - 2);
            }
            if (input == "Hello World")
            {
                sw.WriteLine("Hello wangziyi!");
            }
            else
            {
                sw.Write(input);
            }
            sw.Close();
        }
    }
}
