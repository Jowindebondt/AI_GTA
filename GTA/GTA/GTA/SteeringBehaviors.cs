using System;
using System.Collections.Generic;
using System.Linq;
using GTA.Common;
using Microsoft.Xna.Framework;

namespace GTA
{
    class SteeringBehaviors
    {
        private Random rand;
        Vector2D m_vWanderTarget;
        private Vector2D target;

        private Vector2D m_vSteeringForce;

        public double m_dWanderJitter;
        public double _mDWanderRadius;
        public float m_dWanderDistance;
        public double m_dViewDistance;
        public double m_dWeightSeparation;
        public double m_dDBoxLength;
        public double MinDetectionBoxLength;
        public double m_dWeightObstacleAvoidance;

        private MovingEntity _entity;
        private Queue<Vector2D> ExploreTargets;
        private List<Node> AStarTargets;

        private bool useWander;
        private bool useFlee;
        private bool useSeek;
        private bool useArrive;
        private bool useExplore;
        private bool useAStar;

        private const double PANICDISTANCESQ = 100*100;

        private MovingEntity m_pTargetAgent1;

        public SteeringBehaviors(MovingEntity entity, MovingEntity enemy)
        {
            _entity = entity;
            rand = new Random();
            _mDWanderRadius = 2.5f;
            m_dWanderDistance = 3f;
            m_vSteeringForce = new Vector2D();
            m_dWanderJitter = 40f;

            m_dViewDistance = 40;
            m_dWeightSeparation = 5000;

            MinDetectionBoxLength = 20;
            m_dWeightObstacleAvoidance = 9000000;

            double rotation = VectorHelper.RandFloat() * (Math.PI * 2);
            m_vWanderTarget = new Vector2D(_mDWanderRadius * Math.Cos(rotation), _mDWanderRadius * Math.Sin(rotation));
            m_pTargetAgent1 = enemy;

            AStarTargets = new List<Node>();
            ExploreTargets = new Queue<Vector2D>();

            ExploreTargets.Enqueue(new Vector2D(100,100)); //Left-Top
            ExploreTargets.Enqueue(new Vector2D(100, 500));//Left-Bottom
            ExploreTargets.Enqueue(new Vector2D(800, 500));//Mid-Bottom
            ExploreTargets.Enqueue(new Vector2D(800, 100));//Mid-Top
            ExploreTargets.Enqueue(new Vector2D(1500, 100)); //Right-Top
            ExploreTargets.Enqueue(new Vector2D(1500, 500)); //Right-Bottom
            ExploreTargets.Enqueue(new Vector2D(800, 450)); //Mid-Mid
            ExploreTargets.Enqueue(new Vector2D(100, 450)); // Mid-Left
            ExploreTargets.Enqueue(new Vector2D(1500, 450)); // Mid-Right
        }

        public void CreateListAStar(Node start, Node target)
        {
            List<Node> closedSet = new List<Node>(); // Set of nodes that are already evaluated.
            AStarQueue openSet = new AStarQueue();
            //Queue<Node> openSet = new Queue<Node>();     //Set of tentative nodes to be evaluated, initially containing the start node
            List<Node> cameFrom = new List<Node>();    //List of navigated nodes
            int maxDistanceAtcf, distanceATCFLeft;
            List<Node> path = new List<Node>();

            openSet.Enqueue(start);
            start.DistanceFromStart = 0;
            start.DistanceToGoal = (int)heuristicCostEstimated(start, target);
            start.TotalCost = start.DistanceFromStart + start.DistanceToGoal;
            maxDistanceAtcf = start.DistanceToGoal;
            distanceATCFLeft = start.DistanceToGoal;

            int bestScore = 0;

            while (openSet.GetLength() != 0)
            {
                var current = openSet.Dequeue();
                closedSet.Add(current);
                current.aStarVisited = true;

                bool _tentativeIsBetter;

                if (current == target)
                {
                    AStarTargets = ReconstructPath(current);
                    break;
                }

                foreach (var edge in current._edges)
                {
                    Node nextNode = edge._nextNode;
                    var tentativeGScore = current.DistanceFromStart + DistBetween(current, nextNode);

                    if (nextNode.aStarVisited && nextNode.DistanceFromStart < tentativeGScore) continue;

                    if (!openSet.Contains(nextNode) || tentativeGScore < nextNode.DistanceFromStart)
                    {
                        nextNode.DistanceToGoal = (int) heuristicCostEstimated(nextNode, target);
                        nextNode.Previous = current;
                        nextNode.DistanceFromStart = (int) tentativeGScore;
                        nextNode.TotalCost = edge._nextNode.DistanceFromStart + edge._nextNode.DistanceToGoal;

                        if (!openSet.Contains(nextNode))
                            openSet.Enqueue(nextNode);
                    }
                }
            }
            ResetNodes(closedSet);
        }

        private List<Node> ReconstructPath(Node current)
        {
            var output = new List<Node>();

            if (current.Previous != null)
                output = ReconstructPath(current.Previous);
            
            output.Add(current);

            return output;
        }

        private void ResetNodes(List<Node> nodes)
        {
            foreach (var node in nodes)
            {
                node.DistanceToGoal = -1;
                node.aStarVisited = false;
                node.DistanceFromStart = -1;
            }
        }

        private int DistBetween(Node current, Node nextNode)
        {
            Edge edge = current._edges.First(e => e._nextNode == nextNode);

            if (edge._cost > 0)
                return edge._cost;

            var currentX = current._p.X;
            var currentY = current._p.Y;
            var neighborX = nextNode._p.X;
            var neighborY = nextNode._p.Y;

            var differenceX = currentX - neighborX;
            var differenceY = currentY - neighborY;

            var cost = (int)Math.Sqrt(Math.Pow(differenceX, 2) + Math.Pow(differenceY, 2));

            edge._cost = cost;

            return cost;
        }

        private double heuristicCostEstimated(Node current, Node goal)
        {
            var currentX = current._p.X;
            var currentY = current._p.Y;
            var goalX = goal._p.X;
            var goalY = goal._p.Y;

            var differenceX = currentX - goalX;
            var differenceY = currentY - goalY;

            var total = (Math.Sqrt(Math.Pow(differenceX, 2) + Math.Pow(differenceY, 2)));

            return total;
        }

        private Vector2D Flee(Vector2D target)
        {
            if (Vector2D.Vec2DDistanceSq(_entity.Pos, target) > PANICDISTANCESQ)
                return new Vector2D(0,0);

            Vector2D desiredVelocity = Vector2D.Vec2DNormalize(_entity.Pos - target) * _entity.MaxSpeed;
            return desiredVelocity - _entity.Velocity;
        }

        private Vector2D Seek(Vector2D target)
        {
            Vector2D desiredVelocity = Vector2D.Vec2DNormalize(target - _entity.Pos) * _entity.MaxSpeed;
            return desiredVelocity - _entity.Velocity;
        }

        private Vector2D Arrive(Vector2D target)
        {
            return new Vector2D();
        }

        private Vector2D Wander()
        {
            //this behavior is dependent on the update rate, so this line must
            //be included when using time independent framerate.
            var jitterThisTimeSlice = m_dWanderJitter * _entity.TimeEllapsed;

            if (jitterThisTimeSlice <= 0)
                jitterThisTimeSlice = 1;
            
            //first, add a small random vector to the target's position
            m_vWanderTarget += new Vector2D((float)(VectorHelper.RandomClamped() * jitterThisTimeSlice),
                                        (float)(VectorHelper.RandomClamped() * jitterThisTimeSlice));

            //reproject this new vector back on to a unit circle
            m_vWanderTarget.Normalize();

            //increase the length of the vector to the same as the radius
            //of the wander circle
            m_vWanderTarget *= (float)_mDWanderRadius;

            //move the target into a position WanderDist in front of the agent
            Vector2D target = m_vWanderTarget + new Vector2D(m_dWanderDistance, 0);

            //project the target into world space
            _entity.Target = PointToWorldSpace(target,
                                                 _entity.Heading,
                                                 _entity.Side,
                                                 _entity.Pos);

            //and steer towards it
            return _entity.Target - _entity.Pos;
        }

        private Vector2D Explore()
        {
            Vector2D exploreTarget = ExploreTargets.Peek();

            double toTarget = (exploreTarget - _entity.Pos).Length();

            if (toTarget < 10d)
                ExploreTargets.Enqueue(ExploreTargets.Dequeue());

            Vector2D desiredVelocity = Vector2D.Vec2DNormalize(exploreTarget - _entity.Pos) * _entity.MaxSpeed;
            return desiredVelocity - _entity.Velocity;
        }

        private Vector2D AStar()
        {
            if (AStarTargets.Count == 0)
                return Explore();
            else
            {
                Vector2D vector = new Vector2D(AStarTargets[0]._p.X, AStarTargets[0]._p.Y);

                double toTarget = (vector - _entity.Pos).Length();

                if (toTarget < 10d)
                    AStarTargets.RemoveAt(0);

                Vector2D desiredVelocity = Vector2D.Vec2DNormalize(vector - _entity.Pos)*_entity.MaxSpeed;
                return desiredVelocity - _entity.Velocity;
            }
        }

        public Vector2D ObstacleAvoidance(List<ObstacleEntity> obstacles)
        {
            //the detection box length is proportional to the agent's velocity
            m_dDBoxLength = MinDetectionBoxLength +
                            (_entity.Speed / _entity.MaxSpeed) * MinDetectionBoxLength;


            //this will keep track of the closest intersecting obstacle (CIB)
            BaseGameEntity ClosestIntersectingObstacle = null;

            //this will be used to track the distance to the CIB
            double DistToClosestIP = Double.MaxValue;

            //this will record the transformed local coordinates of the CIB
            Vector2D ClosestObstacleLocalPos = new Vector2D();

            foreach (BaseGameEntity curOb in obstacles)
            {
                //if the obstacle has been tagged within range proceed
                if (curOb.IsTagged)
                {
                    //calculate this obstacle's position in local space
                    Vector2D LocalPos = PointToLocalSpace(curOb.Pos,
                                                           _entity.Heading,
                                                           _entity.Side,
                                                           _entity.Pos);

                    //if the local position has a negative x value then it must lay
                    //behind the agent. (in which case it can be ignored)
                    if (LocalPos.X >= 0)
                    {
                        //if the distance from the x axis to the object's position is less
                        //than its radius + half the width of the detection box then there
                        //is a potential intersection.
                        double ExpandedRadius = curOb.Bradius + _entity.Bradius;

                        if (Math.Abs(LocalPos.Y) < ExpandedRadius)
                        {
                            //now to do a line/circle intersection test. The center of the 
                            //circle is represented by (cX, cY). The intersection points are 
                            //given by the formula x = cX +/-sqrt(r^2-cY^2) for y=0. 
                            //We only need to look at the smallest positive value of x because
                            //that will be the closest point of intersection.
                            double cX = LocalPos.X;
                            double cY = LocalPos.Y;

                            //we only need to calculate the sqrt part of the above equation once
                            double SqrtPart = Math.Sqrt(ExpandedRadius * ExpandedRadius - cY * cY);

                            double ip = cX - SqrtPart;

                            if (ip <= 0.0)
                            {
                                ip = cX + SqrtPart;
                            }

                            //test to see if this is the closest so far. If it is keep a
                            //record of the obstacle and its local coordinates
                            if (ip < DistToClosestIP)
                            {
                                DistToClosestIP = ip;

                                ClosestIntersectingObstacle = curOb;

                                ClosestObstacleLocalPos = LocalPos;
                            }
                        }
                    }
                }
            }

            //if we have found an intersecting obstacle, calculate a steering 
            //force away from it
            Vector2D SteeringForce = new Vector2D(0.0, 0.0);

            if (ClosestIntersectingObstacle != null)
            {
                //the closer the agent is to an object, the stronger the 
                //steering force should be
                double multiplier = 1.0 + (m_dDBoxLength - ClosestObstacleLocalPos.X) / m_dDBoxLength;

                //calculate the lateral force                                

                // Check to see if we could use a hint on choosing a more 
                // efficient direction to turn when avoiding the obstacle
                Vector2D targetLoc = null;

                //if (On(behavior_type.follow_path) && (!m_pPath.Finished()))
                //{
                //    targetLoc = m_pPath.CurrentWaypoint();
                //}
                //else if ((On(behavior_type.seek) || On(behavior_type.arrive)) && (!Vector2D.IsNull(GameWorld.Instance.TargetPos)))
                //{
                //    targetLoc = GameWorld.Instance.TargetPos;
                //}
                //else if ((On(behavior_type.pursuit) || On(behavior_type.offset_pursuit)) && (m_pTargetAgent1 != null))
                //{
                //    targetLoc = m_pTargetAgent1.Pos;
                //}

                if (!Vector2D.IsNull(targetLoc))
                {
                    // Get normalised direction to obstacle from current location
                    Vector2D dirToObs = ClosestIntersectingObstacle.Pos - _entity.Pos;
                    dirToObs.Normalize();

                    // Calculate the two "apex choices" on the obstacles sphere
                    Vector2D interceptRightHand = Vector2D.ProjectedPerp(ClosestIntersectingObstacle.Pos,
                                                                                    dirToObs,
                                                                                    ClosestIntersectingObstacle.Bradius,
                                                                                    false);

                    Vector2D interceptLeftHand = Vector2D.ProjectedPerp(ClosestIntersectingObstacle.Pos,
                                                                                    dirToObs,
                                                                                    ClosestIntersectingObstacle.Bradius,
                                                                                    true);

                    // Calculate and compare the distances to determine the preferred "side" of the sphere.
                    double distRightHand = interceptRightHand.DistanceSq(targetLoc);
                    double distLeftHand = interceptLeftHand.DistanceSq(targetLoc);

                    if (distLeftHand < distRightHand)
                    {
                        multiplier = multiplier * -1; // We will travel on the left hand side of the sphere.
                    }
                }

                SteeringForce.Y = ClosestIntersectingObstacle.Bradius * multiplier;

                //apply a braking force proportional to the obstacles distance from
                //the vehicle. 
                double BrakingWeight = 0.2;

                SteeringForce.X = (ClosestIntersectingObstacle.Bradius -
                                   ClosestObstacleLocalPos.X) *
                                   BrakingWeight;
            }

            //finally, convert the steering vector from local to world space
            Vector2D vecReturn = VectorToWorldSpace(SteeringForce,
                                      _entity.Heading,
                                      _entity.Side);

            return vecReturn;
        }

        private Vector2D Separation(List<MovingEntity> neighbors)
        {
            Vector2D SteeringForce = new Vector2D(0.0, 0.0);

            foreach (MovingEntity neighbor in neighbors)
            {
                //make sure this agent isn't included in the calculations and that
                //the agent being examined is close enough. 
                //***also make sure it doesn't include the evade target ***
                if ((neighbor != _entity) && neighbor.IsTagged &&
                (neighbor != m_pTargetAgent1))
                {
                    Vector2D ToAgent = (_entity.Pos - neighbor.Pos);

                    //scale the force inversely proportional to the agents distance from its neighbor.
                    SteeringForce += Vector2D.Vec2DNormalize(ToAgent) / ToAgent.Length();
                }
            }

            return SteeringForce;
        }

        public Vector2D Calculate()
        {
            m_vSteeringForce = new Vector2D(0,0);

            //tag neighbors if any of the following 3 group behaviors are switched on
            World.GetInstance().TagAgentsWithinViewRange(_entity, m_dViewDistance);
            World.GetInstance().TagObstaclesWithinViewRange(_entity, m_dDBoxLength);

            m_vSteeringForce += ObstacleAvoidance(World.GetInstance().ObstacleEntities) * m_dWeightObstacleAvoidance;
            m_vSteeringForce += Separation(World.GetInstance().MovingEntities) * m_dWeightSeparation;

            if (useWander)
                m_vSteeringForce += Wander();

            if (useFlee)
                m_vSteeringForce += Flee(target);
            
            if (useSeek)
                m_vSteeringForce += Seek(target);

            if (useExplore)
                m_vSteeringForce += Explore();

            if (useAStar)
                m_vSteeringForce += AStar();

            return m_vSteeringForce;
        }

        public Vector2D ForwardComponent(Vector2D vector)
        {
            return new Vector2D();
        }

        public Vector2D SideComponent(Vector2D vector)
        {
            return new Vector2D();
        }

        public void SetPath()
        {
            
        }

        public void SetTarget(Vector2D vector)
        {
            target = vector;
        }

        public void FleeOn()
        {
            useFlee = true;
        }

        public void SeekOn()
        {
            useSeek = true;
        }

        public void ArriveOn()
        {
            useArrive = true;
        }

        public void ExploreOn()
        {
            useExplore = true;
        }

        public void AStarOn()
        {
            useAStar = true;
        }

        public void WanderOn()
        {
            useWander = true;
        }

        public void FleeOff()
        {
            useFlee = false;
        }

        public void SeekOff()
        {
            useSeek = false;
        }

        public void ArriveOff()
        {
            useArrive = false;
        }

        public void ExploreOff()
        {
            useExplore = false;
        }

        public void AStarOff()
        {
            useAStar = false;
        }

        public void WanderOff()
        {
            useWander = false;
        }

        private Vector2D PointToWorldSpace(Vector2D point, Vector2D AgentHeading, Vector2D AgentSide, Vector2D AgentPosition)
        {
            //make a copy of the point
            Vector2D TransPoint = new Vector2D(point.X, point.Y); ;

            //create a transformation matrix
            C2DMatrix matTransform = new C2DMatrix();

            //rotate
            matTransform.Rotate(AgentHeading, AgentSide);

            //and translate
            matTransform.Translate(AgentPosition.X, AgentPosition.Y);

            //now transform the vertices
            matTransform.TransformVector2D(TransPoint);

            return TransPoint;
        }

        private Vector2D PointToLocalSpace(Vector2D point, Vector2D AgentHeading, Vector2D AgentSide, Vector2D AgentPosition)
        {
            //make a copy of the point
            Vector2D TransPoint = new Vector2D(point.X, point.Y);

            //create a transformation matrix
            C2DMatrix matTransform = new C2DMatrix();

            double Tx = -AgentPosition.Dot(AgentHeading);
            double Ty = -AgentPosition.Dot(AgentSide);

            //create the transformation matrix
            matTransform._11(AgentHeading.X); matTransform._12(AgentSide.X);
            matTransform._21(AgentHeading.Y); matTransform._22(AgentSide.Y);
            matTransform._31(Tx); matTransform._32(Ty);

            //now transform the vertices
            matTransform.TransformVector2D(TransPoint);

            return TransPoint;
        }

        private Vector2D VectorToWorldSpace(Vector2D vec, Vector2D AgentHeading, Vector2D AgentSide)
        {
            //make a copy of the point
            Vector2D TransVec = new Vector2D(vec.X, vec.Y); ;

            //create a transformation matrix
            C2DMatrix matTransform = new C2DMatrix();

            //rotate
            matTransform.Rotate(AgentHeading, AgentSide);

            //now transform the vertices
            matTransform.TransformVector2D(TransVec);

            return TransVec;
        }
    }
}
