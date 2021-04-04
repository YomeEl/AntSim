using AntSim.Simulation.Map;

namespace AntSim.Simulation.Ants
{
    class Queen : Ant
    {
        public uint FoodStock { get; set; }

        public Queen(uint antId, uint factionId, SFML.Graphics.Texture texture, byte width, byte height) :
            base(antId, factionId, texture, width, height)
        {
        }

        public override void Step(float dt, Field<Cell> field)
        {
            
        }
    }
}
