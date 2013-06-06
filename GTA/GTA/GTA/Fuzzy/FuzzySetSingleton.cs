using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTA
{
    public class FuzzySetSingleton : FuzzySet
    {
        private double midPoint, leftOffset, rightOffset;

        public FuzzySetSingleton(double midPoint, double leftOffset, double rightOffset)
            : base(midPoint)
        {
            this.midPoint = midPoint;
            this.leftOffset = leftOffset;
            this.rightOffset = rightOffset;
        }

        public override double CalculateDom(double val)
        {
            if ((val >= midPoint - leftOffset) && (val <= midPoint + rightOffset))
                return 1.0;
            return 0.0;
        }
    }
}
