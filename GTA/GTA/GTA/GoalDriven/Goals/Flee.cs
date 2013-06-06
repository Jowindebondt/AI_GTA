using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTA
{
    class Flee : AtomicGoal
    {
        public Flee(MovingEntity owner)
        {
            Owner = owner;
        }

        public override void Activate()
        {
            StatusOfGoal = Status.Active;
            Owner.Flee = true;
        }

        public override Status Process()
        {
            if(StatusOfGoal == Status.Inactive)
                Activate();

            if(!Owner.isEnemyClose())
                StatusOfGoal = Status.Completed;

            return StatusOfGoal;
        }

        public override void Terminate()
        {
            Owner.Flee = false;
        }

        public override string ToString()
        {
            return "Flee";
        }
    }
}
