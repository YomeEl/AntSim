using SFML.Graphics;
using SFML.System;

namespace AntSim.Graphics
{
    class GraphicalObject
    {
        public Texture Texture { get; }

        public (byte W, byte H) Size { get; }

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

            win.Draw(sprite);
        }
    }
}
