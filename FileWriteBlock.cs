using System;
using System.IO;
using System.Threading.Tasks.Dataflow;

namespace sensitive_word_finder
{
    class FileWriteBlock
    {
        private string _path;
        public FileWriteBlock(string path)
        {
            _path = path;
            _Write = new ActionBlock<string>(p => {
                File.AppendAllText(_path, p);
            });
        }
        private ActionBlock<string> _Write;
        public void Enqueue(string input)
        {
            _Write.Post(input);
        }
    }
}
