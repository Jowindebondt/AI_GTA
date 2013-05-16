using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GTA
{
    abstract class ObstacleEntity : BaseGameEntity
    {
        public Blocking Blocking { get; set; }

        public abstract override void Update(float timeElapsed);
        public abstract override void Render(SpriteBatch spriteBatch);
        public abstract override void Load(GraphicsDevice graphicsDevice, ContentManager content);
    }
}
