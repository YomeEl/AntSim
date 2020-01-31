﻿using AntSim.Simulation.Map;
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
        private const bool RENDER_SMELLS = false;

        public Camera Camera { get; }

        public Engine(uint width, uint height)
        {
            win = new RenderWindow(new VideoMode(width, height), "Ant simulation");
            win.SetFramerateLimit(60);

            BindEvents(win);

            cellSize = 30;

            Camera = new Camera();
        }

        private void ChangeCellSize(float amount)
        {
            float newCellSize = cellSize;
            newCellSize += amount;
            if (newCellSize < 2) newCellSize = 2;
            if (newCellSize > 100) newCellSize = 100;
            Camera.Position *= newCellSize / cellSize;
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

        private void RecalculateCameraParameters()
        {
            float speed = Keyboard.IsKeyPressed(Keyboard.Key.LShift) ? 15 : 5;

            if (Keyboard.IsKeyPressed(Keyboard.Key.W)) Camera.Position.Y -= speed;
            if (Keyboard.IsKeyPressed(Keyboard.Key.A)) Camera.Position.X -= speed;
            if (Keyboard.IsKeyPressed(Keyboard.Key.S)) Camera.Position.Y += speed;
            if (Keyboard.IsKeyPressed(Keyboard.Key.D)) Camera.Position.X += speed;

            if (Keyboard.IsKeyPressed(Keyboard.Key.Add) && cellSize < 100) ChangeCellSize(1);
            if (Keyboard.IsKeyPressed(Keyboard.Key.Subtract) && cellSize > 2) ChangeCellSize(-1);

            var currentMousePosition = Mouse.GetPosition();
            if (Mouse.IsButtonPressed(Mouse.Button.Left))
            {
                Camera.Position.X += previousMousePosition.X - currentMousePosition.X;
                Camera.Position.Y += previousMousePosition.Y - currentMousePosition.Y;
            }
            previousMousePosition = currentMousePosition;
        }

        public bool Draw(GraphicalObject[] objects)
        {
            RecalculateCameraParameters();

            win.Clear(Color.White);

            byte maxAntSize = 4;

            float trueLeft = Camera.Position.X - win.Size.X / 2;
            float trueTop = Camera.Position.Y - win.Size.Y / 2;
            int left = (int)(trueLeft / cellSize);
            int top = (int)(trueTop / cellSize);
            int width = (int)win.Size.X / cellSize + 1;
            int height = (int)win.Size.Y / cellSize + 1;
            (int X, int Y) offset = ((int)Camera.Position.X, (int)Camera.Position.Y);

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

        //Deprecated drawing
        public bool Draw(Field<Cell> field)
        {
            RecalculateCameraParameters();

            win.Clear(Color.White);

            int maxAntSize = 4;

            float trueLeft = Camera.Position.X - win.Size.X / 2;
            float trueTop = Camera.Position.Y - win.Size.Y / 2;
            int left = (int)(trueLeft / cellSize);
            int top = (int)(trueTop / cellSize);
            int width = (int)win.Size.X / cellSize + 1;
            int height = (int)win.Size.Y / cellSize + 1;
            (int X, int Y) offset = ((int)Camera.Position.X, (int)Camera.Position.Y);

            var rect = new RectangleShape(new Vector2f(cellSize, cellSize));
            for (int i = left - maxAntSize; i < left + width; i++)
            {
                for (int j = top - maxAntSize; j < top + height; j++)
                {
                    if (field[i, j].Entity != null)
                    {
                        var pos = new Vector2i(i * cellSize - offset.X + (int)win.Size.X / 2, j * cellSize - offset.Y + (int)win.Size.Y / 2);
                        field[i, j].Entity.Draw(win, pos, cellSize);
                    }

                    if (RENDER_SMELLS && field[i, j].Smells.ContainsKey(0))
                    {
                        var pos = new Vector2i(i * cellSize - offset.X + (int)win.Size.X / 2, j * cellSize - offset.Y + (int)win.Size.Y / 2);
                        float alpha = 255f * field[i, j].Smells[0] / 100f;
                        rect.FillColor = new Color(255, 0, 0, (byte)alpha);
                        rect.Position = new Vector2f(pos.X, pos.Y);
                        win.Draw(rect);
                    }
                }
            }

            win.Display();

            win.DispatchEvents();

            return win.IsOpen;
        }
    }
}
