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
    class Citizen : Person
    {
        public MovingEntity enemy;
        public bool Flee;
        public bool Wander;
        public bool Seek;
        public bool Explore;
        public bool AStar;

        public Citizen()
        {
            SteeringBehaviors = new SteeringBehaviors(this, enemy);
            _personTexture = new AnimatedTexture(new Vector2D(16,16), 0, 1, 0);
            Mass = 0.1f;
            double rotation = VectorHelper.RandFloat()*(Math.PI*2);
            Heading = new Vector2D((float)Math.Sin(rotation), (float)-Math.Cos(rotation));
            MaxSpeed = 50f;
            MaxForce = 50f;
            Velocity = new Vector2D(0, 0);
            Side = Heading.Perp();
            Target = new Vector2D(0,0);
            Bradius = 16;
        }

        public override void Update(float timeElapsed)
        {
            TimeEllapsed = timeElapsed;

            if (Flee)
                SteeringBehaviors.FleeOn();
            else
                SteeringBehaviors.FleeOff();

            if (Wander)
                SteeringBehaviors.WanderOn();
            else
                SteeringBehaviors.WanderOff();

            if (Seek)
                SteeringBehaviors.SeekOn();
            else
                SteeringBehaviors.SeekOff();

            if (Explore)
                SteeringBehaviors.ExploreOn();
            else
                SteeringBehaviors.ExploreOff();

            if (AStar)
                SteeringBehaviors.AStarOn();
            else
                SteeringBehaviors.AStarOff();

            SteeringBehaviors.SetTarget(enemy.Pos);

            SteeringForce = SteeringBehaviors.Calculate();
            
            Vector2D acceleration = SteeringForce / Mass;

            Velocity += acceleration * timeElapsed;

            Velocity.Truncate(MaxSpeed);

            Pos += Velocity * timeElapsed;

            if (Velocity.LengthSq() > 0.00000001)
            {
                Heading = Vector2D.Vec2DNormalize(Velocity);
                Side = Heading.Perp();
            }

            if (Pos.X > 1600) { Pos = new Vector2D(0, Pos.Y); }
            if (Pos.X < 0) { Pos = new Vector2D(1600, Pos.Y); }
            if (Pos.Y < 0) { Pos = new Vector2D(Pos.X, 900); }
            if (Pos.Y > 900) { Pos = new Vector2D(Pos.X, 0);}

            _personTexture.UpdateFrame(TimeEllapsed);
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            _personTexture.DrawFrame(spriteBatch, Pos, _sourceY, Heading);
        }

        public override void Load(GraphicsDevice graphicsDevice, ContentManager content)
        {
            _personTexture.Load(graphicsDevice, content, "people2", Frames, FramesPerSec);
        }
    }
}
