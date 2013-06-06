using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTA
{
    class GoToSafeHouse: CompositeGoal
    {
        private Goal pathFinding;
        private Goal flee;
        public GoToSafeHouse(MovingEntity owner)
        {
            Owner = owner;
            SubGoals = new Stack<Goal>();
            pathFinding = new PathFinding(Owner);
            flee = new Flee(Owner);

            AddSubgoal(pathFinding);
        }

        public override void Activate()
        {
            StatusOfGoal = Status.Active;
        }

        public override Status Process()
        {
            if(StatusOfGoal == Status.Inactive)
                Activate();

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
            return "Go To SafeHouse";
        }
    }
}
