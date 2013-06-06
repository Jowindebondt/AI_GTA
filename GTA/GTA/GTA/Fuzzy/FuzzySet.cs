using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GTA
{
    public abstract class FuzzySet
    {
        public double Dom { get; set; }
        public double RepresentativeValue { get; private set; }

        public FuzzySet(double RepVal)
        {
            Dom = 0.0d;
            RepresentativeValue = RepVal;
        }

        public abstract double CalculateDom(double val);

        public void OrwithDom(double val)
        {
            if(val <= 1.0d && val >=0.0d)
                if (val > Dom) 
                    Dom = val;
        }

        public void ClearDom()
        {
            Dom = 0.0d;
        }
    }
}
