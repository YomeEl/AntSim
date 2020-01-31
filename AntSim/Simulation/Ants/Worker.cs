using AntSim.Simulation.Map;
using AntSim.Simulation.Objects;
using AntSim.Simulation.Items;

using SFML.System;

namespace AntSim.Simulation.Ants
{
    class Worker : Ant
    {
        public Worker(int id, SFML.Graphics.Texture texture, byte width, byte height) : base(id, texture, width, height)
        {
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
                return Direction.Idle;
            }

            Direction dir = Direction.Up;
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
                dir = (Direction)randomizer.Next(0, 3);
            }

            return dir;
        }

        private Direction BringFoodHome(Cell[,] vicinity)
        {
            return Direction.Up;
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
                throw new System.Exception("Wrong vicinity size!");
            }
        }
    }
}
