namespace AntSim.Simulation.Map.Smells
{
    class SmellInfo
    {
        public const uint TYPES_COUNT = 4;

        public static int GetMaxStrength(SmellType type)
        {
            return (int)Global.NumberConstants.Get("MaxStrength_" + type.ToString());
        }

        public static bool IsPermanent(SmellType type)
        {
            return type == SmellType.Food || type == SmellType.Home;
        }
    }
}
