using AntSim.Simulation.Map;
using AntSim.Simulation.Objects;

using System;
using System.Collections.Generic;

using SFML.System;

namespace AntSim.Simulation
{
    class MapGenerator : IGenerator<Cell>
    {
        public Cell DefaultValue { get; }
        public List<FoodPile> FoodPiles { get; }

        private readonly Random randomizer;
        private readonly Graphics.Engine engine;
        private const int FOOD_PILE_RADIUS = 20;

        public MapGenerator(Graphics.Engine engine)
        {
            DefaultValue = new Cell();
            FoodPiles = new List<FoodPile>();

            randomizer = new Random(32);
            this.engine = engine;
        }

        public Chunk<Cell> GenerateChunk(Vector2i position)
        {
            var chunk = new Chunk<Cell>();
            const int CHUNK_SIZE = Chunk<Cell>.SIZE;
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
            float posX = relativeFoodPosition.X + chunkPosition.X * Chunk<Cell>.SIZE;
            float posY = relativeFoodPosition.Y + chunkPosition.Y * Chunk<Cell>.SIZE;
            food.Position = new Vector2f(posX, posY);

            chunk.Grid[relativeFoodPosition.X, relativeFoodPosition.Y].Item = food;
            chunk.Grid[relativeFoodPosition.X, relativeFoodPosition.Y].Smells.Add(new Map.Smells.SmellInfo(Map.Smells.SmellType.Food, 1));

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
