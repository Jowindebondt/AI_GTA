using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTA
{
    class PathFinding: AtomicGoal
    {
        public PathFinding(MovingEntity owner)
        {
            Owner = owner;
        }

        public override void Activate()
        {
            StatusOfGoal = Status.Active;
            Owner.AStar = true;

            Node startnode;

            Node endnode; =

            while (startnode != null)
                startnode = World.GetInstance()._graph.GetNodeFromPoint(((int) (Owner.Pos.X/32)*32),
                                                                        (int) (Owner.Pos.Y/32)*32);
            while (endnode != null)
                endnode = World.GetInstance()._graph.GetNodeFromPoint((int) (Owner.SafeHouse.X/32)*32,
                                                                      (int) (Owner.SafeHouse.Y/32)*32;
            Owner.SteeringBehaviors.CreateListAStar(startnode, endnode);
        }

        public override Status Process()
        {
            if(StatusOfGoal == Status.Inactive)
                Activate();

            if(Owner.SteeringBehaviors.AStarTargets.Count == 0)
                StatusOfGoal = Status.Completed;

            return StatusOfGoal;
        }

        public override void Terminate()
        {
            Owner.AStar = false;
            Owner.SteeringBehaviors.AStarTargets.Clear();
            foreach (var node in World.GetInstance()._graph.getNodes())
            {
                node.Previous = null;
                node.DistanceToGoal = -1;
                node.aStarVisited = false;
                node.DistanceFromStart = -1;
            }
        }

        public override string ToString()
        {
            return "Pathfinding";
        }
    }
}
