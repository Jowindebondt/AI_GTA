﻿using System;
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
            Brain = new Think(this);
            Brain.Activate();
            Strength = 0;

            fm = new FuzzyModule();

            Fuzzy_Desirability = fm.CreateFLV("Desirability");
            FuzzySet Low_Desirability = Fuzzy_Desirability.AddLeftShoulderSet("Low", 0, 100, 200);
            FuzzySet Average_Desirability = Fuzzy_Desirability.AddTriangleSet("Average", 100, 200, 375);
            FuzzySet Very_Desirability = Fuzzy_Desirability.AddRightShoulderSet("Very", 200, 375, 500);
            
            Fuzzy_DistanceToTarget = fm.CreateFLV("Distance to Target");
            FuzzySet TargetClose = Fuzzy_DistanceToTarget.AddLeftShoulderSet("TargetClose", 0, 300, 800);
            FuzzySet TargetMedium = Fuzzy_DistanceToTarget.AddTriangleSet("TargetMedium", 300, 800, 1300);
            FuzzySet TargetFar = Fuzzy_DistanceToTarget.AddRightShoulderSet("TargetFar", 800, 1300, 1600);

            Fuzzy_Strength = fm.CreateFLV("Strength");
            FuzzySet Weak_Strength = Fuzzy_Strength.AddLeftShoulderSet("Weak", 0, 10000, 50000);
            FuzzySet Average_Strength = Fuzzy_Strength.AddTriangleSet("Average", 10000, 50000, 75000);
            FuzzySet Strong_Strength = Fuzzy_Strength.AddRightShoulderSet("Strong", 50000, 75000, 100000);

            fm.AddRule(new FuzzyAnd(Strong_Strength, TargetClose), Very_Desirability);
            fm.AddRule(new FuzzyAnd(Strong_Strength, TargetMedium), Average_Desirability);
            fm.AddRule(new FuzzyAnd(Strong_Strength, TargetFar), Low_Desirability);

            fm.AddRule(new FuzzyAnd(Weak_Strength, TargetClose), Low_Desirability);
            fm.AddRule(new FuzzyAnd(Weak_Strength, TargetMedium), Average_Desirability);
            fm.AddRule(new FuzzyAnd(Weak_Strength, TargetFar), Very_Desirability);

            fm.AddRule(new FuzzyAnd(Average_Strength, TargetMedium), Average_Desirability);
            fm.AddRule(new FuzzyAnd(Average_Strength, TargetClose), Average_Desirability);
            fm.AddRule(new FuzzyAnd(Average_Strength, TargetFar), Average_Desirability);
        }

        public override void Update(float timeElapsed)
        {
            TimeEllapsed = timeElapsed;

            CalculateDistance();
            
            Brain.Arbitrate();
            Brain.Process();
            
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

            Strength++;
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            if (World.GetInstance()._drawBrain)
                DrawGdbText(Brain, Pos.X + 10, Pos.Y - 20, spriteBatch);

            _personTexture.DrawFrame(spriteBatch, Pos, _sourceY, Heading);
        }

        private void DrawGdbText(Goal currentGoal, double x, double y, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Font, currentGoal.ToString(), new Vector2((float)x, (float)y),
                                       new Color(0, 0, 0), 0f, Vector2.Zero, .7f, SpriteEffects.None, 0f);

            if (currentGoal.GetType().BaseType == typeof(CompositeGoal))
            {
                for (int i = 0; i < ((CompositeGoal) currentGoal).SubGoals.Count; i ++)
                {
                    Goal nextGoal = ((CompositeGoal) currentGoal).SubGoals.ElementAt(i);
                    DrawGdbText(nextGoal, x + 15, y + ((i + 1) * 15), spriteBatch);
                }
            }
        }

        public override void Load(GraphicsDevice graphicsDevice, ContentManager content)
        {
            _personTexture.Load(graphicsDevice, content, "people2", Frames, FramesPerSec);
            Font = content.Load<SpriteFont>("font");
        }

        public override void CalculateDistance()
        {
            double distanceX = Pos.X - enemy.Pos.X;
            double distanceY = Pos.Y - enemy.Pos.Y;

            if (distanceX < 0)
                distanceX *= -1;

            if (distanceY < 0)
                distanceY *= -1;

            DistanceToTarget = Math.Sqrt(Math.Pow(distanceX, 2) + Math.Pow(distanceY, 2));
        }

        public override bool isEnemyClose()
        {
            int radius = 200;

            if (DistanceToTarget < 200)
                return true;

            return false;
        }
    }
}
