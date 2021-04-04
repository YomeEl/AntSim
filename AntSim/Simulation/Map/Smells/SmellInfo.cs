namespace AntSim.Simulation.Map.Smells
{
    class SmellInfo
    {
        public int Strength { get; }
        public SmellType Type { get; }

        public SmellInfo(SmellType type, int strength)
        {
            Type = type;
            Strength = strength;
        }
    }
}
