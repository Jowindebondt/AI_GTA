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
            Owner.SteeringBehaviors.WanderOn();
        }

        public override Status Process()
        {
            if(StatusOfGoal == Status.Inactive)
                this.Activate();

            return StatusOfGoal;
        }

        public override void Terminate()
        {
            Owner.SteeringBehaviors.WanderOff();
        }
    }
}
