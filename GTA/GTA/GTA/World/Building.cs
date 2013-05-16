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
        public Building()
        {
            Blocking = Blocking.All;
            Bradius = 64;
        }

        public override void Update(float timeElapsed)
        {
            /*throw new NotImplementedException();*/
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            int FrameWidth = myTexture.Width / 3;

            Rectangle sourcerect = new Rectangle(FrameWidth * 64, 1, FrameWidth, myTexture.Height);
            spriteBatch.Draw(myTexture, Pos.toVector2(), sourcerect, Color.White,
                       1, new Vector2(0,0), 1, SpriteEffects.None, 1);
        }

        public override void Load(GraphicsDevice graphicsDevice, ContentManager content)
        {
            myTexture = content.Load<Texture2D>("map");
        }
    }
}
