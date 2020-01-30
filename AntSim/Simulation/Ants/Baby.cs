using AntSim.Simulation.Map;

namespace AntSim.Simulation.Ants
{
    class Baby : Ant
    {
        public AntType Type { get; }
        public Baby(AntType type, int id, SFML.Graphics.Texture texture, byte width, byte height) : base(id, texture, width, height)
        {
            Type = type;
        }

        public override Direction Move(Cell[,] vicinity)
        {
            throw new System.NotImplementedException();
        }
    }
}
