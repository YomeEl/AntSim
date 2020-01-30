using SFML.Graphics;

namespace AntSim.Simulation.Ants
{
    static class AntsFactory
    {
        private static Texture queenTexture = new Texture("Splines/queen.png");
        private static Texture babysitterTexture = new Texture("Splines/babysitter.png");
        private static Texture soldierTexture = new Texture("Splines/soldier.png");
        private static Texture workerTexture = new Texture("Splines/worker.png");

        private static int nextAntId = 0;

        public static Baby CreateBaby(AntType type)
        {
            switch (type)
            {
                case AntType.Babysitter:
                    return new Baby(AntType.Babysitter, nextAntId++, babysitterTexture, 1, 1);

                case AntType.Soldier:
                    return new Baby(AntType.Soldier, nextAntId++, soldierTexture, 1, 1);

                case AntType.Worker:
                    return new Baby(AntType.Worker, nextAntId++, workerTexture, 1, 1);

                default:
                    throw new System.Exception("Unable to create baby of this type");
            }
        }

        public static Babysitter CreateBabysitter()
        {
            return new Babysitter(nextAntId++, babysitterTexture, 2, 2);
        }

        public static Queen CreateQueen()
        {
            return new Queen(nextAntId++, queenTexture, 4, 4);
        }

        public static Soldier CreateSoldier()
        {
            return new Soldier(nextAntId++, soldierTexture, 2, 2);
        }

        public static Worker CreateWorker()
        {
            return new Worker(nextAntId++, workerTexture, 2, 2);
        }
    }
}
