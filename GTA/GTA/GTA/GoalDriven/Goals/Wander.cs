﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTA
{
    class Wander : AtomicGoal
    {
        public Wander(MovingEntity owner)
        {
            Owner = owner;
        }

        public override void Activate()
        {
            StatusOfGoal = Status.Active;
            Owner.Wander = true;
        }

        public override Status Process()
        {
            if(StatusOfGoal == Status.Inactive)
                this.Activate();

            return StatusOfGoal;
        }

        public override void Terminate()
        {
            Owner.Wander = false;
        }

        public override string ToString()
        {
            return "Wander";
        }
    }
}
