using System;
using Microsoft.Xna.Framework;

namespace GTA
{
    class SteeringBehaviors
    {
        private Random rand;
        Vector2 m_vWanderTarget;
        private Vector2 target;

        private Vector2 m_vSteeringForce;

        double m_dWanderJitter;
        double _mDWanderRadius;
        float m_dWanderDistance;
        private MovingEntity _entity;

        private bool useWander;
        private bool useFlee;
        private bool useSeek;
        private bool useArrive;

        private const double PANICDISTANCESQ = 100*100;

        public SteeringBehaviors(MovingEntity entity)
        {
            _entity = entity;
            rand = new Random();
            _mDWanderRadius = 1.2f;
            m_dWanderDistance = 10.0f;
            m_vSteeringForce = new Vector2();
            m_dWanderJitter = 40.0f;
        }

        private Vector2 Flee(Vector2 target)
        {
            if (Vector2.DistanceSquared(_entity.Pos, target) > PANICDISTANCESQ)
                return Vector2.Zero;

            Vector2 desiredVelocity = Vector2.Normalize(_entity.Pos - target) * _entity.MaxSpeed;
            return desiredVelocity - _entity.Velocity;
        }

        private Vector2 Seek(Vector2 target)
        {
            Vector2 desiredVelocity = Vector2.Normalize(target - _entity.Pos) * _entity.MaxSpeed;
            return desiredVelocity - _entity.Velocity;
        }

        private Vector2 Arrive(Vector2 target)
        {
            return new Vector2();
        }

        private Vector2 Wander()
        {
            //this behavior is dependent on the update rate, so this line must
            //be included when using time independent framerate.
            var jitterThisTimeSlice = m_dWanderJitter * _entity.TimeEllapsed;
            if (jitterThisTimeSlice <= 0)
                jitterThisTimeSlice = 1;
            //first, add a small random vector to the target's position
            m_vWanderTarget = new Vector2((float)(RandomClamped() * jitterThisTimeSlice),
                                        (float)(RandomClamped() * jitterThisTimeSlice));

            //reproject this new vector back on to a unit circle
            m_vWanderTarget.Normalize();

            //increase the length of the vector to the same as the radius
            //of the wander circle
            m_vWanderTarget *= (float)_mDWanderRadius;

            //m_vWanderTarget.X *= (float)_mDWanderRadius;
            //m_vWanderTarget.Y *= (float)_mDWanderRadius;

            //move the target into a position WanderDist in front of the agent
            Vector2 target = m_vWanderTarget + new Vector2(m_dWanderDistance, 0);

            //_entity.Side = VectorHelper.GetPerpVector(_entity.Heading);

            //project the target into world space
            Vector2 Target = PointToWorldSpace(target,
                                                 _entity.Heading,
                                                 _entity.Side,
                                                 _entity.Pos);

            //and steer towards it
            return Target - _entity.Pos;
        }

        private Vector2 Explore(Vector2 vector)
        {
            return new Vector2();
        }

        public Vector2 Calculate()
        {
            m_vSteeringForce = Vector2.Zero;

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

            return m_vSteeringForce;
        }

        public Vector2 ForwardComponent(Vector2 vector)
        {
            return new Vector2();
        }

        public Vector2 SideComponent(Vector2 vector)
        {
            return new Vector2();
        }

        public void SetPath()
        {
            
        }

        public void SetTarget(Vector2 vector)
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

        private double RandomClamped()
        {
            return rand.NextDouble()/* - rand.NextDouble()*/;
        }

        private Vector2 PointToWorldSpace(Vector2 point, Vector2 AgentHeading, Vector2 AgentSide, Vector2 AgentPosition)
        {
            Vector2 TransPoint = point;

            //create a transformation matrix
            Matrix matTransform = Matrix.CreateRotationX(MathHelper.ToRadians(Vector2.Dot(AgentHeading, AgentSide)));
            matTransform += Matrix.CreateTranslation(AgentPosition.X, AgentPosition.Y, 0);

            //now transform the vertices
            TransPoint = Vector2.Transform(TransPoint, matTransform);

            return TransPoint;
        }
    }
}
