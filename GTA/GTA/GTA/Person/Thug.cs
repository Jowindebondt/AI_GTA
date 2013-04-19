using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GTA
{
    class Thug : Person
    {
        public Thug()
        {
            _personTexture = new AnimatedTexture(Vector2.Zero, 0, 1, 0);
        }

        public override void Update(TimeSpan timeElapsed)
        {
            _personTexture.UpdateFrame((float)timeElapsed.TotalSeconds);
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            _personTexture.DrawFrame(spriteBatch, Pos, _sourceY);
        }

        public override void Load(GraphicsDevice graphicsDevice, ContentManager content)
        {
            _personTexture.Load(graphicsDevice, content, "people2", Frames, FramesPerSec);
        }
    }
}
