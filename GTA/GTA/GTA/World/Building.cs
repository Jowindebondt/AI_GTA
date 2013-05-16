using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GTA
{
    class Building : ObstacleEntity
    {
        private Texture2D myTexture;
        public int FrameWidth { get; set; }
        public int FrameHeight { get; set; }
        public Building()
        {
            Blocking = Blocking.All;
            Bradius = 20;
        }

        public override void Update(float timeElapsed)
        {
            /*throw new NotImplementedException();*/
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            Rectangle sourcerect = new Rectangle(64, 1, FrameWidth, FrameHeight);
            spriteBatch.Draw(myTexture, Pos.toVector2(), sourcerect, Color.White,
                       0, new Vector2(0,0), 1, SpriteEffects.None, 1);
        }

        public override void Load(GraphicsDevice graphicsDevice, ContentManager content)
        {
            myTexture = content.Load<Texture2D>("map");
            FrameWidth = myTexture.Width / 3;
            FrameHeight = myTexture.Height;
        }
    }
}
