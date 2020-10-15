using System.Collections.Generic;
using System.Diagnostics;
using System.Collections.Concurrent;
using System.Threading;
using System.Reflection;

namespace Tracer
{
    public class TracerClass : ITracer
    {
        private TraceResult traceResult = new TraceResult();
        private ConcurrentDictionary<int, Stack<(Methods, Stopwatch)>> threadDictionary = new ConcurrentDictionary<int, Stack<(Methods, Stopwatch)>>();
        public TraceResult GetTraceResult()
        {
            return traceResult;
        }

        public void StartTrace()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            StackFrame frame = new StackFrame(1);
            MethodBase frameMethod = frame.GetMethod();

            Methods method = new Methods();
            method.ClassName = frameMethod.DeclaringType.Name;
            method.Name = frameMethod.Name;

            int ThreadId = Thread.CurrentThread.ManagedThreadId;

            if (threadDictionary.TryAdd(ThreadId, new Stack<(Methods, Stopwatch)>()))
            {
                traceResult.Threads.Add(new Threads { id = ThreadId });
            }

            threadDictionary[ThreadId].Push((method, stopwatch));
        }

        public void StopTrace()
        {
            int ThreadId = Thread.CurrentThread.ManagedThreadId;
            (Methods ThisMethod, Stopwatch stopwatch) = threadDictionary[ThreadId].Pop();
            stopwatch.Stop();
            ThisMethod.Time = stopwatch.ElapsedMilliseconds;

            if (threadDictionary[ThreadId].Count != 0)
            {
                (Methods PreMethod, Stopwatch preStopwatch) = threadDictionary[ThreadId].Peek();
                if (PreMethod.Methods == null)
                {
                    PreMethod.Methods = new List<Methods>();
                }
                PreMethod.Methods.Add(ThisMethod);
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
