using AntSim.Simulation.Map;

namespace AntSim.Simulation.Ants
{
    class Worker : Ant
    {
        public Worker(uint antId, uint factionId, SFML.Graphics.Texture texture, byte width, byte height) : 
            base(antId, factionId, texture, width, height)
        {
        }

        public override void Step(float dt, Field<Cell> field)
        {

        }
    }
}
