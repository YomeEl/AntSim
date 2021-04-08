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
        public float Rotation { get; set; } 

        private static int nextId = 0;
        private int id;

        public GraphicalObject(Texture texture, byte width, byte height)
        {
            Texture = texture;
            Size = (width, height);
            Rotation = 0f;

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

        public Vector2f GetDirection()
        {
            var rad = ToRadians(Rotation);
            return new Vector2f(Position.X * (float)Math.Cos(rad), Position.Y * (float)Math.Sin(rad));
        }

        private float ToRadians(float deg)
        {
            return deg / 360f * (float)Math.PI * 2;
        }
    }
}
