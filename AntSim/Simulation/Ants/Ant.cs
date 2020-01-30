using AntSim.Graphics;
using AntSim.Simulation.Map;

using SFML.System;

using System;

namespace AntSim.Simulation.Ants
{
    abstract class Ant : GraphicalObject
    {
        protected Random randomizer;

        public int Id { get; set; }

        public Vector2i Position { get; set; }

        public float Health { get; }

        public float Food { get; }

        public float Speed { get; }

        public float Age { get; private set; }

        public Ant(int id, SFML.Graphics.Texture texture, byte width, byte height) : base(texture, width, height)
        {
            Health = 1;
            Food = 1;
            Speed = 1;
            Age = 1;

            Id = id;
            randomizer = new Random(id);
        }

        public abstract Direction Move(Cell[,] vicinity);
    }
}