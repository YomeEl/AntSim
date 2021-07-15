using AntSim.Simulation.Map.Smells;
using AntSim.Simulation.Items;

namespace AntSim.Simulation.Map
{
    class Cell
    {
        public IItem Item { get; set; }

        private readonly int[] smells;
        private readonly int[] timestamps;

        public Cell()
        {
            Item = null;

            smells = new int[SmellInfo.TYPES_COUNT];
            timestamps = new int[SmellInfo.TYPES_COUNT];

            for (int i = 0; i < smells.Length; i++)
            {
                smells[i] = 0;
                timestamps[i] = 0;
            }
        }

        public void SetSmell(SmellType type)
        {
            smells[(uint)type] = SmellInfo.GetMaxStrength(type);
            timestamps[(uint)type] = Global.Time.Get();
        }

        public int GetSmell(SmellType type)
        {
            var maxStrength = SmellInfo.GetMaxStrength(type);
            if (SmellInfo.IsPermanent(type))
            {
                return smells[(uint)type];
            }
            {
                return (int)(smells[(uint)type] - (Global.Time.Get() - timestamps[(uint)type]));
            }
            
        }

        public void RemoveSmell(SmellType type)
        {
            smells[(uint)type] = 0;
        }
    }
}
