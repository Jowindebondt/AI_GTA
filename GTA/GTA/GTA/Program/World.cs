using System.Collections.Generic;
using System;
using GTA.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GTA
{
    class World
    {
        private static World _instance;
        public readonly List<MovingEntity> MovingEntities;
        public readonly List<ObstacleEntity> ObstacleEntities;
        private MovingEntity thug;
        private World()
        {
            MovingEntities = new List<MovingEntity>();
            ObstacleEntities = new List<ObstacleEntity>();
            var rand = new Random();
            
            thug = new Thug() { Pos = new Vector2D(800, 450), _sourceY = 0 };

            MovingEntities.Add(thug);

            for (int i = 0; i < 200; i++)
            {
                bool seek = rand.Next(0, 2) == 1;
                bool flee = !seek;
                bool wander = !seek;

                int citizenNr = rand.Next(1, 7);

                var citizen = new Citizen() { Pos = new Vector2D(rand.Next(0, 1600), rand.Next(0, 800)), _sourceY = citizenNr * 16, enemy = thug, Flee = flee, Wander = wander, Seek = seek };
                //var citizen = new Citizen() { Pos = new Vector2D(rand.Next(0, 1600), rand.Next(0, 800)), _sourceY = citizenNr * 16, enemy = thug, Flee = false, Wander = false, Seek = true };
                MovingEntities.Add(citizen);
            }

            for (int x = 0; x < 25; x++)
            {
                for (int y = 0; y < 14; y++)
                {
                    if (y == 0 || y == 14)
                    {
                        ObstacleEntities.Add(new Building { Pos = new Vector2D(x * 64, y * 64) });
                    }
                    else if(x == 0 || x == 14)
                    {
                        ObstacleEntities.Add(new Building { Pos = new Vector2D(x * 64, y * 64) }); 
                    }
                } 
            }
            //ObstacleEntities.Add(new Building {Pos = new Vector2D(0,0)});
            //ObstacleEntities.Add(new Road {Pos = new Vector2D(64,0)});
            //ObstacleEntities.Add(new Pavement {Pos = new Vector2D(0,64)});
        }

        public static World GetInstance()
        {
            return _instance ?? (_instance = new World());
        }

        public void Load(GraphicsDevice graphicsDevice, ContentManager content)
        {
            thug.Load(graphicsDevice, content);
            foreach (var entity in MovingEntities)
                entity.Load(graphicsDevice, content);
        }

        public void Update(float timeElapsed)
        {
            foreach (var entity in MovingEntities)
                entity.Update(timeElapsed);

            foreach (var entity in ObstacleEntities)
                entity.Update(timeElapsed);
        }

        public void Render(SpriteBatch spriteBatch)
        {
            foreach (var entity in MovingEntities)
                entity.Render(spriteBatch);

            foreach (var entity in ObstacleEntities)
                entity.Render(spriteBatch);
        }

        public void UpdateThug(Keys key)
        {
            const int speed = 5;

            Vector2D oldPos = thug.Pos;
            Vector2D tempHeading = new Vector2D(0, 0);
            
            if (key == Keys.Up)
            {
                thug.Pos = new Vector2D(thug.Pos.X, thug.Pos.Y - speed);
                tempHeading += thug.Pos - oldPos;
            }
            if (key == Keys.Down)
            {
                thug.Pos = new Vector2D(thug.Pos.X, thug.Pos.Y + speed);
                tempHeading += thug.Pos - oldPos;
            }
            if (key == Keys.Left)
            {
                thug.Pos = new Vector2D(thug.Pos.X - speed, thug.Pos.Y);
                tempHeading += thug.Pos - oldPos;
            }
            if (key == Keys.Right)
            {
                thug.Pos = new Vector2D(thug.Pos.X + speed, thug.Pos.Y);
                tempHeading += thug.Pos - oldPos;
            }

            thug.Heading = Vector2D.Vec2DNormalize(tempHeading);

        }

        public void TagAgentsWithinViewRange(BaseGameEntity pEntity, double radius)
        {
            //  tags any entities contained that are within the
            //  radius of the single entity parameter

            //iterate through all entities checking for range
            foreach (MovingEntity curEntity in MovingEntities)
            {
                //first clear any current tag
                curEntity.IsTagged = false;

                Vector2D to = curEntity.Pos - pEntity.Pos;

                //the bounding radius of the other is taken into account by adding it 
                //to the range
                double range = radius + curEntity.Bradius;

                //if entity within range, tag for further consideration. (working in
                //distance-squared space to avoid sqrts)
                if ((curEntity != pEntity) && (to.LengthSq() < range * range))
                {
                    curEntity.IsTagged = true;
                }

            }//next entity
        }

        internal void TagObstaclesWithinViewRange(MovingEntity pEntity, double radius)
        {
            //iterate through all entities checking for range
            foreach (BaseGameEntity curEntity in ObstacleEntities)
            {

                //first clear any current tag
                curEntity.IsTagged = false;

                Vector2D to = curEntity.Pos - pEntity.Pos;

                //the bounding radius of the other is taken into account by adding it 
                //to the range
                double range = radius + curEntity.Bradius;

                //if entity within range, tag for further consideration. (working in
                //distance-squared space to avoid sqrts)
                if ((curEntity != pEntity) && (to.LengthSq() < range * range))
                {
                    curEntity.IsTagged = true;
                }

            }//next entity
        }
    }
}
