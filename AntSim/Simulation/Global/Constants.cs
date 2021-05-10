using System.Collections.Generic;
using System.IO;

namespace AntSim.Simulation.Global
{
    static class NumberConstants
    {
        private const string FILENAME = "Settings.txt";
        private static Dictionary<string, float> collection;

        public static void Load()
        {
            collection = new Dictionary<string, float>();
            SetDefault();

            string[] lines;
            try
            {
                lines = File.ReadAllLines(FILENAME);
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e.Data);
            }
            finally
            {
                lines = new string[0];
            }

            if (lines.Length == 0)
            {
                WriteCurrent();
                return;
            }
            
            foreach (string line in lines)
            {
                var lineElem = line.Split('=');
                collection.Add(lineElem[0], float.Parse(lineElem[1]));
            }
        }

        public static float Get(string name) => collection[name];

        private static void SetDefault()
        {
            collection["WindowWidth"] = 800;
            collection["WindowHeight"] = 600;

            collection["DeltaTime"] = 0.1f;

            collection["DefaultWorkerSpeed"] = 1;
            collection["WorkerSearchingRadius"] = 25;
            collection["WorkerNewWPMinDist"] = 5;
            collection["WorkerNewWPMaxDist"] = 10;

            collection["ChunkSize"] = 100;

            collection["FoodCount"] = 5;
            collection["FoodPileRadius"] = 25;
        }

        private static void WriteCurrent()
        {
            File.WriteAllLines(FILENAME, GetLines());
        }

        private static IEnumerable<string> GetLines()
        {
            foreach (KeyValuePair<string, float> pair in collection)
            {
                yield return pair.Key + '=' + pair.Value.ToString();
            }
        }
    }
}
