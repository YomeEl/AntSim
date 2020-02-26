using AntSim.Simulation.Map;

using SFML.System;

using System;
using System.Collections.Generic;

namespace AntSim.Simulation
{
    class SmellSystem
    {
        public const uint FOOD_ID = 0;
        public const uint FACTION_ID_OFFSET = 0x10000000u;
        public const uint ANT_ID_OFFSET = 0x20000000u;

        private readonly Field<Cell> map;

        public Stack<Vector2i> NewFoodPiles { get; }

        public SmellSystem(Field<Cell> map)
        {
            this.map = map;
            NewFoodPiles = new Stack<Vector2i>();
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
                        if (map[i, j].Smells.ContainsKey(code))
                        {
                            map[i, j].Smells[code] += strength - (uint)dist;
                        }
                        else
                        {
                            map[i, j].Smells[code] = strength - (uint)dist;
                        }
                    }
                }
            }
        }

        public void ProcessNewFoodPiles()
        {
            while (NewFoodPiles.Count > 0)
            {
                SpreadSmell(FOOD_ID, 100, NewFoodPiles.Pop());
            }
        }
    }
}
