using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GTA
{
    class Building : ObstacleEntity
    {
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
            /*throw new NotImplementedException();*/
        }

        public override void Load(GraphicsDevice graphicsDevice, ContentManager content)
        {
            /*throw new NotImplementedException();*/
        }
    }
}
