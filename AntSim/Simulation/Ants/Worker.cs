using AntSim.Simulation.Map;
using AntSim.Simulation.Map.Smells;

using SFML.System;

namespace AntSim.Simulation.Ants
{
    class Worker : Ant
    {
        private Vector2f waypoint;
        private Vector2i target;

        private readonly int SEARCHING_RADIUS;
        private readonly int NEW_WP_MIN_DIST;
        private readonly int NEW_WP_MAX_DIST;

        private bool foundFood = false;
        private bool foundHome = false;

        private bool hasFood = false;

        public Worker(uint antId, uint factionId, SFML.Graphics.Sprite sprite) : 
            base(antId, factionId, sprite)
        {
            waypoint = Position;

            speed = Global.NumberConstants.Get("DefaultWorkerSpeed");
            SEARCHING_RADIUS = (int)Global.NumberConstants.Get("WorkerSearchingRadius");
            NEW_WP_MIN_DIST = (int)Global.NumberConstants.Get("WorkerNewWPMinDist");
            NEW_WP_MAX_DIST = (int)Global.NumberConstants.Get("WorkerNewWPMaxDist");
        }

        public override void Step(float dt, Field<Cell> field)
        {
            bool newWp = false;
            bool updateWp = false;
            var dist = Distance(waypoint, Position);

            if (dist < 1f || dist > SEARCHING_RADIUS)
            {
                if (hasFood)
                {
                    if (foundHome)
                    {
                        if (dist > SEARCHING_RADIUS)
                        {
                            //missed
                            updateWp = true;
                            foundHome = false;
                        }
                        else
                        {
                            //we are home
                            hasFood = false;
                            foundHome = false;
                            updateWp = true;
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
                            target = smellsAround.close[0].Value;
                            waypoint = new Vector2f(target.X, target.Y);
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
                                updateWp = true;
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
                            updateWp = true;
                            foundFood = false;
                        }
                        else
                        {
                            //we are on food
                            foundFood = false;
                            var food = (Objects.FoodPile)field[target.X, target.Y].Item;
                            if (food != null)
                            {
                                field[target.X, target.Y].Item = null;
                                food.ShouldBeDestroyed = true;
                                field[target.X, target.Y].RemoveSmell(SmellType.Food);
                                hasFood = true;
                                updateWp = true;
                            }
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
                            target = smellsAround.close[0].Value;
                            waypoint = new Vector2f(target.X, target.Y);
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
                                updateWp = true;
                            }
                        }
                    }
                }

                if (updateWp && !hasFood)
                {
                    //Generate random waypoint
                    int sgnX = (randomizer.Next(0, 10) < 5) ? 1 : -1;
                    int sgnY = (randomizer.Next(0, 10) < 5) ? 1 : -1;
                    waypoint.X = randomizer.Next(NEW_WP_MIN_DIST, NEW_WP_MAX_DIST + 1) * sgnX;
                    waypoint.Y = randomizer.Next(NEW_WP_MIN_DIST, NEW_WP_MAX_DIST + 1) * sgnY;
                    waypoint += Position;
                }

                newWp = true;
            }

            if (IsRotating || newWp)
            {
                Direction = Normalize(waypoint - Position);
            }
            Position += Direction * dt * speed;

            SmellType type = hasFood ? SmellType.FromFood : SmellType.FromHome;
            LeaveSmell(field, type);
        }
    }
}