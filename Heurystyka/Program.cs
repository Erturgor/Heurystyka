// See https://aka.ms/new-console-template for more information
using Heurystyka;
using System.Numerics;
using System.Runtime.Intrinsics;

Equilibrium equilibrium = new Equilibrium();
equilibrium.fit(Funkcje.Sphere,N:30, i:100,d:5, Max:5.12, Min:-5.12);
var wartosc = equilibrium.Solve();
Console.WriteLine(wartosc.ToString());
Console.WriteLine();
Console.WriteLine("[{0}]", String.Join(',',equilibrium.XBest));
