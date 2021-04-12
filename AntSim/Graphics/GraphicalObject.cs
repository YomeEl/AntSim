using System;

using SFML.Graphics;
using SFML.System;

namespace AntSim.Graphics
{
    class GraphicalObject : IComparable
    {
        public Texture Texture { get; }
        public (byte W, byte H) Size { get; }
        public Vector2f Position { get; set; }
        public Vector2f Direction { get; set; }

        private static int nextId = 0;
        private int id;

        public GraphicalObject(Texture texture, byte width, byte height)
        {
            Texture = texture;
            Size = (width, height);
            Position = new Vector2f(0, 0);
            Direction = new Vector2f(0, 0);

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
