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
            Finder finder = new Finder("Hello World", "Hello wangziyi");
            String output = finder.Find(sr);
            sw.Write(output);
            sw.Close();
        }
    }

    class Finder
    {
        String query;
        String replacement;
        public Finder(String query, String replacement)
        {
            this.query = query;
            this.replacement = replacement;
        }
        public String StringFind(String input)
        {
            if (input.Contains(this.query))
            {
                return input.Replace(this.query, this.replacement);
            }
            else
            {
                return input;
            }
        }
        public String FileFind(StreamReader input)
        {
            string buffer = new string("");
            string line = input.ReadLine();
            // 提前文本内容
            while (line != null)
            {
                buffer += line + ' ';
                line = input.ReadLine();
            }
            // 删除最后一个空格
            if (buffer.Length - 1 > 0)
            {
                buffer = buffer.Remove(buffer.Length - 1);
            }
            return this.StringFind(buffer);
        }
        public String Find(object input)
        {
            // 判断输入类型
            if (input.GetType().Name == "String")
            {
                return this.StringFind((String)input);
            }
            else
            {
                return this.FileFind((StreamReader)input);
            }
        }
    }
}
