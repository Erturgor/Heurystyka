﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Heurystyka
{
    public class ArtificialBeeColony : IOptimizationAlogirthm
    {
        // Zalozenia odgorne
        public string Name { get; set; } = "Artificla Bee Colony Algorithm";
        public double[] XBest { get; set; }
        public double FBest { get; set; }
        public int NumberOfEvaluationFitnessFunction { get; set; }

        // Wymagania do testowania kazdej klasy tak czy siak 
        public int size { get; set; }
        public int iteration { get; set; }
        public int dimensions { get; set; }
        public double min { get; set; }
        public double max { get; set; }
        public Func<double[], double> fun { get; set; }
        //Parametry ABC
        int currentIteration;
        List<double[]> bees;
        double[] fitnesses;
        int[] trial;

        public void fit(Func<double[], double> function, int N = 10, int i = 5, int d = 3, double Max = 5.0, double Min = -5.0)
        {
            size = N;
            iteration = i;
            dimensions = d;
            min = Min;
            max = Max;
            fun = function;
            trial = new int[size];
            for (int j = 0; j < size; j++)
            {
                trial[j] = 0;
            }
        }
        public double Solve()
        {
            bool readed = readFile();
            if (!readed) { generateBees(); currentIteration = 1; }
            while (currentIteration <= iteration)
            {
                SendEmployedBees();
                SendOnlookerBees();
                SendScoutBees();
                for (int j = 1; j < size; j++)
                {
                    if (fitnesses[j] > FBest)
                    {
                        FBest = fitnesses[j];
                        XBest = bees[j];
                    }
                }
                currentIteration++;
            }

            return FBest;

        }


        private bool readFile()
        {
            string filePath = "ABC.json";

            if (File.Exists(filePath))
            {
                try
                {
                    string jsonContent = File.ReadAllText(filePath);
                    var state = JsonSerializer.Deserialize<ArtificialBeeColony>(jsonContent);

                    if (state == null)
                    {
                        return false;
                    }

                    XBest = state.XBest;
                    FBest = state.FBest;
                    NumberOfEvaluationFitnessFunction = state.NumberOfEvaluationFitnessFunction;

                    size = state.size;
                    iteration = state.iteration;
                    dimensions = state.dimensions;
                    min = state.min;
                    max = state.max;
                    currentIteration = state.currentIteration;

                    bees = state.bees;
                    fitnesses = state.fitnesses;
                    trial = state.trial;

                    fun = state.fun;

                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        private void generateBees()
        {
            bees = new List<double[]>();
            fitnesses = new double[size];
            Random rd = new Random();
            for (int i = 0; i < size; i++)
            {
                double[] bee = new double[dimensions];
                for (int j = 0; j < dimensions; j++)
                    bee[j] = min + rd.NextDouble() * (max - min);
                bees.Add(bee);
                fitnesses[i] = fitness(bees[i]);
                trial[i] = 0;
            }
            FBest = fitnesses[0];
            XBest = bees[0];
        }

        private double fitness(double[] value)
        {
            return 1 / (1 + fun(value));
        }

        private double[] generateNewSolution(int i)
        {
            Random random = new Random();
            int k;
            do
            {
                k = random.Next(size);
            } while (k == i);

            double[] solution = new double[dimensions];
            for (int j = 0; j < dimensions; j++)
            {
                double phi = random.NextDouble() * 2 - 1; // Random number in [-1, 1]
                solution[j] = bees[i][j] + phi * (bees[i][j] - bees[k][j]);
                solution[j] = Math.Max(min, Math.Min(max, solution[j]));
            }

            return solution;
        }
        private void SendEmployedBees()
        {
            for (int i = 0; i < size; i++)
            {
                createAndCheckFitness(i);
            }
        }

        private void SendOnlookerBees()
        {
            Random rd = new Random();
            double totalFitness = 0.0;
            double[] cumulativeProbabilities = new double[size];

            for (int i = 0; i < size; i++)
                totalFitness += fitnesses[i];

            double cumulativeSum = 0.0;
            for (int i = 0; i < size; i++)
            {
                cumulativeSum += fitnesses[i] / totalFitness;
                cumulativeProbabilities[i] = cumulativeSum;
            }
            for (int i = 0; i < size; i++)
            {
                double randomValue = rd.NextDouble(); 

                for (int j = 0; j < size; j++)
                {
                    if (randomValue < cumulativeProbabilities[j])
                    {
                        createAndCheckFitness(j);
                        break;
                    }
                }
            }
        }

        private void createAndCheckFitness(int i)
        {
            double[] newSolution = generateNewSolution(i);
            double newFitness = fitness(newSolution);
            if (newFitness > fitnesses[i])
            {
                bees[i] = newSolution;
                fitnesses[i] = newFitness;
                trial[i] = 0;
            }
            else
            {
                trial[i]++;
            }
            NumberOfEvaluationFitnessFunction++;
        }

        private void SendScoutBees()
        {
            Random random = new Random();
            for (int i = 0; i < size; i++)
            {
                if (trial[i] > size * dimensions)
                {
                    for (int j = 0; j < dimensions; j++)
                        bees[i][j] = min + random.NextDouble() * (max - min);

                    fitnesses[i] = fitness(bees[i]);
                    trial[i] = 0;
                }
            }
        }
    }
}
