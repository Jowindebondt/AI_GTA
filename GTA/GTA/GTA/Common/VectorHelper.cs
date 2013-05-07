using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTA.Common;
using Microsoft.Xna.Framework;

namespace GTA
{
    static class VectorHelper
    {
        private static Random rand = new Random();

        public static Vector2D MaxLimit(Vector2D vector, float limit)
        {
            if (vector.Length() > limit)
            {
                var factor = vector.Length() / limit;
                vector = vector / factor;
            }
            return vector;
        }

        public static Vector2D ToLimit(Vector2D vector, float limit)
        {
            var factor = vector.Length() / limit;
            vector = vector / factor;
            return vector;
        }

        public static Vector2D GetPerpVector(Vector2D vec)
        {
            return new Vector2D(-vec.Y, vec.X);
        }

        public static double RandFloat()
        {
            return rand.NextDouble();
        }

        public static double RandomClamped()
        {
            return RandFloat() - RandFloat();
        }
    }
}
