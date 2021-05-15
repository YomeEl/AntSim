namespace AntSim.Simulation.Map.Smells
{
    class SmellInfo
    {
        public int Strength {
            get => IsPermanent() ? maxStrength : maxStrength - (Global.Time.Get() - timestamp);
        }
        public SmellType Type { get; }

        private readonly int timestamp;
        private readonly int maxStrength;

        public SmellInfo(SmellType type)
        {
            Type = type;
            timestamp = Global.Time.Get();
            maxStrength = (int)Global.NumberConstants.Get("MaxStrength_" + type.ToString());
        }

        public bool IsPermanent()
        {
            return Type == SmellType.Food || Type == SmellType.Home;
        }
    }
}
