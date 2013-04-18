using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GTA
{
    abstract class BaseGameEntity
    {
        public int Id { get; set; }
        public Vector2 Pos { get; set; }
        public float Scale { get; set; }
        public float Bradius { get; set; }
        
        public abstract void Update(int time_elapsed);
        public abstract void Render();
    }
}
