using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GTA
{
    abstract class BaseGameEntity
    {
        private int _id;
        private Vector2 _pos;
        private float _scale;
        private float _bradius;

        public abstract void Update(int time_elapsed);
        public abstract void Render();
        
        public int GetId()
        {
            return _id;
        }

        public Vector2 GetPos()
        {
            return _pos;
        }

        public float GetScale()
        {
            return _scale;
        }

        public float GetBradius()
        {
            return _bradius;
        }
    }
}
