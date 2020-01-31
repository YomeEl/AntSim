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
        private EntityMapGenerator generator;
        public List<Ant> Ants { get; private set; }
        public Field<Cell> EntityMap { get; private set; }

        public Logic()
        {
            Ants = new List<Ant>();
            generator = new EntityMapGenerator();
            EntityMap = new Field<Cell>(generator);
            InitialChunkGeneration();
            engine = new Engine(800, 600);
        }

        private void SpreadSmell(byte code, uint strength, Vector2i from)
        {
            int radius = (int)strength;
            for (int i = from.X - radius; i < from.X + radius; i++)
            {
                for (int j = from.Y - radius; j < from.Y + radius; j++)
                {
                    var dist = Math.Sqrt(Math.Pow(i - from.X, 2) + Math.Pow(j - from.Y, 2));
                    if (dist <= radius)
                    {
                        if (EntityMap[i, j].Smells.ContainsKey(code) &&
                            EntityMap[i, j].Smells[code] < strength - (uint)dist ||
                            !EntityMap[i, j].Smells.ContainsKey(code))
                        {
                            EntityMap[i, j].Smells[code] = strength - (uint)dist;
                        }
                    }
                }
            }
        }

        private void SpreadFoodSmell()
        {
            foreach (FoodPile foodPile in generator.FoodPiles)
            {
                SpreadSmell(0, 25, foodPile.Position);
            }
        }

        public void SpawnHive(int x, int y, int radius)
        {
            if (EntityMap[x, y].Entity != null) return;

            //var queen = AntsFactory.CreateQueen();
            //EntityMap[x, y].Entity = queen;
            //Ants.Add(queen);

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
                    EntityMap[i++, j].Entity = antsToSpawn[spawned++];
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
                    throw new System.Exception("Not enough place to spawn hive");
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
                        vicinity[i + 1, j + 1] = EntityMap[center.X + i, center.Y + j];
                    }
                }

                switch (ant.Rotation = ant.Move(vicinity))
                {
                    case Direction.Up:
                        if (EntityMap[center.X, center.Y - 1].Entity == null)
                        {
                            EntityMap[center.X, center.Y - 1].Entity = ant;
                            EntityMap[center.X, center.Y].Entity = null;
                            ant.Position += new Vector2i(0, -1);
                        }
                        break;
                    case Direction.Right:
                        if (EntityMap[center.X + 1, center.Y].Entity == null)
                        {
                            EntityMap[center.X + 1, center.Y].Entity = ant;
                            EntityMap[center.X, center.Y].Entity = null;
                            ant.Position += new Vector2i(1, 0);
                        }
                        break;
                    case Direction.Down:
                        if (EntityMap[center.X, center.Y + 1].Entity == null)
                        {
                            EntityMap[center.X, center.Y + 1].Entity = ant;
                            EntityMap[center.X, center.Y].Entity = null;
                            ant.Position += new Vector2i(0, 1);
                        }
                        break;
                    case Direction.Left:
                        if (EntityMap[center.X - 1, center.Y].Entity == null)
                        {
                            EntityMap[center.X - 1, center.Y].Entity = ant;
                            EntityMap[center.X, center.Y].Entity = null;
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

            while (engine.Draw(EntityMap))
            {
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
                    EntityMap.GenerateChunk(new Vector2i(i, j));
                }
            }
            SpreadFoodSmell();
        }
    }
}
