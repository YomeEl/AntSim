using AntSim.Simulation.Map;

using System;

namespace AntSim.Simulation.Ants
{
    class Soldier : Ant
    {
        public Soldier(uint antId, uint factionId, SFML.Graphics.Sprite sprite) :
            base(antId, factionId, sprite)
        {
        }

        public override void Step(float dt, Field<Cell> field)
        {
            throw new NotImplementedException();
        }
    }
}
