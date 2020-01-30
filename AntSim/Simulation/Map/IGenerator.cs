using SFML.System;

namespace AntSim.Simulation.Map
{
    interface IGenerator<T>
    {
        T DefaultValue { get; }
        Chunk<T> GenerateChunk(Vector2i position);
    }
}
