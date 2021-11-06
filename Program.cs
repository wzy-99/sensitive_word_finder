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
            string input = sr.ReadLine();
            if (input == "Hello World")
            {
                sw.WriteLine("Hello wangziyi!");
            }
            else
            {
                sw.WriteLine(input);
            }
            sw.Close();
        }
    }
}
