using System;
using System.IO;
using Serilog;

namespace sensitive_word_finder
{
    public interface IReadStrategy
    {
        void Start();
        void SetArrivedAction(Action<string> dataArrived);

    }

    class ReadBlock
    {
        private IReadStrategy readStrategy;
        public ReadBlock(IReadStrategy ReadStrategy)
        {
            readStrategy = ReadStrategy;
        }
        public void Start()
        {
            readStrategy.SetArrivedAction(DataArrived);
            readStrategy.Start();
        }
        public Action<string> DataArrived;
    }

    class FileReadStrategy : IReadStrategy
    {
        private string _path;
        private Action<string> DataArrived;
        public FileReadStrategy(string path)
        {
            _path = path;
        }

        public void Start()
        {
            StreamReader sr = new StreamReader(_path);
            try
            {
                string line = null;
                while ((line = sr.ReadLine()) != null)
                {
                    Log.Information("Input string: {Line}", line);
                    Console.WriteLine(line);
                    DataArrived?.Invoke(line);
                }
            }
            catch
            {
                Log.Error("Can't write {FilePath}", _path);
            }
            finally
            {
                if (sr != null) ((IDisposable)sr).Dispose();
            }
        }
        public void SetArrivedAction(Action<string> dataArrived)
        {
            DataArrived = dataArrived;
        }
    }
    public class ConsoleReadStrategy : IReadStrategy
    {
        private Action<string> DataArrived;
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
        public void SetArrivedAction(Action<string> dataArrived)
        {
            DataArrived = dataArrived;
        }
    }
}
