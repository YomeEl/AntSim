using AntSim.Graphics;
using System.Collections.Generic;

namespace AntSim.Simulation.Map
{
    class Cell
    {
        public GraphicalObject Entity { get; set; } = null;
        public Dictionary<byte, uint> Smells { get; set; } = new Dictionary<byte, uint>();
    }
}
