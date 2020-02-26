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

        private Direction FindMaxIntensityDirection(Cell[,] vicinity, uint smellId)
        {
            Direction dir = Direction.Idle;
            uint maxIntesity = 0;

            if (vicinity[1, 0].Smells.ContainsKey(smellId) && vicinity[1, 0].Smells[smellId] > maxIntesity)
            {
                dir = Direction.Up;
                maxIntesity = vicinity[1, 0].Smells[smellId];
            }
            if (vicinity[0, 1].Smells.ContainsKey(smellId) && vicinity[0, 1].Smells[smellId] > maxIntesity)
            {
                dir = Direction.Left;
                maxIntesity = vicinity[0, 1].Smells[smellId];
            }
            if (vicinity[2, 1].Smells.ContainsKey(smellId) && vicinity[2, 1].Smells[smellId] > maxIntesity)
            {
                dir = Direction.Right;
                maxIntesity = vicinity[2, 1].Smells[smellId];
            }
            if (vicinity[1, 2].Smells.ContainsKey(smellId) && vicinity[1, 2].Smells[smellId] > maxIntesity)
            {
                dir = Direction.Down;
                maxIntesity = vicinity[1, 2].Smells[smellId];
            }

            return maxIntesity == 0 ? Direction.Idle : dir;
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

            Direction dir = FindMaxIntensityDirection(vicinity, SmellSystem.FOOD_ID);

            if (dir == Direction.Idle)
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
            bool found = false;
            Vector2i queenPos = new Vector2i(0, 0);

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (vicinity[i, j].Entity is Queen)
                    {
                        found = true;
                        queenPos = new Vector2i(i, j);
                    }
                }
            }

            if (found)
            {
                var queen = (Queen)vicinity[queenPos.X, queenPos.Y].Entity;
                queen.FoodStock += Item.Count;
                Item = null;
                movingDirection = new Vector2i(0, 0);
                return Direction.Idle;
            }

            Direction dir = FindMaxIntensityDirection(vicinity, FactionId + SmellSystem.FACTION_ID_OFFSET);

            if (dir == Direction.Idle)
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
