using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GTA
{
    public abstract class FuzzySet
    {
        public double Dom
        {
            get { return Dom; }
            set
            {
                if (value <= 1 && value >= 0) 
                    Dom = value;
                else
                    MessageBox.Show("Error!");
            }
        }
        public double RepresentativeValue { get; private set; }

        public FuzzySet(double RepVal)
        {
            Dom = 0.0d;
            RepresentativeValue = RepVal;
        }

        public abstract double CalculateDom(double val);

        public void OrwithDom(double val)
        {
            if (val > Dom) 
                Dom = val;
        }

        public void ClearDom()
        {
            Dom = 0.0d;
        }
    }
}
