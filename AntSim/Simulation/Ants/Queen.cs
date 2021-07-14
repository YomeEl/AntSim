using AntSim.Simulation.Map;

namespace AntSim.Simulation.Ants
{
    class Queen : Ant
    {
        public uint FoodStock { get; set; }

        public Queen(uint antId, uint factionId, SFML.Graphics.Sprite sprite) :
            base(antId, factionId, sprite)
        {
        }

        public override void Step(float dt, Field<Cell> field)
        {
            
        }
    }
}
