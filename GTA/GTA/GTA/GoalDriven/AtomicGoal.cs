using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTA
{
    abstract class AtomicGoal : Goal
    {
        public abstract override void Activate();
        public abstract override Status Process();
        public abstract override void Terminate();
    }
}
