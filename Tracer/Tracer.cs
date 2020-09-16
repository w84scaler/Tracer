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
        private Dictionary<int, Stack<(Methods, Stopwatch)>> threadDictionary = new Dictionary<int, Stack<(Methods, Stopwatch)>>();
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
            }

            int ThreadId = Thread.CurrentThread.ManagedThreadId;

            if (!threadDictionary.ContainsKey(ThreadId))
            {
                traceResult.Threads.Add(new Threads { id = ThreadId });
                threadDictionary.Add(ThreadId, new Stack<(Methods, Stopwatch)>());
            }

            threadDictionary[ThreadId].Push((method, stopwatch));
        }

        public void StopTrace()
        {
            int ThreadId = Thread.CurrentThread.ManagedThreadId;

            (Methods ThisMethod, Stopwatch stopwatch) = threadDictionary[ThreadId].Pop();
            ThisMethod.Time = stopwatch.ElapsedMilliseconds;
            if (threadDictionary[ThreadId].Count != 0)
            {
                (Methods PreMethod, Stopwatch preStopwatch) = threadDictionary[ThreadId].Pop();
                if (PreMethod.Methods == null)
                {
                    PreMethod.Methods = new List<Methods>();
                }
                PreMethod.Methods.Add(ThisMethod);
                threadDictionary[ThreadId].Push((PreMethod, preStopwatch));
            }
            else
            {
                int ThreadIndex = traceResult.Threads.FindIndex(_thread => _thread.id == ThreadId);
                if (traceResult.Threads[ThreadIndex].Methods == null)
                {
                    traceResult.Threads[ThreadIndex].Methods = new List<Methods>();
                }
                traceResult.Threads[ThreadIndex].Methods.Add(ThisMethod);
                traceResult.Threads[ThreadIndex].Time += ThisMethod.Time;
            }    
        }
    }
}
