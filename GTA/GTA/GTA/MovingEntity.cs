using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTA.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GTA
{
    public abstract class MovingEntity : BaseGameEntity
    {
        public Vector2D Velocity { get; set; }
        public float Mass { get; set; }
        public Vector2D Heading { get; set; }
        public Vector2D Side { get; set; }
        public float MaxSpeed { get; set; }
        public float MaxForce { get; set; }
        public float MaxTurnRate { get; set; }
        public float Speed { get; set; }
        public SteeringBehaviors SteeringBehaviors { get; set; }
        public float TimeEllapsed { get; set; }
        public Vector2D SteeringForce { get; set; }
        public Vector2D Rotation { get; set; }
        public Vector2D Target { get; set; }

        public abstract override void Update(float timeElapsed);
        public abstract override void Render(SpriteBatch spriteBatch);
        public abstract override void Load(GraphicsDevice graphicsDevice, ContentManager content);
    }
}
