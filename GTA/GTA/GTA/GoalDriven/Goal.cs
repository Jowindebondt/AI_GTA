using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTA
{
    public enum Status
    {
        Inactive = 0,
        Active = 1,
        Completed = 2,
        Failed = 3
    };

    public abstract class Goal
    {
        protected MovingEntity Owner { get; set; }
        public Status StatusOfGoal { get; set; }

        public abstract void Activate();
        public abstract Status Process();
        public abstract void Terminate();
    }
}
