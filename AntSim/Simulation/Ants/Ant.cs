using AntSim.Graphics;
using AntSim.Simulation.Map;
using AntSim.Simulation.Items;

using System;

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
    }
}