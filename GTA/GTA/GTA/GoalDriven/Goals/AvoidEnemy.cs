using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTA
{
    class AvoidEnemy : CompositeGoal
    {
        public AvoidEnemy(MovingEntity owner)
        {
            Owner = owner;
            SubGoals = new Stack<Goal>();
        }

        public override void Activate()
        {
            StatusOfGoal = Status.Active;
        }

        public override Status Process()
        {
            if(StatusOfGoal == Status.Inactive)
                Activate();

            if(Owner.isEnemyClose())
                AddSubgoal(new Flee(Owner));

            StatusOfGoal = ProcessSubgoals();

            return StatusOfGoal;
        }

        public override void Terminate()
        {
            RemoveAllSubgoals();
            StatusOfGoal = Status.Inactive;
        }

        public override string ToString()
        {
            return "Avoid Enemy";
        }
    }
}
