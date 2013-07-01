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
        public Think Brain { get; set; }
        public bool enemyReached { get; set; }
        public int Strength { get; set; }
        public double DistanceToTarget { get; set; }

        public Point SafeHouse { get; set; }
        public FuzzyModule fm { get; set; }
        public FuzzyVariable Fuzzy_Desirability { get; set; }
        public FuzzyVariable Fuzzy_DistanceToTarget { get; set; }
        public FuzzyVariable Fuzzy_Strength { get; set; }

        public bool Flee { get; set; }
        public bool Wander { get; set; }
        public bool Seek { get; set; }
        public bool Explore { get; set; }
        public bool AStar { get; set; }

        public abstract override void Update(float timeElapsed);
        public abstract override void Render(SpriteBatch spriteBatch);
        public abstract override void Load(GraphicsDevice graphicsDevice, ContentManager content);
        public abstract void CalculateDistance();
        public abstract bool isEnemyClose();
    }
}
