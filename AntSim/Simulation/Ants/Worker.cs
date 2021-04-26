using System;

using AntSim.Simulation.Map;

using SFML.System;

namespace AntSim.Simulation.Ants
{
    class Worker : Ant
    {
        private const int SEARCHING_RADIUS = 10;
        private const int NEW_WP_MIN_DIST = 5;

        public Vector2f waypoint;
        private Vector2i? foodPosition;

        public Worker(uint antId, uint factionId, SFML.Graphics.Texture texture, byte width, byte height) : 
            base(antId, factionId, texture, width, height)
        {
            waypoint = Position;
        }

        public override void Step(float dt, Field<Cell> field)
        {
            Console.WriteLine(Distance(waypoint, Position));
            if (Distance(waypoint, Position) < 0.1f)
            {
                var target = FindFarSmell(field, SEARCHING_RADIUS, Map.Smells.SmellType.FromFood);
                if (!target.found)
                {
                    float sgnX = (randomizer.Next(0, 9) < 5) ? 1 : -1;
                    float sgnY = (randomizer.Next(0, 9) < 5) ? 1 : -1;
                    waypoint.X = randomizer.Next(NEW_WP_MIN_DIST, SEARCHING_RADIUS) * sgnX;
                    waypoint.Y = randomizer.Next(NEW_WP_MIN_DIST, SEARCHING_RADIUS) * sgnY;
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
            else
            {
                if (IsRotating)
                {
                    Direction = Normalize(waypoint - Position);
                }
            }

            Position += Direction * dt;
        }
    }
}
