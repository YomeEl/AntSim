using AntSim.Simulation.Map;
using AntSim.Simulation.Map.Smells;
using AntSim.Simulation.Objects;

using System;
using System.Collections.Generic;

using SFML.System;

namespace AntSim.Simulation
{
    class MapGenerator : IGenerator<Cell>
    {
        public List<FoodPile> FoodPiles { get; }

        private readonly Random randomizer;
        private readonly Graphics.Engine engine;
        private readonly int FOOD_PILE_RADIUS;
        private readonly int CHUNK_SIZE;

        public MapGenerator(Graphics.Engine engine)
        {
            FoodPiles = new List<FoodPile>();

            FOOD_PILE_RADIUS = (int)Global.NumberConstants.Get("FoodPileRadius");
            CHUNK_SIZE = (int)Global.NumberConstants.Get("ChunkSize");

            randomizer = new Random(32);
            this.engine = engine;
        }

        public Chunk<Cell> GenerateChunk(Vector2i position)
        {
            var chunk = new Chunk<Cell>();
            var center = new Vector2i(1, 1) * (CHUNK_SIZE / 2);
            for (int i = 0; i < CHUNK_SIZE; i++)
            {
                for (int j = 0; j < CHUNK_SIZE; j++)
                {
                    chunk.Grid[i, j] = new Cell();
                }
            }

            if (position.X != 0 && position.Y != 0 && randomizer.Next(0, 4) == 0)
            {
                GenerateFoodPile(chunk, position, center, FOOD_PILE_RADIUS);
            }

            return chunk;
        }

        private void GenerateFoodPile(Chunk<Cell> chunk, Vector2i chunkPosition, Vector2i relativeFoodPosition, int radius)
        {
            if (radius == 0 || chunk.Grid[relativeFoodPosition.X, relativeFoodPosition.Y].Item != null)
            {
                return;
            }

            var food = ObjectsFactory.CreateFoodPile();
            float posX = relativeFoodPosition.X + chunkPosition.X * CHUNK_SIZE;
            float posY = relativeFoodPosition.Y + chunkPosition.Y * CHUNK_SIZE;
            food.Position = new Vector2f(posX, posY);

            chunk.Grid[relativeFoodPosition.X, relativeFoodPosition.Y].Item = food;
            chunk.Grid[relativeFoodPosition.X, relativeFoodPosition.Y].Smells[SmellType.Food] = new SmellInfo(SmellType.Food);

            engine.Register(food);

            if (randomizer.Next(0, 10) <= 7)
            {
                GenerateFoodPile(chunk, chunkPosition, relativeFoodPosition + new Vector2i(1, 0), radius - 1);
            }
            if (randomizer.Next(0, 10) <= 7)
            {
                GenerateFoodPile(chunk, chunkPosition, relativeFoodPosition + new Vector2i(-1, 0), radius - 1);
            }
            if (randomizer.Next(0, 10) <= 7)
            {
                GenerateFoodPile(chunk, chunkPosition, relativeFoodPosition + new Vector2i(0, 1), radius - 1);
            }
            if (randomizer.Next(0, 10) <= 7)
            {
                GenerateFoodPile(chunk, chunkPosition, relativeFoodPosition + new Vector2i(0, -1), radius - 1);
            }
        }
    }
}
