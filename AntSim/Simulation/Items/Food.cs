namespace AntSim.Simulation.Items
{
    class Food : IItem
    {
        public uint Count { get; set; }

        public Food(uint count)
        {
            Count = count;
        }
    }
}
