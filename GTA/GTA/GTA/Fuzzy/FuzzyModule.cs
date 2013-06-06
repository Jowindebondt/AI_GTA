using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GTA
{
    public class FuzzyModule
    {
        private Dictionary<string, FuzzyVariable> map;
        public const int NumSamplesToUseForCentroid = 15;
        private List<FuzzyRule> rules;

        public FuzzyModule()
        {
            map = new Dictionary<string, FuzzyVariable>();
            rules = new List<FuzzyRule>();
        }

        public FuzzyVariable CreateFLV(string name)
        {
            map[name] = new FuzzyVariable();
            return map[name];
        }

        public void AddRule(FuzzyTerm antecedent, FuzzyTerm consequence)
        {
            rules.Add(new FuzzyRule(antecedent, consequence));
        }

        public void Fuzzify(string NameOfFlv, double val)
        {
            map[NameOfFlv].Fuzzify(val);
        }

        public double DeFuzzify(string key)
        {
            if(map[key] == null)
                MessageBox.Show("Name is not in map");

            SetConfidencesOfConsequentsToZero();

            foreach (var fuzzyRule in rules)
                fuzzyRule.Calculate();

            return map[key].DeFuzzifyMaxAv();
        }

        private void SetConfidencesOfConsequentsToZero()
        {
            foreach (var fuzzyRule in rules)
                fuzzyRule.SetConfidenceOfConsequentToZero();
        }
    }
}
