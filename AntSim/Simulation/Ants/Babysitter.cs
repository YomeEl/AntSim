using AntSim.Simulation.Map;
using System;

namespace AntSim.Simulation.Ants
{
    class Babysitter : Ant
    {
        public Babysitter(uint antId, uint factionId, SFML.Graphics.Texture texture, byte width, byte height) :
            base(antId, factionId, texture, width, height)
        {
        }

        public override Direction Move(Cell[,] vicinity)
        {
            throw new NotImplementedException();
        }
    }
}
