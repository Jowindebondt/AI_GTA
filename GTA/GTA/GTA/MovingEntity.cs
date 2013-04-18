using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GTA
{
    abstract class MovingEntity : BaseGameEntity
    {
        private Vector2 _velocity;
        private float _mass;
        private Vector2 _heading;
        private float _maxSpeed;
        private float _maxForce;
        private float _maxTurnRate;

        public abstract override void Update(int time_elapsed);
        public abstract override void Render();

        public Vector2 GetVelocity()
        {
            return _velocity;
        }

        public float GetMass()
        {
            return _mass;
        }

        public Vector2 GetHeading()
        {
            return _heading;
        }

        public float GetMaxSpeed()
        {
            return _maxSpeed;
        }

        public float GetMaxForce()
        {
            return _maxForce;
        }

        public float GetMaxTurnRate()
        {
            return _maxTurnRate;
        }
    }
}
