﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GTA
{
    class Pavement : ObstacleEntity
    {
        private Texture2D myTexture;
        public Pavement()
        {
            Blocking = Blocking.Car;
            Bradius = 64;
        }

        public override void Update(float timeElapsed)
        {
            /*throw new NotImplementedException();*/
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            int FrameWidth = myTexture.Width / 3;
            Rectangle sourcerect = new Rectangle((2*64), 1, FrameWidth, myTexture.Height);
            spriteBatch.Draw(myTexture, Pos.toVector2(), sourcerect, Color.White,
                       0, new Vector2(0, 0), 1, SpriteEffects.None, 1);
        }

        public override void Load(GraphicsDevice graphicsDevice, ContentManager content)
        {
            myTexture = content.Load<Texture2D>("map");
        }
    }
}
