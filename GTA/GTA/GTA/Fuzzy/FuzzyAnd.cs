using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTA
{
    public class FuzzyAnd : FuzzyTerm
    {
        private List<FuzzyTerm> terms = new List<FuzzyTerm>();

        public FuzzyAnd(FuzzyAnd fa)
        {
            this.terms = fa.terms;
        }

        public FuzzyAnd(FuzzyTerm op1, FuzzyTerm op2) 
        {
            terms.Add(op1);
            terms.Add(op2);
        }
        
        public FuzzyAnd(FuzzyTerm op1, FuzzyTerm op2, FuzzyTerm op3) 
        {
            terms.Add(op1);
            terms.Add(op2);
            terms.Add(op3);
        }

        public FuzzyAnd(FuzzyTerm op1, FuzzyTerm op2, FuzzyTerm op3, FuzzyTerm op4) 
        {
            terms.Add(op1);
            terms.Add(op2);
            terms.Add(op3);
            terms.Add(op4);
        }

        public override double GetDom() 
        {
            double smallest = double.MaxValue;

            foreach(FuzzyTerm term in terms)
                if (term.GetDom() < smallest)
                    smallest = term.GetDom();
            return smallest;
        }

        public override void ClearDom() 
        {
            foreach (FuzzyTerm term in terms) 
                term.ClearDom();
        }

        public override void OrWithDom(double val) 
        {
            foreach (FuzzyTerm term in terms)
                term.OrWithDom(val);
        }
    }
}
