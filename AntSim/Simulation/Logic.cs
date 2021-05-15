using AntSim.Graphics;
using AntSim.Simulation.Ants;
using AntSim.Simulation.Map;
using AntSim.Simulation.Map.Smells;

using System.Collections.Generic;

using SFML.System;

namespace AntSim.Simulation
{
    class Logic
    {
        public List<Colony> Colonies { get; }
        public Field<Cell> Smells { get; }

        private readonly Engine engine;

        public Logic()
        {
            Global.NumberConstants.Load();

            Colonies = new List<Colony>();
            var width = (uint)Global.NumberConstants.Get("WindowWidth");
            var height = (uint)Global.NumberConstants.Get("WindowHeight");
            engine = new Engine(width, height);
            Smells = new Field<Cell>(new MapGenerator(engine));
        }

        public void RunSimulation()
        {
            InitializeColonies();

            float deltaTime = Global.NumberConstants.Get("DeltaTime");
            while (engine.Active)
            {
                foreach (Colony colony in Colonies)
                {
                    foreach (Ant ant in colony.Ants)
                    {
                        ant.Step(deltaTime, Smells);
                        Global.Time.Increase();
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
            var chunkSize = (int)Global.NumberConstants.Get("ChunkSize");

            //Generate four chunks around colony;
            Smells[intCPosX, intCposY].Item = null;
            Smells[intCPosX - chunkSize, intCposY].Item = null;
            Smells[intCPosX, intCposY - chunkSize].Item = null;
            Smells[intCPosX - chunkSize, intCposY - chunkSize].Item = null;

            var queen = AntsFactory.CreateQueen();
            queen.Position = colonyPosition;
            Smells[intCPosX, intCposY].Smells[SmellType.Home] = new SmellInfo(SmellType.Home);
            engine.Register(queen);
            var colony = new Colony(0, colonyPosition, queen);
            for (int i = 0; i < 100; i++)
            {
                var ant = AntsFactory.CreateWorker();
                ant.Position = colonyPosition;
                colony.Ants.Add(ant);
                engine.Register(ant);
            }
            Colonies.Add(colony);
        }
    }
}
