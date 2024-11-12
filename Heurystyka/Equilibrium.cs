using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heurystyka
{
    public class Equilibrium : IOptimizationAlogirthm
    {
        // Zalozenia odgorne
        public string Name { get; set; } = "Equilibrum Optimizer Algorithm";
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

        private bool checkRange(double x) // Dla Bukina to nie zadziala ale ****
        {
            return (x >= min) && x <= max;
        }
        //Parametry Equilibrum
        double a1 { get; set; }
        double a2 { get; set; }
        double GP { get; set; }
        List<double[]> equilibrumPool;
        List<double[]> particles;
        List<double[]> oldParticles;//Czastki pamietaja swoje jedno polozenie wczesniej
        public void fit(Func<double[], double> function, int N = 10, int i = 5, int d = 3, double Max = 5.0, double Min = -5.0, double aa1=2, double aa2=1, double gp = 0.5)
        {
            size = N;
            iteration = i;
            dimensions = d;
            min = Min;
            max = Max;
            fun = function;
            a1 = aa1;
            a2 = aa2;
            GP = gp;
        }

        //Obliczenia
        public double Solve()
        {
            bool readed = readFile();
            if (!readed) generateParticles();
            for (int i = 1; i <= iteration; i++)
            {
                checkFitness();
                if (i > 1) memorySave();
                double t = Math.Pow((1 - i / iteration), (a2 * i / iteration));
                for (int j = 0; j < size; j++)
                {
                    Random rd = new Random();
                    var Ceq = equilibrumPool[rd.Next(equilibrumPool.Count)];
                    var lamda = randomTable();
                    var r = randomTable();
                    var F = new double[dimensions];
                    for (int k = 0; k < dimensions; k++)
                    {
                        F[k] = a1 * Math.Sign(r[k] - 0.5) * (Math.Pow(Math.E, lamda[k]*t)-1);
                    }
                    var generationControlParameter = generateGCP();
                    var G0 = new double[dimensions];
                    var G = new double[dimensions];
                    for(int k = 0;k < dimensions; k++)
                    {
                        G0[k] = generationControlParameter[k] * (Ceq[k] - lamda[k] * particles[j][k]);
                        G[k] = G0[k] * F[k];
                        var temp = Ceq[k] + (particles[j][k] - Ceq[k]) * F[k] + G[k] * (1 - F[k]);
                        if (checkRange(temp)) particles[j][k] = temp;

                    }
                }
            }
            equilibrumPool.Sort((x1, x2) => fun(x1).CompareTo(fun(x2)));
            XBest = equilibrumPool[0];
            FBest = fun(equilibrumPool[0]);
            return FBest;
        }


        private bool readFile()
        {
            return false; 
        }
        private void generateParticles()
        {
            equilibrumPool = new List<double[]>();
            particles = new List<double[]>();
            oldParticles = new List<double[]>();
            Random rd = new Random();
            for (int i = 0; i < size; i++)
            {
                double[] particle = new double[dimensions];
                for (int j = 0; j < dimensions; j++)
                {
                    particle[j]= min+rd.NextDouble()*(max-min);
                }
                particles.Add(particle);
            }
            for (int i = 0; i < 5; i++)
            {
                double[] equilibrum = new double[dimensions];
                for (int j = 0; j < dimensions; j++)
                {
                    equilibrum[j] = Math.Pow(10, 15);
                }
                equilibrumPool.Add(equilibrum);
            }
            oldParticles = copy();//kopiujemy poprzedni wynik na poczatku po prostu to samo
        }

        private List<double[]> copy()
        {
            List<double[]> temp = new List<double[]>();
            foreach (var array in particles) 
            {
                double[] copiedArray = new double[array.Length];
                Array.Copy(array, copiedArray, array.Length);
                temp.Add(copiedArray);
            }
            return temp;
        }

        private void checkFitness()
        {

            for (int i = 0;i < dimensions; i++)
            {
                if (fun(particles[i]) < fun(equilibrumPool[0])){
                    var temp = equilibrumPool[0];
                    equilibrumPool[0]=particles[i];
                    particles[i] = temp;
                    NumberOfEvaluationFitnessFunction++;
                }
                else if (fun(particles[i]) < fun(equilibrumPool[1]))
                {
                    var temp = equilibrumPool[1];
                    equilibrumPool[1] = particles[i];
                    particles[i] = temp;
                    NumberOfEvaluationFitnessFunction++;
                }
                else if (fun(particles[i]) < fun(equilibrumPool[2]))
                {
                    var temp = equilibrumPool[2];
                    equilibrumPool[2] = particles[i];
                    particles[i] = temp;
                    NumberOfEvaluationFitnessFunction++;
                }
                else if (fun(particles[i]) < fun(equilibrumPool[3]))
                {
                    var temp = equilibrumPool[3];
                    equilibrumPool[3] = particles[i];
                    particles[i] = temp;
                    NumberOfEvaluationFitnessFunction++;

                }
            }
            for (var i = 0; i < dimensions; i++) {
                var average = equilibrumPool[4];
                average[i] = (equilibrumPool[0][i] + equilibrumPool[1][i] + equilibrumPool[2][i] + equilibrumPool[3][i])/4;
            }

        }
        private double[] randomTable()
        {
            double[] table = new double[dimensions];
            Random rd = new Random();

            for (int i = 0; i < dimensions; i++)
            {
                table[i] = rd.NextDouble();
            }
            return table;
        }
        private void memorySave()
        {
            for (int i = 0; i < dimensions; i++)
            {
                if (fun(particles[i]) > fun(oldParticles[i]))
                {
                   particles[i] = oldParticles[i];
                }
            }
            oldParticles = copy();
        }
        private double[] generateGCP()
        {
            Random rd = new Random();
            double r1 = rd.NextDouble();
            double r2 = rd.NextDouble();
            double[] GCP = new double[dimensions];
            for (int i = 0; i < dimensions; i++)
            {
                if (r2 >= GP)
                {
                    GCP[i] = 0.5 * r1;
                }
                else GCP[i] = 0;
            }
            return GCP;
        }

    }
    
}
