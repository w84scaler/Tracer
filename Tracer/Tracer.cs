using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace Tracer
{
    public class TracerClass : ITracer
    {
        private TraceResult traceResult = new TraceResult();
        private Dictionary<int, Stack<(Methods, Stopwatch)>> stack = new Dictionary<int, Stack<(Methods, Stopwatch)>>();
        public TraceResult GetTraceResult()
        {
            return traceResult;
        }

        public void StartTrace()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Methods method = new Methods();

            StackFrame frame = new StackFrame(1);
            var frameMethod = frame.GetMethod();
            method.ClassName = frameMethod.DeclaringType.ToString();
            method.Name = frameMethod.Name;

            if (traceResult.Threads == null)
            {
                traceResult.Threads = new List<Threads>();
                traceResult.Threads.Add(new Threads());
            }
            stack.Push((method, stopwatch));
        }

        public void StopTrace()
        {
            
            (Methods ThisMethod, Stopwatch stopwatch) = stack.Pop();
            ThisMethod.Time = stopwatch.ElapsedMilliseconds;
            if (stack.Count != 0)
            {
                (Methods PreMethod, Stopwatch preStopwatch) = stack.Pop(); 
                if (PreMethod.Methods == null)
                {
                    PreMethod.Methods = new List<Methods>();
                }
                PreMethod.Methods.Add(ThisMethod);
                stack.Push((PreMethod, preStopwatch));
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
