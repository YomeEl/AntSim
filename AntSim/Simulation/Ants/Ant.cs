﻿using AntSim.Graphics;
using AntSim.Simulation.Map;
using AntSim.Simulation.Map.Smells;
using AntSim.Simulation.Items;

using System;

using SFML.System;

namespace AntSim.Simulation.Ants
{
    abstract class Ant : GraphicalObject
    {
        protected Random randomizer;

        public uint AntId { get; }
        public uint FactionId { get; }
        public IItem Item { get; protected set; }

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
        protected (int x, int y, bool found) FindFarSmell(Field<Cell> field, int radius, SmellType type)
        {
            float dist = -1;
            (int x, int y) pos = (0, 0);
            int str = -1;

            Predicate<SmellInfo> pred;
            pred = (SmellInfo s) => s.Type == type;

            for (int i = -radius; i < radius; i++)
            {
                for (int j = -radius; j < radius; j++)
                {
                    var smell = field[i, j].Smells.Find(pred);
                    var curDist = Math.Abs(Position.X - i) + Math.Abs(Position.Y - j);
                    if (smell != null && (dist < curDist || smell.Strength > str))
                    {
                        pos = (i, j);
                        dist = curDist;
                        str = smell.Strength;
                    }
                }
            }

            return (pos.x, pos.y, dist >= 0);
        }

        protected Vector2f Normalize(Vector2f vect)
        {
            var len = (float)Math.Sqrt(vect.X * vect.X + vect.Y * vect.Y);
            vect.X /= len;
            vect.Y /= len;
            return vect;
        }
    }
}