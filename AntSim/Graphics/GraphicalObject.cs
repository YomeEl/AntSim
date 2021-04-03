using SFML.Graphics;
using SFML.System;

using AntSim.Simulation.Ants;

namespace AntSim.Graphics
{
    class GraphicalObject
    {
        public Texture Texture { get; }
        public (byte W, byte H) Size { get; }
        public Vector2f Position { get; set; }
        public float Rotation { get; set; } = 0f;

        public GraphicalObject(Texture texture, byte width, byte height)
        {
            Texture = texture;
            Size = (width, height);
        }
    }
}
