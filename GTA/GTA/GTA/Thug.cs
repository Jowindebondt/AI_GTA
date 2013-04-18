using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GTA
{
    class Thug : Person
    {

        public Thug()
        {
        }

        public override void Update(TimeSpan timeElapsed)
        {
            _personTexture.UpdateFrame((float)timeElapsed.TotalSeconds);
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            _personTexture.DrawFrame(spriteBatch, Pos);
        }

        public override void Load(GraphicsDevice graphicsDevice, ContentManager content)
        {
            _personTexture.Load(graphicsDevice, content, "people2", Frames, FramesPerSec);
        }
    }
}
