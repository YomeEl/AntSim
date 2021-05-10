using System.Collections.Generic;

using SFML.System;

namespace AntSim.Simulation.Map
{
    class Field<T>
    {
        private readonly IGenerator<T> generator;
        private readonly Dictionary<Vector2i, Chunk<T>> chunks;
        private readonly int ChunkSize;

        public Field(IGenerator<T> generator)
        {
            chunks = new Dictionary<Vector2i, Chunk<T>>();
            ChunkSize = (int)Global.NumberConstants.Get("ChunkSize");
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
                targetChunkCoords.X = x / ChunkSize;
            }
            else
            {
                targetChunkCoords.X = (x + 1) / ChunkSize - 1;
            }

            if (y >= 0)
            {
                targetChunkCoords.Y = y / ChunkSize;
            }
            else
            {
                targetChunkCoords.Y = (y + 1) / ChunkSize - 1;
            }

            return targetChunkCoords;
        }

        private Vector2i GetLocalCoords(int x, int y)
        {
            var targetCoords = new Vector2i();

            if (x >= 0)
            {
                targetCoords.X = x % ChunkSize;
            }
            else
            {
                targetCoords.X = ChunkSize + ((x + 1) % ChunkSize) - 1;
            }

            if (y >= 0)
            {
                targetCoords.Y = y % ChunkSize;
            }
            else
            {
                targetCoords.Y = ChunkSize + ((y + 1) % ChunkSize) - 1;
            }

            return targetCoords;
        }
    }
}
