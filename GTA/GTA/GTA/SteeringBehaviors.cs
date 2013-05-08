using System;
using System.Collections.Generic;
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

        private MovingEntity _entity;

        private bool useWander;
        private bool useFlee;
        private bool useSeek;
        private bool useArrive;

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
            m_dWeightSeparation = 9000;

            double rotation = VectorHelper.RandFloat() * (Math.PI * 2);
            m_vWanderTarget = new Vector2D(_mDWanderRadius * Math.Cos(rotation), _mDWanderRadius * Math.Sin(rotation));
            m_pTargetAgent1 = enemy;
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

        private Vector2D Explore(Vector2D vector)
        {
            return new Vector2D();
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
            

            if (useWander)
            {
                m_vSteeringForce += Wander();
            }

            if (useFlee)
            {
                m_vSteeringForce += Flee(target);
            }
            
            if (useSeek)
            {
                m_vSteeringForce += Seek(target);
            }

            m_vSteeringForce += Separation(World.GetInstance()._entities) * m_dWeightSeparation;

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
    }
}
