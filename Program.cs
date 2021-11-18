
using CommandDotNet;

namespace sensitive_word_finder
{
    class Program
    {
        static void Main(string[] args)
        {
            new AppRunner<Finder>().Run(args);
        }
    }
}
