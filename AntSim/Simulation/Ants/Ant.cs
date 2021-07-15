using AntSim.Graphics;
using AntSim.Simulation.Map;
using AntSim.Simulation.Map.Smells;
using AntSim.Simulation.Items;

using System;

using SFML.System;

namespace AntSim.Simulation.Ants
{
    abstract class Ant : GraphicalObject
    {
        public uint AntId { get; }
        public uint FactionId { get; }
        public IItem Item { get; protected set; }

        protected float speed;
        protected Random randomizer;

        public Ant(uint antId, uint factionId, SFML.Graphics.Sprite sprite) : 
            base(sprite)
        {
            AntId = antId;
            FactionId = factionId;
            randomizer = new Random((int)antId);
        }

        public abstract void Step(float dt, Field<Cell> field);

        /// <summary>
        /// Finds coordinates of the closest and the farthest smells of specified types
        /// </summary>
        /// <param name="field">Field for seraching</param>
        /// <param name="radius">Searching radius</param>
        /// <param name="types">Array of types to search</param>
        /// <returns>Coordinates of the closest and the farthest smells of specified types.<para/>
        /// close[] far[], strong[] and weak[] lengths are equal to length of types[]<para/>
        /// close[n] is a coordinates of the closest smell of type types[n]<para/>
        /// far[n] is a coordinates of the furthest smell of type types[n]<para/>
        /// strong[n] is a coordinates of the strongest smell of type types[n]<para/>
        /// weak[n] is a coordinates of the weakest smell of type types[n]</returns>
        protected 
            (
            Vector2i?[] close, 
            Vector2i?[] far, 
            Vector2i?[] strong,
            Vector2i?[] weak
            ) FindSmells(Field<Cell> field, int radius, SmellType[] types)
        {
            Vector2i?[] close, far, strong, weak;
            int[] minDist, maxDist, minStr, maxStr;

            close = new Vector2i?[types.Length];
            far = new Vector2i?[types.Length];
            strong = new Vector2i?[types.Length];
            weak = new Vector2i?[types.Length];

            minDist = new int[types.Length];
            maxDist = new int[types.Length];
            minStr = new int[types.Length];
            maxStr = new int[types.Length];

            for (int i = 0; i < types.Length; i++)
            {
                close[i] = null;
                far[i] = null;
                strong[i] = null;
                weak[i] = null;

                minDist[i] = 2 * radius + 1;
                maxDist[i] = -1;
                minStr[i] = int.MaxValue;
                maxStr[i] = 0;
            }

            var intPos = new Vector2i((int)Position.X, (int)Position.Y);
            var start = intPos - new Vector2i(radius, radius);
            var end = intPos + new Vector2i(radius, radius);
            for (int i = start.X; i <= end.X; i++)
            {
                for (int j = start.Y; j <= end.Y; j++)
                {
                    for (int k = 0; k < types.Length; k++)
                    {
                        var smellStrength = field[i, j].GetSmell(types[k]);
                        if (smellStrength > 0)
                        {
                            var cur = new Vector2i(i, j);
                            int dist = Distance(intPos, cur);
                            if (minDist[k] > dist)
                            {
                                minDist[k] = dist;
                                close[k] = cur;
                            }
                            if (maxDist[k] < dist)
                            {
                                maxDist[k] = dist;
                                far[k] = cur;
                            }
                            if (minStr[k] > smellStrength)
                            {
                                minStr[k] = (int)smellStrength;
                                weak[k] = cur;
                            }
                            if (maxStr[k] < smellStrength)
                            {
                                maxStr[k] = (int)smellStrength;
                                strong[k] = cur;
                            }
                        }
                    }
                }
            }

            return (close, far, strong, weak);
        }
        
        protected void LeaveSmell(Field<Cell> field, SmellType type)
        {
            field[(int)Position.X, (int)Position.Y].SetSmell(type);
        }
    }
}