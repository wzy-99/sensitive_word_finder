using System;
using System.Threading.Tasks.Dataflow;

namespace sensitive_word_finder
{
    public class ConsoleWriteBlock
    {
        private ActionBlock<string> _InputBlock;
        public ConsoleWriteBlock()
        {
            _InputBlock = new ActionBlock<string>(p => {
                Console.Write("ouput:");
                Console.WriteLine(p);
            });
        }
        public void Enqueue(string input)
        {
            _InputBlock.Post(input);
        }
    }
}
