using ClosedXML.Excel;
using Heurystyka;

class Program
{
    static void Main()
    {
        var numberOfRuns = 20;
        var numberOfParameters = new[] { 2,5,10,30,50}; // Liczba szukanych parametrów
        var iterations = new[] { 5, 10, 20, 40, 60, 80 }; // Liczba iteracji
        var populationSizes = new[] { 10, 20, 40, 80 }; // Rozmiar populacji
        var fun = Funkcje.Sphere;

        // Tworzenie nowego dokumentu Excel
        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add("Results");

            // Nagłówki
            worksheet.Cell(1, 1).Value = "Algorytm";
            worksheet.Cell(1, 2).Value = "Funkcja Testowa";
            worksheet.Cell(1, 3).Value = "Liczb szukanych parametrow";
            worksheet.Cell(1, 4).Value = "Liczba Iteracji";
            worksheet.Cell(1, 5).Value = "Rozmiar Populacji";
            worksheet.Cell(1, 6).Value = "Znalezione Minimum";
            worksheet.Cell(1, 7).Value = "Odchylenie Standardowe poszukiwanych parametrow";
            worksheet.Cell(1, 8).Value = "Wartosc Funkcji Celu";
            worksheet.Cell(1, 9).Value = "Odchylenie Standardowe wartosci Funkcji Celu";

            int row = 2; // Zaczynamy od drugiego wiersza

            foreach (var dim in numberOfParameters)
            {
                foreach (var iter in iterations)
                {
                    foreach (var popSize in populationSizes)
                    {
                        var results = new List<double>();
                        var parametersList = new List<double[]>();

                        for (int run = 0; run < numberOfRuns; run++)
                        {
                            var eq = new ArtificialBeeColony();
                            eq.fit(fun, popSize, iter, dim, Max: 3, Min: -15);
                            eq.Solve();
                            results.Add(fun(eq.XBest));
                            parametersList.Add(eq.XBest);
                        }

                        // Obliczanie statystyk
                        double bestResult = results.Min();
                        int bestIndex = results.IndexOf(bestResult);
                        double[] bestParameters = parametersList[bestIndex];

                        double meanValue = results.Average();
                        double stdDevValue = Math.Sqrt(results.Select(x => Math.Pow(x - meanValue, 2)).Sum() / results.Count);

                        var meanParamsArray = new double[bestParameters.Length];
                        var stdDevParamsArray = new double[bestParameters.Length];

                        for (int i = 0; i < bestParameters.Length; i++)
                        {
                            var paramValuesForAxis = parametersList.Select(p => p[i]).ToList();
                            double meanForAxis = paramValuesForAxis.Average();
                            double stdDevForAxis = Math.Sqrt(paramValuesForAxis.Select(x => Math.Pow(x - meanForAxis, 2)).Sum() / paramValuesForAxis.Count);

                            meanParamsArray[i] = meanForAxis;
                            stdDevParamsArray[i] = stdDevForAxis;
                        }

                        // Zapisz dane do arkusza
                        worksheet.Cell(row, 1).Value = "ABC";
                        worksheet.Cell(row, 2).Value = "Sphere";
                        worksheet.Cell(row, 3).Value = dim;
                        worksheet.Cell(row, 4).Value = iter;
                        worksheet.Cell(row, 5).Value = popSize;
                        worksheet.Cell(row, 6).Value = string.Join(",", bestParameters.Select(val => val.ToString()));
                        worksheet.Cell(row, 7).Value = string.Join(",", stdDevParamsArray.Select(val => val.ToString()));
                        worksheet.Cell(row, 8).Value = bestResult;
                        worksheet.Cell(row, 9).Value = stdDevValue;

                        row++; // Przechodzimy do następnego wiersza
                    }
                }
            }

            // Zapisz plik do lokalizacji
            workbook.SaveAs("Sphere ABC.xlsx");
        }

        Console.WriteLine("Wyniki zapisano do pliku Results.xlsx");
    }
}