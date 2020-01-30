namespace AntSim.Simulation.Map
{
    class Chunk<T>
    {
        public const byte SIZE = 16;

        public Chunk()
        {
            Grid = new T[SIZE, SIZE];
        }

        public T[,] Grid { get; set; }
    }
}
