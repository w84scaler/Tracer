using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Tracer
{
    public class TracerClass : ITracer
    {
        private Stopwatch time = new Stopwatch();
        private TraceResult traceResult = new TraceResult();
        private Stack<Methods> stack = new Stack<Methods>();
        public TraceResult GetTraceResult()
        {
            return traceResult;
        }

        public void StartTrace()
        {
            time.Reset();
            time.Start();
            Methods _methods = new Methods();

            StackFrame frame = new StackFrame(1);
            var method = frame.GetMethod();
            _methods.ClassName = method.DeclaringType.ToString();
            _methods.Name = method.Name;

            if (traceResult.Threads == null)
            {
                traceResult.Threads = new List<Threads>();
                traceResult.Threads.Add(new Threads());
            }

            stack.Push(_methods);
        }

        public void StopTrace()
        {
            time.Stop();
            Methods ThisMethod = stack.Pop();
            ThisMethod.Time = time.ElapsedMilliseconds;
            if (stack.Count != 0)
            {
                Methods PreMethod = stack.Pop();
                if (PreMethod.Methods == null)
                {
                    PreMethod.Methods = new List<Methods>();
                }
                PreMethod.Methods.Add(ThisMethod);
                stack.Push(PreMethod);
            }
            else
            {
                if (traceResult.Threads[0].Methods == null)
                {
                    traceResult.Threads[0].Methods = new List<Methods>();
                }
                traceResult.Threads[0].Methods.Add(ThisMethod);
            }    
        }
    }
}
