using AntSim.Simulation.Map;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace AntSim.Graphics
{
    class Engine
    {
        private readonly RenderWindow win;
        private byte cellSize;
        private Vector2i previousMousePosition;

        private Vector2f cameraPosition;

        public Engine(uint width, uint height)
        {
            win = new RenderWindow(new VideoMode(width, height), "Ant simulation");
            win.SetFramerateLimit(60);

            BindEvents(win);

            cellSize = 30;
        }

        private void ChangeCellSize(float amount)
        {
            float newCellSize = cellSize;
            newCellSize += amount;
            if (newCellSize < 2) newCellSize = 2;
            if (newCellSize > 100) newCellSize = 100;
            cameraPosition *= newCellSize / cellSize;
            cellSize = (byte)newCellSize;
        }

        private void Win_MouseWheelScrolled(object sender, MouseWheelScrollEventArgs e)
        {
            ChangeCellSize(e.Delta);
        }

        private void Win_Closed(object sender, System.EventArgs e)
        {
            win.Close();
        }

        private void BindEvents(RenderWindow win)
        {
            win.Closed += Win_Closed;
            win.MouseWheelScrolled += Win_MouseWheelScrolled;
        }

        private void ProcessCameraEvents()
        {
            float speed = Keyboard.IsKeyPressed(Keyboard.Key.LShift) ? 15 : 5;

            if (Keyboard.IsKeyPressed(Keyboard.Key.W)) cameraPosition.Y -= speed;
            if (Keyboard.IsKeyPressed(Keyboard.Key.A)) cameraPosition.X -= speed;
            if (Keyboard.IsKeyPressed(Keyboard.Key.S)) cameraPosition.Y += speed;
            if (Keyboard.IsKeyPressed(Keyboard.Key.D)) cameraPosition.X += speed;

            if (Keyboard.IsKeyPressed(Keyboard.Key.Add) && cellSize < 100) ChangeCellSize(1);
            if (Keyboard.IsKeyPressed(Keyboard.Key.Subtract) && cellSize > 2) ChangeCellSize(-1);

            var currentMousePosition = Mouse.GetPosition();
            if (Mouse.IsButtonPressed(Mouse.Button.Left))
            {
                cameraPosition.X += previousMousePosition.X - currentMousePosition.X;
                cameraPosition.Y += previousMousePosition.Y - currentMousePosition.Y;
            }
            previousMousePosition = currentMousePosition;
        }

        public bool Draw(GraphicalObject[] objects)
        {
            ProcessCameraEvents();

            win.Clear(Color.White);

            byte maxAntSize = 4;

            float trueLeft = cameraPosition.X - win.Size.X / 2;
            float trueTop = cameraPosition.Y - win.Size.Y / 2;
            int left = (int)(trueLeft / cellSize);
            int top = (int)(trueTop / cellSize);
            int width = (int)win.Size.X / cellSize + 1;
            int height = (int)win.Size.Y / cellSize + 1;
            (int X, int Y) offset = ((int)cameraPosition.X, (int)cameraPosition.Y);

            foreach (GraphicalObject obj in objects)
            {
                if (obj.Position.X >= left - maxAntSize && obj.Position.X <= left + width &&
                    obj.Position.Y >= top - maxAntSize && obj.Position.Y <= top + height)
                {
                    var pos = new Vector2i(obj.Position.X * cellSize - offset.X + (int)win.Size.X / 2, obj.Position.Y * cellSize - offset.Y + (int)win.Size.Y / 2);
                    obj.Draw(win, pos, cellSize);
                }
            }

            win.Display();

            win.DispatchEvents();

            return win.IsOpen;
        }

        public void DrawSmells(Field<Cell> map)
        {
            float trueLeft = cameraPosition.X - win.Size.X / 2;
            float trueTop = cameraPosition.Y - win.Size.Y / 2;
            int left = (int)(trueLeft / cellSize);
            int top = (int)(trueTop / cellSize);
            int width = (int)win.Size.X / cellSize + 1;
            int height = (int)win.Size.Y / cellSize + 1;
            (int X, int Y) offset = ((int)cameraPosition.X, (int)cameraPosition.Y);

            var shape = new RectangleShape(new Vector2f(cellSize, cellSize));
            shape.FillColor = Color.Red;
            for (int i = left; i < left + width; i++)
            {
                for (int j = top; j < top + height; j++)
                {
                    if (map[i, j].Smells.ContainsKey(Simulation.SmellSystem.FOOD_ID))
                    {
                        shape.Position = new Vector2f((i - left) * cellSize, (j - top) * cellSize);
                        win.Draw(shape);
                    }
                }
            }
        }
    }
}
