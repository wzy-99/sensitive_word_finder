using System;
using System.Threading.Tasks.Dataflow;


namespace sensitive_word_finder
{
    class SensitiveReplaceBlock
    {
        private string _oldValue;
        private string _newValue;
        public SensitiveReplaceBlock(string oldValue, string newValue)
        {
            _oldValue = oldValue;
            _newValue = newValue;
            _Process = new ActionBlock<string>(p => {
                var result = p.Replace(_oldValue, _newValue);
                DataArrived?.Invoke(result);
            });
        }
        public void Enqueue(string result)
        {
            _Process.Post(result);
            // _Process是一个ActionBlock对象，其具有Post方法。
            // 相比Action，ActionBlock的Post方法可以轻松的实现并行处理message。
        }
        private ActionBlock<string> _Process;
        public Action<string> DataArrived;
    }
}
