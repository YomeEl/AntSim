using AntSim.Graphics;
using AntSim.Simulation.Items;

using SFML.Graphics;

namespace AntSim.Simulation.Objects
{
    class FoodPile : GraphicalObject, IItem
    {
        public uint Count { get; set; }

        public FoodPile(uint count, Texture texture) : base(texture, 1, 1)
        {
            Count = count;
        }
    }
}
