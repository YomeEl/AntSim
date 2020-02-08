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
        private readonly MapGenerator generator;
        private readonly SmellSystem smellSystem;
        public List<Ant> Ants { get; }
        public Field<Cell> Map { get; }

        public Logic()
        {
            Ants = new List<Ant>();
            generator = new MapGenerator();
            Map = new Field<Cell>(generator);
            smellSystem = new SmellSystem(Map);
            generator.SmellSystem = smellSystem;
            engine = new Engine(800, 600);

            InitialChunkGeneration();
        }

        public void SpawnHive(int x, int y, int radius)
        {
            if (Map[x, y].Entity != null) return;

            var queen = AntsFactory.CreateQueen();
            Map[x, y].Entity = queen;
            Ants.Add(queen);
            smellSystem.SpreadSmell(queen.FactionId, 100, queen.Position);

            const uint babysittersCount = 0;
            const uint soldiersCount = 0;
            const uint workersCount = 5;

            var antsToSpawn = new List<Ant>();
            for (int n = 0; n < babysittersCount; n++) antsToSpawn.Add(AntsFactory.CreateBabysitter());
            for (int n = 0; n < soldiersCount; n++) antsToSpawn.Add(AntsFactory.CreateSoldier());
            for (int n = 0; n < workersCount; n++) antsToSpawn.Add(AntsFactory.CreateWorker());
            //antsToSpawn.Add(AntsFactory.CreateBaby(AntType.Soldier));

            int spawned = 0;
            int i = x - radius;
            int j = y - radius;
            while (spawned != antsToSpawn.Count)
            {
                if (i != 0 || j != 0)
                {
                    Ants.Add(antsToSpawn[spawned]);
                    antsToSpawn[spawned].Position = new Vector2i(i, j);
                    Map[i++, j].Entity = antsToSpawn[spawned++];
                }
                else
                {
                    i++;
                }

                if (i > x + radius)
                {
                    i = x - radius;
                    j++;
                }
                if (j > y + radius)
                {
                    throw new Exception("Not enough place to spawn hive");
                }
            }
        }

        public void Step()
        {
            Cell[,] vicinity = new Cell[3, 3];

            foreach (Ant ant in Ants)
            {
                var center = ant.Position;
                for (int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        vicinity[i + 1, j + 1] = Map[center.X + i, center.Y + j];
                    }
                }

                var direction = ant.Move(vicinity);
                if (direction != Direction.Idle)
                {
                    ant.Rotation = direction;
                }

                switch (direction)
                {
                    case Direction.Up:
                        //if (Map[center.X, center.Y - 1].Entity == null)
                        {
                            Map[center.X, center.Y - 1].Entity = ant;
                            Map[center.X, center.Y].Entity = null;
                            ant.Position += new Vector2i(0, -1);
                        }
                        break;
                    case Direction.Right:
                        //if (Map[center.X + 1, center.Y].Entity == null)
                        {
                            Map[center.X + 1, center.Y].Entity = ant;
                            Map[center.X, center.Y].Entity = null;
                            ant.Position += new Vector2i(1, 0);
                        }
                        break;
                    case Direction.Down:
                        //if (Map[center.X, center.Y + 1].Entity == null)
                        {
                            Map[center.X, center.Y + 1].Entity = ant;
                            Map[center.X, center.Y].Entity = null;
                            ant.Position += new Vector2i(0, 1);
                        }
                        break;
                    case Direction.Left:
                        //if (Map[center.X - 1, center.Y].Entity == null)
                        {
                            Map[center.X - 1, center.Y].Entity = ant;
                            Map[center.X, center.Y].Entity = null;
                            ant.Position += new Vector2i(-1, 0);
                        }
                        break;
                }
            }
        }

        public void Debug()
        {
            uint tick = 0;

            SpawnHive(0, 0, 3);

            while (true)
            {
                var drawArray = new GraphicalObject[Ants.Count + generator.FoodPiles.Count];
                GraphicalObject[] antsArray = Ants.ToArray();
                GraphicalObject[] foodPilesArray = generator.FoodPiles.ToArray();
                antsArray.CopyTo(drawArray, 0);
                foodPilesArray.CopyTo(drawArray, Ants.Count);

                engine.Draw(drawArray);

                if (tick % 10 == 0) Step();
                tick++;
            };
        }

        private void InitialChunkGeneration()
        {
            for (int i = -10; i < 11; i++)
            {
                for (int j = -10; j < 11; j++)
                {
                    var pos = new Vector2i(i, j);
                    if (!Map.ChunkExists(pos))
                    {
                        Map.GenerateChunk(new Vector2i(i, j));
                    }
                }
            }
        }
    }
}
