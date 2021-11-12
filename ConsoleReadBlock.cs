using System;

namespace sensitive_word_finder
{
    public class ConsoleReadBlock
    {
        public void Start()
        {

            Console.WriteLine("等待输入 ");
            while (true)
            {
                string input = Console.ReadLine();
                if (input != "")
                {
                    DataArrived?.Invoke(input);
                }
            }
        }
        public Action<string> DataArrived;
    }
}
