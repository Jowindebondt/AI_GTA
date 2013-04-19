using System;
using Microsoft.Xna.Framework;

namespace GTA
{
    class SteeringBehaviors
    {
        private Random rand;
        Vector2 m_vWanderTarget;
        double m_dWanderJitter;
        double _mDWanderRadius;
        float m_dWanderDistance;
        private MovingEntity _entity;

        public SteeringBehaviors(MovingEntity entity)
        {
            _entity = entity;
            rand = new Random();
            _mDWanderRadius = 1.2;
            m_dWanderDistance = 2.0f;
        }

        private Vector2 Flee(Vector2 target)
        {
            return new Vector2();
        }

        private Vector2 Arrive(Vector2 target)
        {
            return new Vector2();
        }

        private Vector2 Wander(Vector2 _target)
        {
            //this behavior is dependent on the update rate, so this line must
            //be included when using time independent framerate.
            var jitterThisTimeSlice = m_dWanderJitter * _entity.TimeEllapsed;

            //first, add a small random vector to the target's position
            m_vWanderTarget += new Vector2((float)(RandomClamped() * jitterThisTimeSlice),
                                        (float)(RandomClamped() * jitterThisTimeSlice));

            //reproject this new vector back on to a unit circle
            m_vWanderTarget.Normalize();

            //increase the length of the vector to the same as the radius
            //of the wander circle
            m_vWanderTarget.X *= (float)_mDWanderRadius;
            m_vWanderTarget.Y *= (float)_mDWanderRadius;

            //move the target into a position WanderDist in front of the agent
            Vector2 target = _target + new Vector2(m_dWanderDistance, 0);

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
            return new Vector2();
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
            
        }

        public void FleeOn()
        {

        }

        public void ArriveOn()
        {

        }

        public void ExploreOn()
        {
            
        }

        public void WanderOn()
        {

        }

        public void FleeOff()
        {

        }

        public void ArriveOff()
        {

        }

        public void ExploreOff()
        {

        }

        public void WanderOff()
        {

        }

        private double RandomClamped()
        {
            return rand.NextDouble() - rand.NextDouble();
        }

        private Vector2 PointToWorldSpace(Vector2 point, Vector2 AgentHeading, Vector2 AgentSide, Vector2 AgentPosition)
        {
            ////make a copy of the point
            //Vector2 TransPoint = point;
  
            ////create a transformation matrix
            //Matrix.
            //C2DMatrix matTransform;

            ////rotate
            //matTransform.Rotate(AgentHeading, AgentSide);

            ////and translate
            //matTransform.Translate(AgentPosition.x, AgentPosition.y);
	
            ////now transform the vertices
            //matTransform.TransformVector2Ds(TransPoint);

            //return TransPoint;
        }
    }
}
