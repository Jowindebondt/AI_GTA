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

        public override void Update(float timeElapsed)
        {

            if (Pos.X > 1600) { Pos = new Vector2(0, Pos.Y); }
            if (Pos.X < 0) { Pos = new Vector2(1600, Pos.Y); }
            if (Pos.Y < 0) { Pos = new Vector2(Pos.X, 900); }
            if (Pos.Y > 900) { Pos = new Vector2(Pos.X, 0); }

            _personTexture.UpdateFrame(timeElapsed);
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
