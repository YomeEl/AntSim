using AntSim.Simulation.Map;
using AntSim.Simulation.Objects;
using AntSim.Simulation.Items;

using SFML.System;

using System;

namespace AntSim.Simulation.Ants
{
    class Worker : Ant
    {
        private Vector2i movingDirection = new Vector2i(0, 0);

        public Worker(uint antId, uint factionId, SFML.Graphics.Texture texture, byte width, byte height) : 
            base(antId, factionId, texture, width, height)
        {
        }

        private Vector2i PickRandomVector2i()
        {
            var x = (int)(randomizer.NextDouble() * 8 - 4);
            var y = (int)(randomizer.NextDouble() * 8 - 4);

            return new Vector2i(x, y);
        }

        private Direction FindFood(Cell[,] vicinity)
        {
            bool found = false;
            Vector2i foodPos = new Vector2i(0, 0);

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (vicinity[i, j].Entity is FoodPile)
                    {
                        found = true;
                        foodPos = new Vector2i(i, j);
                    }
                }
            }
            
            if (found)
            {
                Item = new Food(10);
                var pile = (FoodPile)vicinity[foodPos.X, foodPos.Y].Entity;
                pile.Count -= 10;
                movingDirection *= -1;
                return Direction.Idle;
            }

            Direction dir = Direction.Idle;
            uint maxIntesity = 0;

            if (vicinity[1, 0].Smells.ContainsKey(0) && vicinity[1, 0].Smells[0] > maxIntesity)
            {
                dir = Direction.Up;
                maxIntesity = vicinity[1, 0].Smells[0];
            }
            if (vicinity[0, 1].Smells.ContainsKey(0) && vicinity[0, 1].Smells[0] > maxIntesity)
            {
                dir = Direction.Left;
                maxIntesity = vicinity[0, 1].Smells[0];
            }
            if (vicinity[2, 1].Smells.ContainsKey(0) && vicinity[2, 1].Smells[0] > maxIntesity)
            {
                dir = Direction.Right;
                maxIntesity = vicinity[2, 1].Smells[0];
            }
            if (vicinity[1, 2].Smells.ContainsKey(0) && vicinity[1, 2].Smells[0] > maxIntesity)
            {
                dir = Direction.Down;
                maxIntesity = vicinity[1, 2].Smells[0];
            }

            if (maxIntesity == 0)
            {
                if (movingDirection.X == 0 && movingDirection.Y == 0)
                {
                    movingDirection = PickRandomVector2i();
                }

                if (randomizer.Next(0, Math.Abs(movingDirection.X) + Math.Abs(movingDirection.Y)) < Math.Abs(movingDirection.X))
                {
                    return movingDirection.X < 0 ? Direction.Left : Direction.Right;
                }
                else
                {
                    return movingDirection.Y < 0 ? Direction.Down : Direction.Up;
                }
            }

            return dir;
        }

        private Direction BringFoodHome(Cell[,] vicinity)
        {
            Direction dir = Direction.Idle;
            uint maxIntesity = 0;

            if (vicinity[1, 0].Smells.ContainsKey(FactionId) && vicinity[1, 0].Smells[FactionId] > maxIntesity)
            {
                dir = Direction.Up;
                maxIntesity = vicinity[1, 0].Smells[FactionId];
            }
            if (vicinity[0, 1].Smells.ContainsKey(FactionId) && vicinity[0, 1].Smells[FactionId] > maxIntesity)
            {
                dir = Direction.Left;
                maxIntesity = vicinity[0, 1].Smells[FactionId];
            }
            if (vicinity[2, 1].Smells.ContainsKey(FactionId) && vicinity[2, 1].Smells[FactionId] > maxIntesity)
            {
                dir = Direction.Right;
                maxIntesity = vicinity[2, 1].Smells[FactionId];
            }
            if (vicinity[1, 2].Smells.ContainsKey(FactionId) && vicinity[1, 2].Smells[FactionId] > maxIntesity)
            {
                dir = Direction.Down;
                maxIntesity = vicinity[1, 2].Smells[FactionId];
            }

            if (maxIntesity == 0)
            {
                if (randomizer.Next(0, Math.Abs(movingDirection.X) + Math.Abs(movingDirection.Y)) < Math.Abs(movingDirection.X))
                {
                    return movingDirection.X < 0 ? Direction.Left : Direction.Right;
                }
                else
                {
                    return movingDirection.Y < 0 ? Direction.Down : Direction.Up;
                }
            }

            return dir;
        }

        public override Direction Move(Cell[,] vicinity)
        {
            if (IsVicinityCorrect(vicinity, 3))
            {
                Direction dir;
                if (Item == null)
                {
                    dir = FindFood(vicinity);
                }
                else
                {
                    dir = BringFoodHome(vicinity);
                }

                return dir;
            }
            else
            {
                return Direction.Idle;
            }
        }
    }
}
