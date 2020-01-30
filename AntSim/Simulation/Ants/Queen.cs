using AntSim.Simulation.Map;
using System;

namespace AntSim.Simulation.Ants
{
    class Queen : Ant
    {
        public Queen(int id, SFML.Graphics.Texture texture, byte width, byte height) : base(id, texture, width, height)
        {
        }

        public override Direction Move(Cell[,] vicinity)
        {
            throw new NotImplementedException();
        }
    }
}
