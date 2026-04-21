using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using EngineApp.ParallelSim;
using EngineApp.SequentialSim;

namespace EngineApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new GridConfig();

            Console.WriteLine("Generando datos de animación...");
            
            var simSeq = new SequentialSimulator(config);
            simSeq.Run("Sequential");
            
            var simPar = new ParallelSimulator(config);
            simPar.Run(8, "Parallel");

            Console.WriteLine("\nIniciando Benchmark Strong Scaling...");
            
            var csvContent = new StringBuilder();
            csvContent.AppendLine("Cores,TimeMs,SpeedUp");
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
                
                string resultLine = $"{cores},{timeMs},{speedUp:F2}";
                Console.WriteLine(resultLine);
                csvContent.AppendLine(resultLine);
            }

            string csvPath = "/Users/mori/RiderProjects/MonteCarloSIR/Data/scaling.csv";
            File.WriteAllText(csvPath, csvContent.ToString());
            
            Console.WriteLine($"\n El archivo se guardado en: {csvPath}");
        }
    }
}