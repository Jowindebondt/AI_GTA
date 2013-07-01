using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GTA
{
    public abstract class FuzzySet : FuzzyTerm
    {
        private double dom;
        public double Dom
        {
            get { return dom; }
            set
            {
                if (value <= 1 && value >= 0) dom = value;
                else Console.WriteLine("<FuzzySet::SetDOM>: invalid value");
            }

        }

        public double RepresentativeValue { get; private set; }

        public FuzzySet(double RepVal)
        {
            Dom = 0.0d;
            RepresentativeValue = RepVal;
        }

        public abstract double CalculateDom(double val);

        public override void OrWithDom(double val)
        {
            if (val > Dom) Dom = val; 
        }

        public override void ClearDom()
        {
            Dom = 0.0d;
        }
    }
}
