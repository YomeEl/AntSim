using AntSim.Simulation.Map.Smells;
using AntSim.Simulation.Items;

using System.Collections.Generic;

namespace AntSim.Simulation.Map
{
    class Cell
    {
        public IItem Item { get; set; }

        private readonly Dictionary<SmellType, SmellInfo> smells;

        public Cell()
        {
            Item = null;
            smells = new Dictionary<SmellType, SmellInfo>();
        }

        public void SetSmell(SmellType type)
        {
            smells[type] = new SmellInfo(type);
        }

        public bool TryGetSmell(SmellType type, out SmellInfo smellInfo)
        {
            if (smells.TryGetValue(type, out SmellInfo smell))
            {
                if (smell.Strength > 0)
                {
                    smellInfo = smell;
                    return true;
                }
                else
                {
                    RemoveSmell(type);
                }
            }

            smellInfo = new SmellInfo(SmellType.Home);
            return false;
        }

        public void RemoveSmell(SmellType type)
        {
            smells.Remove(type);
        }
    }
}
