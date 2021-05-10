using AntSim.Simulation.Map;
using AntSim.Simulation.Map.Smells;

using SFML.System;

namespace AntSim.Simulation.Ants
{
    class Worker : Ant
    {
        private Vector2f waypoint;
        private Vector2i target;

        private const int SEARCHING_RADIUS = 25;
        private const int NEW_WP_MIN_DIST = 5;
        private const int NEW_WP_MAX_DIST = 10;

        private bool foundFood = false;
        private bool foundHome = false;

        private bool hasFood = false;

        public Worker(uint antId, uint factionId, SFML.Graphics.Texture texture, byte width, byte height) : 
            base(antId, factionId, texture, width, height)
        {
            waypoint = Position;
            speed = 1f;
        }

        public override void Step(float dt, Field<Cell> field)
        {
            bool newWp = false;
            bool fail = false;
            var dist = Distance(waypoint, Position);

            if (dist < 0.1f || dist > SEARCHING_RADIUS)
            {
                if (hasFood)
                {
                    if (foundHome)
                    {
                        if (dist > SEARCHING_RADIUS)
                        {
                            //missed
                            fail = true;
                            foundHome = false;
                        }
                        else
                        {
                            //we are home
                            hasFood = false;
                            foundHome = false;
                        }
                    }
                    else
                    {
                        //find home then new home trace then random
                        var types = new SmellType[] { SmellType.Home, SmellType.FromHome };
                        var smellsAround = FindSmells(field, SEARCHING_RADIUS, types);
                        if (smellsAround.close[0] != null)
                        {
                            //found home
                            waypoint = new Vector2f(smellsAround.close[0].Value.X, smellsAround.close[0].Value.Y);
                            foundHome = true;
                        }
                        else
                        {
                            if (smellsAround.weak[1] != null)
                            {
                                //found home trace
                                waypoint = new Vector2f(smellsAround.weak[1].Value.X, smellsAround.weak[1].Value.Y);
                            }
                            else
                            {
                                fail = true;
                            }
                        }
                    }
                }
                else
                {
                    if (foundFood)
                    {
                        if (dist > SEARCHING_RADIUS)
                        {
                            //missed
                            fail = true;
                            foundFood = false;
                        }
                        else
                        {
                            //we are on food
                            hasFood = true;
                            foundFood = false;
                            var food = (Objects.FoodPile)field[target.X, target.Y].Item;
                            field[target.X, target.Y].Item = null;
                            food.ShouldBeDestroyed = true;
                        }
                    }
                    else
                    {
                        //find food then new trace then random
                        var types = new SmellType[] { SmellType.Food, SmellType.FromFood };
                        var smellsAround = FindSmells(field, SEARCHING_RADIUS, types);
                        if (smellsAround.close[0] != null)
                        {
                            //found food
                            waypoint = new Vector2f(smellsAround.close[0].Value.X, smellsAround.close[0].Value.Y);
                            foundFood = true;
                        }
                        else
                        {
                            if (smellsAround.weak[1] != null)
                            {
                                //found food trace
                                waypoint = new Vector2f(smellsAround.weak[1].Value.X, smellsAround.weak[1].Value.Y);
                            }
                            else
                            {
                                fail = true;
                            }
                        }
                    }
                }

                if (fail)
                {
                    //Generate random waypoint
                    int sgnX = (randomizer.Next(0, 10) < 5) ? 1 : -1;
                    int sgnY = (randomizer.Next(0, 10) < 5) ? 1 : -1;
                    waypoint.X = randomizer.Next(NEW_WP_MIN_DIST, NEW_WP_MAX_DIST + 1) * sgnX;
                    waypoint.Y = randomizer.Next(NEW_WP_MIN_DIST, NEW_WP_MAX_DIST + 1) * sgnY;
                    waypoint += Position;

                    newWp = true;
                }

                newWp = true;
            }

            if (IsRotating || newWp)
            {
                Direction = Normalize(waypoint - Position);
            }
            Position += Direction * dt * speed;
        }
    }
}
