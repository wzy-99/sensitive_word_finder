using System;
using Serilog;

namespace sensitive_word_finder
{
    public class ConsoleReadBlock
    {
        public void Start()
        {
            Log.Information("Please input");
            while (true)
            {
                string input = Console.ReadLine();
                if (input != "")
                {
                    Log.Information("Input string: {Input}", input);
                    DataArrived?.Invoke(input);
                }
            }
        }
        public Action<string> DataArrived;
    }
}
