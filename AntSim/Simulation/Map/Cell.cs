using AntSim.Graphics;
using System.Collections.Generic;

namespace AntSim.Simulation.Map
{
    class Cell
    {
        public GraphicalObject Entity { get; set; } = null;
        public Dictionary<uint, uint> Smells { get; set; } = new Dictionary<uint, uint>();
    }
}
