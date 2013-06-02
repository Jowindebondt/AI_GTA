using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTA.GoalDriven.Goals
{
    class Wander : AtomicGoal
    {
        public override void Activate()
        {
            StatusOfGoal = Status.Active;
        }

        public override int Process()
        {
            throw new NotImplementedException();
        }

        public override void Terminate()
        {
            throw new NotImplementedException();
        }
    }
}
