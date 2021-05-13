namespace AntSim.Simulation.Global
{
    static class Time
    {
        private static int current = 0;

        public static void Increase()
        {
            current++;
        }
        public static int Get()
        {
            return current;
        }
    }
}
