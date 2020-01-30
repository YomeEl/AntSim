using AntSim.Simulation.Map;

namespace AntSim.Simulation.Ants
{
    class Worker : Ant
    {
        public Worker(int id, SFML.Graphics.Texture texture, byte width, byte height) : base(id, texture, width, height)
        {
        }

        public override Direction Move(Cell[,] vicinity)
        {
            Direction dir = Direction.Forward;
            uint maxIntesity = 0;

            if (vicinity[1, 0].Smells.ContainsKey(0) && vicinity[1, 0].Smells[0] > maxIntesity)
            {
                dir = Direction.Forward;
                maxIntesity = vicinity[1, 0].Smells[0];

            }
            if (vicinity[0, 1].Smells.ContainsKey(0) && vicinity[0, 1].Smells[0] > maxIntesity)
            {
                dir = Direction.Left;
                maxIntesity = vicinity[0, 1].Smells[0];
            }
            if (vicinity[2, 1].Smells.ContainsKey(0) && vicinity[2, 1].Smells[0] > maxIntesity)
            {
                dir = Direction.Right;
                maxIntesity = vicinity[2, 1].Smells[0];
            }
            if (vicinity[1, 2].Smells.ContainsKey(0) && vicinity[1, 2].Smells[0] > maxIntesity)
            {
                dir = Direction.Back;
                maxIntesity = vicinity[1, 2].Smells[0];
            }

            if (maxIntesity == 0)
            {
                dir = (Direction)randomizer.Next(0, 3);
            }

            return dir;
        }
    }
}
