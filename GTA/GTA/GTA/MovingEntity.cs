using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GTA
{
    abstract class MovingEntity : BaseGameEntity
    {
        public Vector2 Velocity { get; set; }
        public float Mass { get; set; }
        public Vector2 Heading { get; set; }
        public float MaxSpeed { get; set; }
        public float MaxForce { get; set; }
        public float MaxTurnRate { get; set; }

        public abstract override void Update(TimeSpan timeElapsed);
        public abstract override void Render();
    }
}
