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

        public Ant(uint antId, uint factionId, SFML.Graphics.Texture texture, byte width, byte height) : base(texture, width, height)
        {
            AntId = antId;
            FactionId = factionId;
            randomizer = new Random((int)antId);
        }

        public abstract void Step(float dt, Field<Cell> field);

        /// <summary>
        /// Find coordinates of the most far smell from self, that is also the most strong
        /// </summary>
        /// <param name="field">Field for seraching</param>
        /// <param name="radius">Radius</param>
        /// <param name="type">Type of smell to search</param>
        /// <returns>Coordinates of desirable smell</returns>
        protected (Vector2i position, bool found) FindFarSmell(Field<Cell> field, int radius)
        {
            float dist = -1;
            var pos = new Vector2i(0, 0);
            int str = -1;

            var intPos = new Vector2i((int)Position.X, (int)Position.Y);
            var start = intPos - new Vector2i(radius, radius);
            var end = intPos + new Vector2i(radius, radius);
            for (int i = start.X; i <= end.X; i++)
            {
                for (int j = start.Y; j <= end.Y; j++)
                {
                    var smell = FindSmellWithPriority(field[i, j].Smells);
                    var curDist = Math.Abs(Position.X - i) + Math.Abs(Position.Y - j);
                    if (smell != null && (dist < curDist || smell.Strength > str))
                    {
                        pos.X = i;
                        pos.Y = j;
                        dist = curDist;
                        str = smell.Strength;
                    }
                }
            }

            return (pos, dist >= 0);
        }
        
        protected SmellInfo FindSmellWithPriority(System.Collections.Generic.List<SmellInfo> list)
        {
            SmellInfo smellInfo = null;
            int priority = -1;
            for (var i = 0; i < list.Count; i++)
            {
                if ((int)list[i].Type > priority)
                {
                    smellInfo = list[i];
                    priority = (int)smellInfo.Type;
                }
            }
            return smellInfo;
        }
    }
}