﻿using AntSim.Simulation.Map;
using AntSim.Simulation.Objects;

using SFML.System;

using System.Collections.Generic;

namespace AntSim.Simulation
{
    class MapGenerator : IGenerator<Cell>
    {
        private System.Random randomizer = new System.Random(32);
        public Cell DefaultValue { get; } = new Cell();
        public List<FoodPile> FoodPiles { get; } = new List<FoodPile>();

        public Chunk<Cell> GenerateChunk(Vector2i position)
        {
            var chunk = new Chunk<Cell>();
            var size = Chunk<Cell>.SIZE;

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    chunk.Grid[i, j] = new Cell();
                    if (randomizer.Next(0, 9999) == 0)
                    {
                        var foodPile = ObjectsFactory.CreateFoodPile();
                        chunk.Grid[i, j].Entity = foodPile;
                        foodPile.Position = new Vector2i(i, j) + position * size;
                        FoodPiles.Add(foodPile);
                    }
                    else
                    {
                        chunk.Grid[i, j].Entity = null;
                    }
                }
            }

            return chunk;
        }
    }
}