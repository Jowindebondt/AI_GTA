using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTA
{
    public class FuzzySetTriangle : FuzzySet
    {
        private double PeekPoint;
        private double LeftOffset;
        private double RightOffset;

        public FuzzySetTriangle(double mid, double left, double right) : base(mid)
        {
            PeekPoint = mid;
            LeftOffset = left;
            RightOffset = right;
        }

        public override double CalculateDom(double value)
        {
            if ((RightOffset == 0.0d && PeekPoint == value) || (LeftOffset == 0.0d && PeekPoint == value))
                return 1.0d;
            if (value <= PeekPoint && value >= (PeekPoint - LeftOffset))
                return (1.0d/LeftOffset) * (value - (PeekPoint - LeftOffset));
            if (value > PeekPoint && value < (PeekPoint + RightOffset))
                return (1.0 / -RightOffset) * (value - PeekPoint) + 1;
            return 0.0d;
        }
    }
}
