using AntSim.Graphics;
using AntSim.Simulation.Map;
using AntSim.Simulation.Items;

using SFML.System;

using System;

namespace AntSim.Simulation.Ants
{
    abstract class Ant : GraphicalObject
    {
        protected Random randomizer;

        public int Id { get; set; }

        public Vector2i Position { get; set; }

        public IItem Item { get; protected set; }

        public Ant(int id, SFML.Graphics.Texture texture, byte width, byte height) : base(texture, width, height)
        {
            Id = id;
            randomizer = new Random(id);
        }

        protected bool IsVicinityCorrect(Cell[,] vicinity, byte radius)
        {
            return vicinity.Length == radius * radius;
        }

        public abstract Direction Move(Cell[,] vicinity);
    }
}