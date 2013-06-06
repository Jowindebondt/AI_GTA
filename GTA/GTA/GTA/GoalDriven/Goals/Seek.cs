using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTA
{
    class Seek : AtomicGoal
    {
        public Seek(MovingEntity owner)
        {
            Owner = owner;
        }

        public override void Activate()
        {
            StatusOfGoal = Status.Active;
            Owner.Seek = true;
        }

        public override Status Process()
        {
            if(StatusOfGoal == Status.Inactive)
                Activate();

            return StatusOfGoal;
        }

        public override void Terminate()
        {
            Owner.Seek = false;
        }

        public override string ToString()
        {
            return "Seek";
        }
    }
}
