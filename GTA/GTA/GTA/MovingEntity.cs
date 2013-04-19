using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GTA
{
    abstract class MovingEntity : BaseGameEntity
    {
        public Vector2 Velocity { get; set; }
        public float Mass { get; set; }
        public Vector2 Heading { get; set; }
        public Vector2 Side { get; set; }
        public float MaxSpeed { get; set; }
        public float MaxForce { get; set; }
        public float MaxTurnRate { get; set; }
        public SteeringBehaviors SteeringBehaviors { get; set; }
        public float TimeEllapsed { get; set; }

        public abstract override void Update(TimeSpan timeElapsed);
        public abstract override void Render(SpriteBatch spriteBatch);
        public abstract override void Load(GraphicsDevice graphicsDevice, ContentManager content);
    }
}
