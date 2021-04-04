using AntSim.Simulation.Map;

namespace AntSim.Simulation.Ants
{
    class Baby : Ant
    {
        public AntType Type { get; }
        public Baby(AntType type, uint antId, uint factionId, SFML.Graphics.Texture texture, byte width, byte height) :
            base(antId, factionId, texture, width, height)
        {
            Type = type;
        }

        public override void Step(float dt, Field<Cell> field)
        {
            throw new System.NotImplementedException();
        }
    }
}
