using System;

using SFML.Graphics;
using SFML.System;

namespace AntSim.Graphics
{
    class GraphicalObject : IComparable
    {
        public Sprite Sprite { get; }
        public Vector2f Position { get; set; }
        public Vector2f Direction 
        { 
            get => currentDirection; 
            set 
            { 
                desiredDirection = value;
            } 
        }
        public bool IsStatic { get; set; }
        public bool ShouldBeDestroyed { get; set; }

        protected bool IsRotating { get; private set; }

        private const float SPEED_OF_ROTATION = 0.1f;
        private Vector2f desiredDirection;
        private Vector2f currentDirection;

        private static int nextId = 0;
        private readonly int id;

        private byte oldCellSize = 1;

        public GraphicalObject(Sprite sprite)
        {
            Sprite = sprite;
            Position = new Vector2f(0, 0);
            Direction = new Vector2f(0, 0);
            IsStatic = false;
            ShouldBeDestroyed = false;

            IsRotating = false;

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

        public void UpdateDirection()
        {
            if (Distance(desiredDirection, currentDirection) > 0.01f)
            {
                currentDirection = Normalize(currentDirection + desiredDirection * SPEED_OF_ROTATION);
                IsRotating = true;
            }
            else
            {
                IsRotating = false;
            }
        }

        public void RescaleSprite(byte cellSize)
        {
            if (cellSize != oldCellSize)
            {
                Sprite.Scale *= (float)cellSize / oldCellSize;
                oldCellSize = cellSize;
            }
        }
        
        protected Vector2f Normalize(Vector2f vect)
        {
            var invertedLen = 1/(float)Math.Sqrt(vect.X * vect.X + vect.Y * vect.Y);
            return vect * invertedLen;
        }

        protected float Distance(Vector2f a, Vector2f b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        }

        protected int Distance(Vector2i a, Vector2i b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        }
    }
}
