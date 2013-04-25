using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GTA
{
    class Citizen : Person
    {
        public MovingEntity enemy; 

        public Citizen()
        {
            SteeringBehaviors = new SteeringBehaviors(this);
            _personTexture = new AnimatedTexture(Vector2.Zero, 0, 1, 0);
            Mass = 1f;
            SteeringBehaviors.WanderOn();
            Heading = new Vector2(1);
            MaxSpeed = 100f;
        }

        public override void Update(float timeElapsed)
        {
            SteeringBehaviors.SetTarget(enemy.Pos);
            SteeringForce = SteeringBehaviors.Calculate();
            Rotation = SteeringForce;
            Vector2 acceleration = SteeringForce / Mass;

            Velocity += acceleration * timeElapsed;

            //Velocity = VectorHelper.MaxLimit(Velocity, MaxSpeed);
            if (Velocity.Length() > 0.001)
                Rotation = Velocity * timeElapsed;
            Pos += Velocity * timeElapsed;

            if (Pos.X > 1600) { Pos = new Vector2(0, Pos.Y); }
            if (Pos.X < 0) { Pos = new Vector2(1600, Pos.Y); }
            if (Pos.Y < 0) { Pos = new Vector2(Pos.X, 900); }
            if (Pos.Y > 900) { Pos = new Vector2(Pos.X, 0);}

            _personTexture.UpdateFrame(TimeEllapsed);
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            _personTexture.DrawFrame(spriteBatch, Pos, _sourceY);
        }

        public override void Load(GraphicsDevice graphicsDevice, ContentManager content)
        {
            _personTexture.Load(graphicsDevice, content, "people2", Frames, FramesPerSec);
        }
    }
}
