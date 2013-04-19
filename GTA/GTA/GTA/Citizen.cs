using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GTA
{
    class Citizen : Person
    {
        public Citizen()
        {
            _personTexture = new AnimatedTexture(Vector2.Zero, 0, 1, 0);    
        }

        public override void Update(TimeSpan timeElapsed)
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
