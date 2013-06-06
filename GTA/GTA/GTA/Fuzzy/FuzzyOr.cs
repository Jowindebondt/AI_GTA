using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTA
{
    public class FuzzyOr : FuzzyTerm
    {
        private List<FuzzyTerm> terms;

        public FuzzyOr(FuzzyOr fa) 
        {
            this.terms = fa.terms;
        }

        public FuzzyOr(FuzzyTerm op1, FuzzyTerm op2) 
        {
            terms.Add(op1);
            terms.Add(op2);
        }

        public FuzzyOr(FuzzyTerm op1, FuzzyTerm op2, FuzzyTerm op3) 
        {
            terms.Add(op1);
            terms.Add(op2);
            terms.Add(op3);
        }

        public FuzzyOr(FuzzyTerm op1, FuzzyTerm op2, FuzzyTerm op3, FuzzyTerm op4) 
        {
            terms.Add(op1);
            terms.Add(op2);
            terms.Add(op3);
            terms.Add(op4);
        }

        public override double GetDom() 
        {
            double largest = float.MinValue;

            foreach(FuzzyTerm term in terms)
                if (term.GetDom() > largest)
                    largest = term.GetDom();

            return largest;
        }

        public override void OrWithDom(double val)
        {
            throw new NotImplementedException();
        }

        public override void ClearDom()
        {
            throw new NotImplementedException();
        }
    }
}
