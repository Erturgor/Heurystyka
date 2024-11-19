using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heurystyka
{
    public class Feudal : IOptimizationAlogirthm
    {
        public string Name { get; set; } = "Feudal Optimizer Algorithm";
        public double[] XBest { get; set; } 
        public double FBest { get; set; } 
        public int NumberOfEvaluationFitnessFunction { get; set; } = 0;

        // Wymagania do testowania kazdej klasy tak czy siak 
        public int size { get; set; }
        public int iteration { get; set; }
        public int dimensions { get; set; }
        public double min { get; set; }
        public double max { get; set; }
        public Func<double[], double> fun { get; set; }

        //Parametry Feudal
        List<double[]> counts;
        List<double[]> vasals;
        double[] fitnesses;
        int[] ageOfVasals;
        double legitimacy;
        public int NumberofCounts { get; set; }
        public void fit(Func<double[], double> function, int N = 10, int i = 5, int d = 3, double Max = 5.0, double Min = -5.0, int Counts = 5, double Legitimacy = 0.4)
        {
            size = N;
            iteration = i;
            dimensions = d;
            min = Min;
            max = Max;
            fun = function;
            NumberofCounts = Counts;
            legitimacy = Legitimacy;
        }

        public double Solve()
        {
            generateVasals();
            for (int i = 1; i <= iteration; i++)
            {
                hierarchyChange();
                for (int j = 0; j < size; j++)
                {
                    createCoteries(j);
                }
                newGeneration();
                
            }
            counts.Sort((x1, x2) => fitness(x1).CompareTo(fitness(x2))); //czysto teoretycznie average moze byc lepszy
            XBest = counts[0];
            FBest = fitness(counts[0]);
            return FBest;
        }
        private void generateVasals()
        {
            counts = new List<double[]>();
            vasals = new List<double[]>();
            Random rd = new Random();
            fitnesses = new double[size];
            ageOfVasals = new int[size];
            for (int i = 0; i < size; i++)
            {
                double[] vasal = new double[dimensions];
                for (int j = 0; j < dimensions; j++)
                {
                    vasal[j] = min + rd.NextDouble() * (max - min);
                }
                vasals.Add(vasal);
                fitnesses[i] = fitness(vasals[i]);
                ageOfVasals[i] = 0;
            }
            for (int i = 0; i < NumberofCounts; i++)
            {
                double[] count = new double[dimensions];
                for (int j = 0; j < dimensions; j++)
                {
                    count[j] = min + rd.NextDouble() * (max - min);
                }
                counts.Add(count);
            }
        }

        private double fitness(double[] value)
        {
            return 1 / (1 + fun(value));
        }

        private void createCoteries(int i)
        {
            double[] newSolution = modifyByPatron(i);
            double newFitness = fitness(newSolution);
            if (newFitness > fitnesses[i])
            {
                vasals[i] = newSolution;
                fitnesses[i] = newFitness;
                ageOfVasals[i] = 0;
            }
            else
            {
                ageOfVasals[i]++;
            }
            NumberOfEvaluationFitnessFunction++;
        }

        private double[] modifyByPatron(int i)
        {
            Random random = new Random();
            var patron = selectNewPatron();
            double[] solution = new double[dimensions];
            for (int j = 0; j < dimensions; j++)
            {
                double phi = random.NextDouble(); 
                solution[j] = vasals[i][j] + phi * (vasals[i][j] - patron[j]);
                solution[j] = Math.Max(min, Math.Min(max, solution[j]));
            }
            return solution;
        }

        private double[] selectNewPatron()
        {
            Random random = new Random();
            double[] patron = new double[dimensions];
            if (random.NextDouble() > legitimacy)
            {
                double totalFitness = 0.0;
                double[] prob = new double[size];

                for (int i = 0; i < size; i++)
                    totalFitness += fitnesses[i];

                for (int i = 0; i < size; i++)
                    prob[i] = fitnesses[i] / totalFitness; //zmienic obliczanie prawdopodobienstwa?

                for (int i = 0; i < size; i++)
                {
                    if (random.NextDouble() < prob[i])
                    {
                        return vasals[i];
                    }
                }
            }
            return counts[random.Next(counts.Count)];
        }

        private void hierarchyChange()
        {

            for (int i = 0; i < dimensions; i++)
            {
                for (int j = 0; j < NumberofCounts; j++)
                {
                    if (fitnesses[i] > fitness(counts[j]))
                    {
                        var temp = counts[j];
                        counts[j] = vasals[i];
                        vasals[i] = temp;
                        fitnesses[i] = fitness(vasals[i]);
                        NumberOfEvaluationFitnessFunction++;
                    }
                }
            }

        }
        private void newGeneration()
        {
            Random random = new Random();
            for (int i = 0; i < size; i++)
            {
                if (ageOfVasals[i] > size * dimensions)
                {
                    for (int j = 0; j < dimensions; j++)
                        vasals[i][j] = min + random.NextDouble() * (max - min);

                    fitnesses[i] = fitness(vasals[i]);
                    ageOfVasals[i] = 0;
                }
            }
        }

    }
}
