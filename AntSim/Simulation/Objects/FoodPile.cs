using AntSim.Graphics;
using AntSim.Simulation.Items;

using SFML.Graphics;

namespace AntSim.Simulation.Objects
{
    class FoodPile : GraphicalObject, IItem
    {
        public uint Count { get; set; }
        public FoodPile(uint count, Texture texture, byte width, byte height) : base(texture, width, height)
        {
            Count = count;
        }
    }
}
