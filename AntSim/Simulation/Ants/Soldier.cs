using AntSim.Simulation.Map;
using System;

namespace AntSim.Simulation.Ants
{
    class Soldier : Ant
    {
        public Soldier(uint antId, uint factionId, SFML.Graphics.Texture texture, byte width, byte height) :
            base(antId, factionId, texture, width, height)
        {
        }

        public override void Step(float dt, Field<Cell> field)
        {

        }
    }
}
