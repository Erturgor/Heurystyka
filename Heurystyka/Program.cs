// See https://aka.ms/new-console-template for more information
using Heurystyka;
using System.Numerics;
using System.Runtime.Intrinsics;

Equilibrium equilibrium = new Equilibrium();
equilibrium.fit(Funkcje.Rastrigin,N:50, i:1000,d:2, Max:4.5, Min:-4.5);
var wartosc = equilibrium.Solve();
Console.WriteLine(wartosc.ToString());
Console.WriteLine();
Console.WriteLine("[{0}]", String.Join(',',equilibrium.XBest));
ArtificialBeeColony artificialBee = new ArtificialBeeColony();
artificialBee.fit(Funkcje.Rastrigin, N: 50, i: 1000, d:2 , Max: 4.5, Min: -4.5);
var wartosc2 = artificialBee.Solve();
Console.WriteLine(wartosc2.ToString());
Console.WriteLine();
Console.WriteLine("[{0}]", String.Join(',', artificialBee.XBest));
