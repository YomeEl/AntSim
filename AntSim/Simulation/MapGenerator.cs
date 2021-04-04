using AntSim.Simulation.Map;
using AntSim.Simulation.Objects;

using SFML.System;

using System;
using System.Collections.Generic;

namespace AntSim.Simulation
{
    class MapGenerator : IGenerator<Cell>
    {
        public Cell DefaultValue { get; }
        public List<FoodPile> FoodPiles { get; }

        private readonly Random randomizer;
        private readonly Graphics.Engine engine;

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
            var size = Chunk<Cell>.SIZE;

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    chunk.Grid[i, j] = new Cell();
                    if (randomizer.Next(0, 25000) == 0)
                    {
                        var foodPile = ObjectsFactory.CreateFoodPile();
                        chunk.Grid[i, j].Item = foodPile;
                        float posX = i + position.X * size;
                        float posY = j + position.Y * size;
                        foodPile.Position = new Vector2f(posX, posY);
                        engine.Register(foodPile);
                    }
                }
            }

            return chunk;
        }
    }
}
