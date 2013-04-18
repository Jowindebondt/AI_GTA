using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GTA
{
    abstract class BaseGameEntity
    {
        public int Id { get; set; }
        public Vector2 Pos { get; set; }
        public float Scale { get; set; }
        public float Bradius { get; set; }

        public abstract void Update(TimeSpan timeElapsed);
        public abstract void Render(SpriteBatch spriteBatch);
        public abstract void Load(GraphicsDevice graphicsDevice, ContentManager content);
    }
}
