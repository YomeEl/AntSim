namespace AntSim.Simulation.Map
{
    class Chunk<T>
    {
        public readonly byte SIZE;

        public Chunk()
        {
            SIZE = (byte)Global.NumberConstants.Get("ChunkSize");
            Grid = new T[SIZE, SIZE];
        }

        public T[,] Grid { get; set; }
    }
}
