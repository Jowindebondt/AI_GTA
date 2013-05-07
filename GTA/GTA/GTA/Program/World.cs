using System.Collections.Generic;
using System;
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
            thug = new Thug() {Pos = new Vector2(800, 450), _sourceY = 0};
            Citizen c1 = new Citizen() {Pos = new Vector2(36, 20), _sourceY = 16, enemy = thug, Flee = true, Wander = true, Seek = false};
            Citizen c2 = new Citizen() { Pos = new Vector2(52, 20), _sourceY = 32, enemy = thug, Flee = false, Wander = false, Seek = true };

            _entities = new List<BaseGameEntity> { 
                c1,
                c2
                /*new Citizen() { Pos = new Vector2(68, 20), _sourceY = 48},
                new Citizen() { Pos = new Vector2(84, 20), _sourceY = 64}, 
                new Citizen() { Pos = new Vector2(100, 20), _sourceY = 80}, 
                new Citizen() { Pos = new Vector2(116, 20), _sourceY = 96}*/
            };
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
            if (key == Keys.Up)
            {
                thug.Pos = new Vector2(thug.Pos.X, thug.Pos.Y - 8);
            }
            if (key == Keys.Down)
            {
                thug.Pos = new Vector2(thug.Pos.X, thug.Pos.Y + 8);
            }
            if (key == Keys.Left)
            {
                thug.Pos = new Vector2(thug.Pos.X - 8, thug.Pos.Y);
            }
            if (key == Keys.Right)
            {
                thug.Pos = new Vector2(thug.Pos.X + 8, thug.Pos.Y);
            }
        }
    }
}
