﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTA
{
    class Thug : Person
    {
        private World world;

        public Thug()
        {
            world = World.GetInstance();
        }

        public override void Update(int time_elapsed)
        {
            throw new NotImplementedException();
        }

        public override void Render()
        {
            throw new NotImplementedException();
        }
    }
}
