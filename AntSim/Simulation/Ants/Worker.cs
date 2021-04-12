using AntSim.Simulation.Map;

using SFML.System;

namespace AntSim.Simulation.Ants
{
    class Worker : Ant
    {
        private const int SEARCHING_RADIUS = 10;

        public Worker(uint antId, uint factionId, SFML.Graphics.Texture texture, byte width, byte height) : 
            base(antId, factionId, texture, width, height)
        {
        }

        public override void Step(float dt, Field<Cell> field)
        {
            var target = FindFarSmell(field, SEARCHING_RADIUS, Map.Smells.SmellType.FromFood);
            float mult = 1f;
            if (!target.found)
            {
                target.x = randomizer.Next(-100, 100);
                target.y = randomizer.Next(-100, 100);
                mult = 0.0001f;
            }

            var newDir = Direction + mult * new Vector2f(target.x, target.y);
            newDir = Normalize(newDir) * dt;

            Position += newDir;
            Direction = newDir;
        }
    }
}
