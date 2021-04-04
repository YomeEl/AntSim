using SFML.Graphics;
using SFML.System;

using System;

namespace AntSim.Graphics
{
    class GraphicalObject : IComparable
    {
        public Texture Texture { get; }
        public (byte W, byte H) Size { get; }
        public Vector2f Position { get; set; }
        public float Rotation { get; set; } = 0f;

        private static int nextId = 0;
        private int id;

        public GraphicalObject(Texture texture, byte width, byte height)
        {
            Texture = texture;
            Size = (width, height);

            id = nextId++;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
            {
                return 1;
            }

            if (obj is GraphicalObject)
            {
                return id.CompareTo(((GraphicalObject)obj).id);
            }
            else
            {
                throw new Exception("Trying to compare objects of different types!");
            }
        }
    }
}
