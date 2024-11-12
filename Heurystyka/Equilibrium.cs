using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heurystyka
{
    public class Equilibrium : IOptimizationAlogirthm
    {
        public string Name {get; set;}
        public double[] XBest { get; set; }
        public double FBest { get; set; }
        public int NumberOfEvaluationFitnessFunction { get; set; }

        public double Solve(Func<double[], double>,)
        {
            readfile();

            throw new NotImplementedException();
        }
    }
}
