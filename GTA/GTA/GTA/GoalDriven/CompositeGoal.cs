using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTA
{
    class CompositeGoal : Goal
    {
        public Stack<Goal> SubGoals { get; set; }

        public override void Activate()
        {
            throw new NotImplementedException();
        }

        public override Status Process()
        {
            throw new NotImplementedException();
        }

        public override void Terminate()
        {
            throw new NotImplementedException();
        }

        public void AddSubgoal(Goal _goal)
        {
            SubGoals.Push(_goal);
        }

        public void RemoveAllSubgoals()
        {
            foreach (var subGoal in SubGoals)
            {
                subGoal.Terminate();
            }
            SubGoals.Clear();
        }

        public Status ProcessSubgoals()
        {
            while (SubGoals.Count != 0 && (SubGoals.First().StatusOfGoal == Status.Completed || SubGoals.First().StatusOfGoal == Status.Failed)) // remove all completed and failed goals from the front of the subgoal list
            {
                SubGoals.First().Terminate();
                SubGoals.Pop();
            }

            if (SubGoals.Count != 0)
            {
                Status StatusOfSubGoals = SubGoals.First().Process();
                if (StatusOfSubGoals == Status.Completed && SubGoals.Count > 1)
                {
                    return Status.Active;
                }

                return StatusOfSubGoals;
            }
            else
            {
                return Status.Completed;
            }
        }
    }
}
