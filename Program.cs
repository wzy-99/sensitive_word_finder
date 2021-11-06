using System;

namespace sensitive_word_finder
{
    class Program
    {
        static void Main(string[] args)
        {
            String input = Console.ReadLine();
            if (input == "Hello World")
            {
                Console.WriteLine("Hello wangziyi!");
            }
            else
            {
                Console.WriteLine(input);
            }
        }
    }
}
