using System;
using System.Diagnostics;
using System.Threading;

namespace Lenneth.Core.Extensions.Utils
{
    public class Profiler
    {
        public static long Profile(Action aAction, int aTries = 5, int aActionRepeats = 1, bool aBoost = true)
        {
            var oldAff = Process.GetCurrentProcess().ProcessorAffinity;
            var oldProcPrior = Process.GetCurrentProcess().PriorityClass;
            var oldThreadPrio = Thread.CurrentThread.Priority;

            if (aBoost)
            {
                Process.GetCurrentProcess().ProcessorAffinity = new IntPtr(1);
                Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.RealTime;
                Thread.CurrentThread.Priority = ThreadPriority.Highest;
            }

            var result = long.MaxValue;
            var sw = new Stopwatch();

            try
            {
                for (var i = 0; i < aTries; i++)
                {
                    sw.Restart();

                    for (var j = 0; j < aActionRepeats; j++)
                        aAction();

                    sw.Stop();

                    if (sw.ElapsedMilliseconds < result)
                        result = sw.ElapsedMilliseconds;
                }
            }
            finally
            {
                if (aBoost)
                {
                    Process.GetCurrentProcess().ProcessorAffinity = oldAff;
                    Process.GetCurrentProcess().PriorityClass = oldProcPrior;
                    Thread.CurrentThread.Priority = oldThreadPrio;
                }
            }

            return result / aActionRepeats;
        }
    }
}