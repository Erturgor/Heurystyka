﻿// See https://aka.ms/new-console-template for more information
using Heurystyka;
using System.Numerics;
using System.Runtime.Intrinsics;
//var fun = Funkcje.Sphere;
var fun = Funkcje.Sphere;
//var fun = Funkcje.Rosenbrock;
Equilibrium equilibrium = new Equilibrium();
equilibrium.fit(fun,N:50, i:10,d:5, Max:4.5, Min:-4.5);
var wartosc = equilibrium.Solve();
Console.WriteLine(wartosc.ToString());
Console.WriteLine("[{0}]", String.Join(" | ",equilibrium.XBest));
Console.WriteLine();

for (int i = 10; i < 100; i+=10)
{
    ArtificialBeeColony artificialBee = new ArtificialBeeColony();
    artificialBee.fit(fun, N: 40, i: i, d: 50, Max: 5, Min: -5);
    var wartosc2 = artificialBee.Solve();
    Console.WriteLine(wartosc2.ToString());
    Console.WriteLine(fun(artificialBee.XBest));
    Console.WriteLine("[{0}]", String.Join(" | ", artificialBee.XBest));
    Console.WriteLine();
}


