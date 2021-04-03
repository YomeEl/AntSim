using AntSim.Graphics;

using SFML.Graphics;

namespace AntSim.Simulation.Objects
{
    class FoodPile : GraphicalObject
    {
        public uint Count { get; set; }
        public FoodPile(uint count, Texture texture, byte width, byte height) : base(texture, width, height)
        {
            Count = count;
        }
    }
}
