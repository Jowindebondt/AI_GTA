using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTA
{
    public abstract class FuzzyTerm
    {
        public abstract double GetDom();
        public abstract void ClearDom();
        public abstract void OrWithDom(double value);
    }
}
