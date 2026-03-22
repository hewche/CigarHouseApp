using System;
using System.Collections.Generic;
using System.Text;

namespace CigarHouseApp.Helpers
{
    public class Freezer
    {

        private Timer timer;
        private readonly int delayMs;
        private readonly Action action;
        private readonly SynchronizationContext context;

        public Freezer(Action action, int delayMs)
        {
            this.action = action;
            this.context = SynchronizationContext.Current ?? new SynchronizationContext();
            this.delayMs = delayMs;
        }


        public void Execute()
        {
            timer?.Dispose();
            timer = new Timer(_ =>
            {
                context.Post(_ => action(), null);
            }, null, delayMs,Timeout.Infinite);
        }

        public void Dispose()
        {
            timer?.Dispose();   
        }
    }
}
