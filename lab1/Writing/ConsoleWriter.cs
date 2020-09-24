using System;

namespace lab1.Writing
{
    class ConsoleWriter : IWriter
    {
        public void Write(string str)
        {
            Console.WriteLine(str);
        }
    }
}