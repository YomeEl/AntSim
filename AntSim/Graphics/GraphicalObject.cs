using SFML.Graphics;
using SFML.System;

using AntSim.Simulation.Ants;

namespace AntSim.Graphics
{
    class GraphicalObject
    {
        public Texture Texture { get; }
        public (byte W, byte H) Size { get; }
        public Vector2i Position { get; set; }
        public Direction Rotation { get; set; } = Direction.Up;

        public GraphicalObject(Texture texture, byte width, byte height)
        {
            Texture = texture;
            Size = (width, height);
        }

        public void Draw(RenderWindow win, Vector2i position, byte cellSize)
        {
            var sprite = new Sprite(Texture)
            {
                Position = new Vector2f(position.X, position.Y),
                Scale = new Vector2f((float)cellSize * Size.W / Texture.Size.X, (float)cellSize * Size.H / Texture.Size.Y)
            };

            sprite.Rotation = -90f * (float)Rotation;

            win.Draw(sprite);
        }
    }
}
