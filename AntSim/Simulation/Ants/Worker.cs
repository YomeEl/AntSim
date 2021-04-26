using System;

using AntSim.Simulation.Map;

using SFML.System;

namespace AntSim.Simulation.Ants
{
    class Worker : Ant
    {
        private const int SEARCHING_RADIUS = 10;
        private const float LEG_LENGTH = 1;

        private Vector2f waypoint;
        private Vector2i? foodPosition;

        public Worker(uint antId, uint factionId, SFML.Graphics.Texture texture, byte width, byte height) : 
            base(antId, factionId, texture, width, height)
        {
            waypoint = Position;
        }

        public override void Step(float dt, Field<Cell> field)
        {
            if (Math.Abs(waypoint.X - Position.X) < 0.1f &&
                Math.Abs(waypoint.Y - Position.Y) < 0.1f)
            {
                var target = FindFarSmell(field, SEARCHING_RADIUS, Map.Smells.SmellType.FromFood);
                if (!target.found)
                {
                    waypoint.X = randomizer.Next(-SEARCHING_RADIUS, SEARCHING_RADIUS);
                    waypoint.Y = randomizer.Next(-SEARCHING_RADIUS, SEARCHING_RADIUS);
                    foodPosition = null;
                }
                else
                {
                    waypoint.X = target.position.X;
                    waypoint.Y = target.position.Y;
                    foodPosition = target.position;
                }
                waypoint += Position;

                Direction = Normalize(waypoint - Position);
            }

            Position += Direction * dt;
        }
    }
}
