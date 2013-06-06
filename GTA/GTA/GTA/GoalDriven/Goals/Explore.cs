using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTA
{
    class Explore : AtomicGoal
    {
        public Explore(MovingEntity owner)
        {
            Owner = owner;
        }

        public override void Activate()
        {
            StatusOfGoal = Status.Active;
            Owner.Explore = true;
        }

        public override Status Process()
        {
            if (StatusOfGoal == Status.Inactive)
                Activate();

            if(Owner.SteeringBehaviors.ExploreTargets.Count == 0)
                StatusOfGoal = Status.Completed;

            return StatusOfGoal;
        }

        public override void Terminate()
        {
            Owner.Explore = false;
        }

        public override string ToString()
        {
            return "Explore";
        }
    }
}
