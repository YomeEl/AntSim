using AntSim.Simulation.Ants;

using System.Collections.Generic;

using SFML.System;

namespace AntSim.Simulation
{
    class Colony
    {
        public int Id { get; }
        public Queen Queen { get; }
        public List<Ant> Ants { get; }
        public Vector2f Position { get; }

        public Colony(int id, Vector2f position, Queen queen)
        {
            Id = id;
            Queen = queen;
            Ants = new List<Ants.Ant>();
            Position = position;
        }
    }
}
