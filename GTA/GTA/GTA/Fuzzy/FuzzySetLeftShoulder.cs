using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTA
{
    class FuzzySetLeftShoulder : FuzzySet
    {
        private double PeekPoint;
        private double LeftOffset;
        private double RightOffset;

        public FuzzySetLeftShoulder(double mid, double left, double right)
            : base(mid)
        {
            PeekPoint = mid;
            LeftOffset = left;
            RightOffset = right;
        }

        public override double CalculateDom(double value)
        {
            if (((RightOffset == 0.0) && (PeekPoint == value)) ||
               ((LeftOffset == 0.0) && (PeekPoint == value)))
                Dom = 1.0d;
            else if ((value >= PeekPoint) && (value < (PeekPoint + RightOffset)))
                Dom = (1.0d / -RightOffset) * (value - PeekPoint) + 1;
            else if ((value < PeekPoint) && (value >= PeekPoint - LeftOffset))
                Dom = 1.0d;
            else
                Dom = 0.0d;
            return Dom;
        }

        public override double GetDom()
        {
            return Dom;
        }

      
    }
}
