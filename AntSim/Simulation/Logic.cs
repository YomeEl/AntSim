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

        public List<Colony> Colonies { get; }
        public Field<Cell> Smells { get; }

        public Logic()
        {
            Colonies = new List<Colony>();
            engine = new Engine(800, 600);
            Smells = new Field<Cell>(new MapGenerator(engine));
        }

        public void RunSimulation()
        {
            InitializeColonies();

            float deltaTime = 1f;
            while (engine.Active)
            {
                foreach (Colony colony in Colonies)
                {
                    foreach (Ant ant in colony.Ants)
                    {
                        ant.Step(deltaTime, Smells);
                    }
                }

                engine.Draw();
            }
        }

        private void InitializeColonies()
        {
            var colonyPosition = new Vector2f(0, 0);

            int intCPosX = (int)colonyPosition.X;
            int intCposY = (int)colonyPosition.Y; 
            var chunkSize = Chunk<Cell>.SIZE;

            //Generate four chunks around colony;
            Smells[intCPosX, intCposY].Item = null;
            Smells[intCPosX - chunkSize, intCposY].Item = null;
            Smells[intCPosX, intCposY - chunkSize].Item = null;
            Smells[intCPosX - chunkSize, intCposY - chunkSize].Item = null;

            var queen = AntsFactory.CreateQueen();
            queen.Position = colonyPosition;
            engine.Register(queen);
            var colony = new Colony(0, colonyPosition, queen);
            for (int i = 0; i < 100; i++)
            {
                var ant = AntsFactory.CreateWorker();
                ant.Position = colonyPosition;
                colony.Ants.Add(ant);
                engine.Register(ant);
            }
        }
    }
}
