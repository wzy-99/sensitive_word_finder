using System;
using System.IO;
using Serilog;


namespace sensitive_word_finder
{
    class FileReadBlock
    {
        private string _path;
        public FileReadBlock(string path)
        {
            _path = path;
        }

        public void Start()
        {
            using (StreamReader sr = new StreamReader(_path))
            {
                string line = null;
                while ((line = sr.ReadLine()) != null)
                {
                    Log.Information("Input string: {Line}", line);
                    Console.WriteLine(line);
                    DataArrived?.Invoke(line);
                }
            }
        }
        public Action<string> DataArrived;
    }
}
