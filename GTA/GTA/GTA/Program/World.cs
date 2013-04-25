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
            thug = new Thug() {Pos = new Vector2(20, 20), _sourceY = 0};
            _entities = new List<BaseGameEntity> { 
                new Citizen() { Pos = new Vector2(36, 20), _sourceY = 16, enemy = thug} 
                /*new Citizen() { Pos = new Vector2(52, 20), _sourceY = 32}, 
                new Citizen() { Pos = new Vector2(68, 20), _sourceY = 48},
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
            switch (key)
            {
                case Keys.Up:
                    thug.Pos = new Vector2(thug.Pos.X, thug.Pos.Y - 5);
                    break;

                case Keys.Down:
                    thug.Pos = new Vector2(thug.Pos.X, thug.Pos.Y + 5);
                    break;

                case Keys.Left:
                    thug.Pos = new Vector2(thug.Pos.X - 5, thug.Pos.Y);
                    break;

                case Keys.Right:
                    thug.Pos = new Vector2(thug.Pos.X + 5, thug.Pos.Y);
                    break;
            }
        }
    }
}
