using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTA
{
    abstract class Person : MovingEntity
    {

        public abstract override void Update(int time_elapsed);
        public abstract override void Render();

    }
}
