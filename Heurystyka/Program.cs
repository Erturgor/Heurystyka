﻿// See https://aka.ms/new-console-template for more information
using Heurystyka;
using System.Numerics;
using System.Runtime.Intrinsics;
double[] x = { 3,0.5};
Equilibrium equilibrium = new Equilibrium();
equilibrium.fit(Funkcje.Rastrigin,N:50, i:100,d:5, Max:4.5, Min:-4.5);
var wartosc = equilibrium.Solve();
Console.WriteLine(wartosc.ToString());
Console.WriteLine(Funkcje.Beale(x));
Console.WriteLine();
Console.WriteLine("[{0}]", String.Join(" | ",equilibrium.XBest));
ArtificialBeeColony artificialBee = new ArtificialBeeColony();
artificialBee.fit(Funkcje.Rastrigin, N: 50, i: 100, d:5 , Max: 4.5, Min: -4.5);
var wartosc2 = artificialBee.Solve();
Console.WriteLine(wartosc2.ToString());
Console.WriteLine();
Console.WriteLine("[{0}]", String.Join(" | ", artificialBee.XBest));
