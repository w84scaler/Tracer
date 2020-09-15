using System;
using System.Collections.Generic;
using System.Text;

namespace Tracer
{
    public class TraceResult
    {
        public List<Threads> Threads;
    }
    public class Item
    {
        public long Time;
        public List<Methods> Methods;
    }
    public class Threads : Item
    {
        public int id;
    }
    public class Methods : Item
    {
        public string Name;
        public string ClassName;
    }
}
