using SFML.System;

namespace AntSim.Simulation.Map
{
    interface IGenerator<T>
    {
        Chunk<T> GenerateChunk(Vector2i position);
    }
}
