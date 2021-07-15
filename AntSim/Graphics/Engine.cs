using System.Collections.Generic;
using System;

using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace AntSim.Graphics
{
    class Engine
    {
        public bool Active { get; private set; }

        private readonly RenderWindow win;
        private byte cellSize;
        private byte oldCellSize;
        private Vector2i previousMousePosition;
        private SortedSet<GraphicalObject> objects;

        private List<GraphicalObject> garbage;

        private Vector2f cameraPosition;

        public Engine(uint width, uint height)
        {
            Active = true;

            objects = new SortedSet<GraphicalObject>();

            win = new RenderWindow(new VideoMode(width, height), "Ant simulation");
            win.SetFramerateLimit(60);

            BindEvents(win);

            cellSize = (byte)Simulation.Global.NumberConstants.Get("CellSize");
            oldCellSize = 1;

            garbage = new List<GraphicalObject>();
        }

        public void Register(GraphicalObject obj)
        {
            objects.Add(obj);
        }

        public void Unregister(GraphicalObject obj)
        {
            objects.Remove(obj);
        }

        public void Draw()
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

            if (cellSize != oldCellSize)
            {
                var factor = (float)cellSize / oldCellSize;
                Simulation.Ants.AntsFactory.RescaleSprites(factor);
                Simulation.Objects.ObjectsFactory.RescaleSprites(factor);
                oldCellSize = cellSize;
            }

            foreach (GraphicalObject obj in objects)
            {
                if (obj.ShouldBeDestroyed)
                {
                    garbage.Add(obj);
                    continue;
                }

                if (!obj.IsStatic)
                {
                    obj.UpdateDirection();
                }

                if (obj.Position.X >= left - maxAntSize && obj.Position.X <= left + width &&
                    obj.Position.Y >= top - maxAntSize && obj.Position.Y <= top + height)
                {
                    var pos = new Vector2f
                    (
                        obj.Position.X * cellSize - offset.X + (int)win.Size.X / 2,
                        obj.Position.Y * cellSize - offset.Y + (int)win.Size.Y / 2
                    );
                    obj.Sprite.Position = pos;

                    if (!obj.IsStatic)
                    {
                        double rad = Math.Atan2(obj.Direction.X, -obj.Direction.Y);
                        float rotation = (float)(rad / Math.PI / 2 * 360f);
                        if (rotation < 0)
                        {
                            rotation = 360 + rotation;
                        }
                        obj.Sprite.Rotation = rotation;
                    }

                    win.Draw(obj.Sprite);
                }
            }

            foreach (GraphicalObject obj in garbage)
            {
                Unregister(obj);
            }
            garbage.Clear();

            win.Display();

            win.DispatchEvents();
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

        private void Win_Closed(object sender, EventArgs e)
        {
            Active = false;

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
    }
}
