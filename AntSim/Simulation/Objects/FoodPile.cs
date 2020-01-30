using AntSim.Graphics;

using SFML.Graphics;
using SFML.System;

namespace AntSim.Simulation.Objects
{
    class FoodPile : GraphicalObject
    {
        public uint Count { get; set; }
        public Vector2i Position { get; set; }
        public bool IsSmellSpreaded { get; set; } = false;

        public FoodPile(uint count, Texture texture, byte width, byte height) : base(texture, width, height)
        {
            Count = count;
        }
    }
}
