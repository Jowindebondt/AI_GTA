using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTA
{
    public class FuzzyVariable
    {
        private Dictionary<string, FuzzySet> memberSets;
        private double minRange, maxRange;

        public FuzzyVariable()
        {
            minRange = 0.0d;
            maxRange = 0.0d;
            memberSets = new Dictionary<string, FuzzySet>();
        }

        public FuzzySet AddLeftShoulderSet(string name, double minBound, double peek, double maxBound)
        {
            memberSets[name] = new FuzzySetLeftShoulder(peek, peek - minBound, maxBound - peek);
            AdjustRangeToFit(minBound, maxBound);
            return memberSets[name];
        }

        public FuzzySet AddRightShoulderSet(string name, double minBound, double peek, double maxBound)
        {
            memberSets[name] = new FuzzySetRightShoulder(peek, peek - minBound, maxBound - peek);
            AdjustRangeToFit(minBound, maxBound);
            return memberSets[name];
        }

        public FuzzySet AddTriangleSet(string name, double minBound, double peek, double maxBound)
        {
            memberSets[name] = new FuzzySetTriangle(peek, peek - minBound, maxBound - peek);
            AdjustRangeToFit(minBound, maxBound);

            return memberSets[name];
        }

        public void Fuzzify(double value)
        {
            if (!((value >= minRange) && (value <= maxRange))) 
                return;

            foreach (var curSet in memberSets)
                curSet.Value.Dom = (curSet.Value.CalculateDom(value));
        }

        public double DeFuzzifyMaxAv()
        {
            double bottom = 0.0;
            double top = 0.0;

            foreach (var curSet in memberSets)
            {
                bottom += curSet.Value.Dom; //DOM ISNT SET;
                top += curSet.Value.RepresentativeValue * curSet.Value.Dom;
            }

            if (bottom == 0.0d) 
                return 0.0;

            return top / bottom;   
        }

        public void AdjustRangeToFit(double min, double max)
        {
            if (min < minRange) 
                minRange = min;
            if (max > maxRange) 
                maxRange = max;
        }
    }
}
