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
            if ((RightOffset == 0.0d && PeekPoint == value))
                return 1.0d;
            if (value <= PeekPoint && value > (PeekPoint - RightOffset))
                return (1.0d / RightOffset) * (value - (PeekPoint - RightOffset));
            if (value > PeekPoint)
                return 1.0d;
            return 0.0d;
        }
    }
}
