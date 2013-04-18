using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace GTA
{
    abstract class Person : MovingEntity
    {
        public abstract override void Update(TimeSpan timeElapsed);
        public abstract override void Render();

    }
}
