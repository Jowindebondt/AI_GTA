﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GTA
{
    class Pavement : ObstacleEntity
    {
        public Pavement()
        {
            Blocking = Blocking.Car;
        }

        public override void Update(float timeElapsed)
        {
            throw new NotImplementedException();
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }

        public override void Load(GraphicsDevice graphicsDevice, ContentManager content)
        {
            throw new NotImplementedException();
        }
    }
}