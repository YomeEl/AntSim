using SFML.System;
using System.Collections.Generic;

namespace AntSim.Simulation.Map
{
    class Field<T>
    {
        private readonly IGenerator<T> generator;
        private readonly Dictionary<Vector2i, Chunk<T>> chunks;

        private Chunk<T> GetChunk(int x, int y)
        {
            var targetChunkCoords = GetChunkCoords(x, y);
            Chunk<T> targetChunk = null;

            if (chunks.ContainsKey(targetChunkCoords))
            {
                targetChunk = chunks[targetChunkCoords];
            }

            return targetChunk;
        }

        private Vector2i GetChunkCoords(int x, int y)
        {
            var targetChunkCoords = new Vector2i();

            if (x >= 0)
            {
                targetChunkCoords.X = x / Chunk<T>.SIZE;
            }
            else
            {
                targetChunkCoords.X = (x + 1) / Chunk<T>.SIZE - 1;
            }

            if (y >= 0)
            {
                targetChunkCoords.Y = y / Chunk<T>.SIZE;
            }
            else
            {
                targetChunkCoords.Y = (y + 1) / Chunk<T>.SIZE - 1;
            }

            return targetChunkCoords;
        }

        private Vector2i GetLocalCoords(int x, int y)
        {
            var targetCoords = new Vector2i();

            if (x >= 0)
            {
                targetCoords.X = x % Chunk<T>.SIZE;
            }
            else
            {
                targetCoords.X = Chunk<T>.SIZE + ((x + 1) % Chunk<T>.SIZE) - 1;
            }

            if (y >= 0)
            {
                targetCoords.Y = y % Chunk<T>.SIZE;
            }
            else
            {
                targetCoords.Y = Chunk<T>.SIZE + ((y + 1) % Chunk<T>.SIZE) - 1;
            }

            return targetCoords;
        }

        public Field(IGenerator<T> generator)
        {
            chunks = new Dictionary<Vector2i, Chunk<T>>();
            this.generator = generator;
        }

        public T this[int x, int y]
        {
            get
            {
                var targetChunk = GetChunk(x, y);

                if (targetChunk == null)
                {
                    targetChunk = GenerateChunk(GetChunkCoords(x, y));
                }

                var targetCoords = GetLocalCoords(x, y);

                return targetChunk.Grid[targetCoords.X, targetCoords.Y];
            }

            set
            {
                var targetChunk = GetChunk(x, y);
                var targetCoords = GetLocalCoords(x, y);

                if (targetChunk == null)
                {
                    targetChunk = GenerateChunk(GetChunkCoords(x, y));
                }

                targetChunk.Grid[targetCoords.X, targetCoords.Y] = value;
            }
        }

        public Chunk<T> GenerateChunk(Vector2i coords)
        {
            var newChunk = generator.GenerateChunk(coords);
            chunks[coords] = newChunk;
            return newChunk;
        }
    }
}
