using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTA;

namespace GTA
{
    public class Think : CompositeGoal
    {
        private Random rand;

        public Think(MovingEntity owner)
        {
            Owner = owner;
            SubGoals = new Stack<Goal>();
            rand = new Random();
        }

        public override void Activate()
        {
            StatusOfGoal = Status.Active;
        }

        public override Status Process()
        {
            if(StatusOfGoal == Status.Inactive)
                Activate();

            return ProcessSubgoals();
        }

        public override void Terminate()
        {
            StatusOfGoal = Status.Failed;
        }

        public void Arbitrate()
        {
            if (SubGoals.Count == 0)
            {
                int randNr = rand.Next(0, 200);
                if (randNr%198 == 0)
                    AddSubgoal(new AttackEnemy(Owner));
                else if (randNr%197 == 0)
                    AddSubgoal(new AvoidEnemy(Owner));
                else if (randNr%196 == 0)
                    AddSubgoal(new GoToSafeHouse(Owner));
            }
        }

        public override string ToString()
        {
            return "Think";
        }
    }
}
