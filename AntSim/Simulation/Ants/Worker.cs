using AntSim.Simulation.Map;
using AntSim.Simulation.Objects;
using AntSim.Simulation.Items;

using SFML.System;

using System;

namespace AntSim.Simulation.Ants
{
    class Worker : Ant
    {
        public Worker(uint antId, uint factionId, SFML.Graphics.Texture texture, byte width, byte height) : 
            base(antId, factionId, texture, width, height)
        {
        }

        public override void Step(float dt)
        {

        }
    }
}
