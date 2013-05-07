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
        private readonly List<BaseGameEntity> _entities;
        private MovingEntity thug;
        private World()
        {
            thug = new Thug() {Pos = new Vector2D(800, 450), _sourceY = 0};
            _entities = new List<BaseGameEntity>();
            Random rand = new Random();
            for (int i = 0; i < 1000; i++)
            {
                bool seek = rand.Next(0, 2) == 1;
                bool flee = !seek;
                bool wander = !seek;

                int citizenNr = rand.Next(1, 7);

                var citizen = new Citizen() { Pos = new Vector2D(rand.Next(0, 1600), rand.Next(0, 800)), _sourceY = citizenNr * 16, enemy = thug, Flee = flee, Wander = wander, Seek = seek };
                _entities.Add(citizen);
            }
        }

        public static World GetInstance()
        {
            return _instance ?? (_instance = new World());
        }

        public void Load(GraphicsDevice graphicsDevice, ContentManager content)
        {
            thug.Load(graphicsDevice, content);
            foreach (var entity in _entities)
                entity.Load(graphicsDevice, content);
        }

        public void Update(float timeElapsed)
        {
            thug.Update(timeElapsed);
            foreach (var entity in _entities)
                entity.Update(timeElapsed);
        }

        public void Render(SpriteBatch spriteBatch)
        {
            thug.Render(spriteBatch);
            foreach (var entity in _entities)
                entity.Render(spriteBatch);
        }

        public void UpdateThug(Keys key)
        {
            int speed = 5;
            if (key == Keys.Up)
            {
                thug.Pos = new Vector2D(thug.Pos.X, thug.Pos.Y - speed);
            }
            if (key == Keys.Down)
            {
                thug.Pos = new Vector2D(thug.Pos.X, thug.Pos.Y + speed);
            }
            if (key == Keys.Left)
            {
                thug.Pos = new Vector2D(thug.Pos.X - speed, thug.Pos.Y);
            }
            if (key == Keys.Right)
            {
                thug.Pos = new Vector2D(thug.Pos.X + speed, thug.Pos.Y);
            }
        }
    }
}
