using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GTA
{
    abstract class Person : MovingEntity
    {
        public AnimatedTexture _personTexture;
        public const int Frames = 3;
        public const int FramesPerSec = 5;
        public int _sourceY;

        public abstract override void Update(float timeElapsed);
        public abstract override void Render(SpriteBatch spriteBatch);
        public abstract override void Load(GraphicsDevice graphicsDevice, ContentManager content);
    }
}
