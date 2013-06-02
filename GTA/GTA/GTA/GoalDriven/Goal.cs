using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTA
{
    internal enum Status
    {
        Inactive = 0,
        Active = 1,
        Completed = 2,
        Failed = 3
    };

    public abstract class Goal
    {
        private MovingEntity Owner { get; set; }
        protected Status StatusOfGoal { get; set; }

        public abstract void Activate();
        public abstract int Process();
        public abstract void Terminate();
    }
}
