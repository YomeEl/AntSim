using AntSim.Simulation.Map;

using SFML.System;

using System;

namespace AntSim.Simulation
{
    class SmellSystem
    {
        private readonly Field<Cell> map;

        public SmellSystem(Field<Cell> map)
        {
            this.map = map;
        }

        public void SpreadSmell(uint code, uint strength, Vector2i from)
        {
            int radius = (int)strength;
            for (int i = from.X - radius; i < from.X + radius; i++)
            {
                for (int j = from.Y - radius; j < from.Y + radius; j++)
                {
                    var dist = Math.Sqrt(Math.Pow(i - from.X, 2) + Math.Pow(j - from.Y, 2));
                    if (dist <= radius)
                    {
                        if (map[i, j].Smells.ContainsKey(code) &&
                            map[i, j].Smells[code] < strength - (uint)dist ||
                            !map[i, j].Smells.ContainsKey(code))
                        {
                            map[i, j].Smells[code] = strength - (uint)dist;
                        }
                    }
                }
            }
        }
    }
}
