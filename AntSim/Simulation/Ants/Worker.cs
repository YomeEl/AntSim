using AntSim.Simulation.Map;

using SFML.System;

namespace AntSim.Simulation.Ants
{
    class Worker : Ant
    {
        private const int SEARCHING_RADIUS = 20;

        public Worker(uint antId, uint factionId, SFML.Graphics.Texture texture, byte width, byte height) : 
            base(antId, factionId, texture, width, height)
        {
        }

        public override void Step(float dt, Field<Cell> field)
        {
            var currentDir = GetDirection();
            var target = FindFarSmell(field, SEARCHING_RADIUS, Map.Smells.SmellType.FromFood);
            if (!target.found)
            {
                target.x = randomizer.Next();
                target.y = randomizer.Next();
            }

            var newDir = currentDir + new Vector2f(target.x, target.y);
            newDir = Normalize(newDir) * dt;

            Position += newDir;
        }
    }
}
