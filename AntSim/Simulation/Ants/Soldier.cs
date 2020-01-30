using AntSim.Simulation.Map;
using System;

namespace AntSim.Simulation.Ants
{
    class Soldier : Ant
    {
        public Soldier(int id, SFML.Graphics.Texture texture, byte width, byte height) : base(id, texture, width, height)
        {
        }

        public override Direction Move(Cell[,] vicinity)
        {
            throw new NotImplementedException();
        }
    }
}
