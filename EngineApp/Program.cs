using System;
using System.Diagnostics;

namespace EngineApp;

class Program
{
    static void Main(string[] args)
    {
        var config = new GridConfig();

        Console.WriteLine("Generando datos...");
        
        var simSeq = new ParallelSimulator(config);
        simSeq.Run(1, "Sequential");

        var simPar = new ParallelSimulator(config);
        simPar.Run(8, "Parallel");

        Console.WriteLine("\nIniciando Benchmark Strong Scaling...");
        Console.WriteLine("Cores,TimeMs,SpeedUp");
        
        int[] coresToTest = { 1, 2, 4, 8 };
        long baseTime = 0;

        foreach (var cores in coresToTest)
        {
            var simulator = new ParallelSimulator(config);
            var stopwatch = Stopwatch.StartNew();
            
            simulator.Run(cores, null);
            
            stopwatch.Stop();
            long timeMs = stopwatch.ElapsedMilliseconds;

            if (cores == 1) baseTime = timeMs;

            double speedUp = (double)baseTime / timeMs;
            Console.WriteLine($"{cores},{timeMs},{speedUp:F2}");
        }
    }
}
