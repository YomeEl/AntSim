using AntSim.Simulation.Map.Smells;
using AntSim.Simulation.Items;

using System.Collections.Generic;

namespace AntSim.Simulation.Map
{
    class Cell
    {
        public IItem Item { get; set; }
        public Dictionary<SmellType, SmellInfo> Smells { get; }

        public Cell()
        {
            Item = null;
            Smells = new Dictionary<SmellType, SmellInfo>();
        }
    }
}
