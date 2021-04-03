using AntSim.Graphics;
using AntSim.Simulation.Ants;
using AntSim.Simulation.Map;
using AntSim.Simulation.Objects;

using SFML.System;

using System;
using System.Collections.Generic;

namespace AntSim.Simulation
{
    class Logic
    {
        private readonly Engine engine;

        public List<Ant> Ants { get; }
        public Field<Cell> Map { get; }

        public Logic()
        {
            Ants = new List<Ant>();
            Map = new Field<Cell>(new MapGenerator());
            engine = new Engine(800, 600);
        }

        public void RunSimulation()
        {
            
        }
    }
}
