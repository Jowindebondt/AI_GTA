using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTA
{
    class CompositeGoal : Goal
    {
        public List<Goal> SubGoals { get; set; }

        public override void Activate()
        {
            throw new NotImplementedException();
        }

        public override int Process()
        {
            throw new NotImplementedException();
        }

        public override void Terminate()
        {
            throw new NotImplementedException();
        }

        public void AddSubgoal(Goal _goal)
        {
            SubGoals.Add(_goal);
        }

        public void RemoveAllSubgoals()
        {
            SubGoals.Clear();
        }

        public Status ProcessSubgoals()
        {
            return 0;
        }
    }
}
