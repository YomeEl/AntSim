using AntSim.Simulation.Map.Smells;
using AntSim.Simulation.Items;

using System.Collections.Generic;

namespace AntSim.Simulation.Map
{
    class Cell
    {
        public IItem Item { get; set; }
        public List<SmellInfo> Smells { get; }

        public Cell()
        {
            Item = null;
            Smells = new List<SmellInfo>();
        }
    }
}
