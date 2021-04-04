using System.Collections.Generic;

using SFML.System;

namespace AntSim.Simulation
{
    class Colony
    {
        public int Id { get; }
        public List<Ants.Ant> Ants { get; }
        public Vector2f Position { get; }

        public Colony(int id, Vector2f position)
        {
            Id = id;
            Ants = new List<Ants.Ant>();
            Position = position;
        }
    }
}
