using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heurystyka
{
    public interface IOptimizationAlogirthm
    {
        string Name { get; set; }
        //rozwiazanie
        //czy jest plik wczytanie pliku
        //jesli nie od poczatku
        //zwraca wartosc funkcji celu dla znalezionego rozwiazania
        double Solve();
        double[] XBest {  get; set; }
        double FBest { get; set; }
        //zwraca liczbę wywołan funkcji dopasowania
        int NumberOfEvaluationFitnessFunction { get; set; }

    }
}
