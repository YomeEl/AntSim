using SFML.Graphics;

namespace AntSim.Simulation.Ants
{
    static class AntsFactory
    {
        public static uint CurrentFactionId = 0;

        private static readonly Texture queenTexture = new Texture("Splines/queen.png");
        private static readonly Texture babysitterTexture = new Texture("Splines/babysitter.png");
        private static readonly Texture soldierTexture = new Texture("Splines/soldier.png");
        private static readonly Texture workerTexture = new Texture("Splines/worker.png");

        private static uint currentAntId = 0;

        public static Baby CreateBaby(AntType type)
        {
            switch (type)
            {
                case AntType.Babysitter:
                    return new Baby(AntType.Babysitter, currentAntId++, CurrentFactionId, babysitterTexture, 1, 1);

                case AntType.Soldier:
                    return new Baby(AntType.Soldier, currentAntId++, CurrentFactionId, soldierTexture, 1, 1);

                case AntType.Worker:
                    return new Baby(AntType.Worker, currentAntId++, CurrentFactionId, workerTexture, 1, 1);

                default:
                    throw new System.Exception("Unable to create baby of this type");
            }
        }

        public static Babysitter CreateBabysitter()
        {
            return new Babysitter(currentAntId++, CurrentFactionId, babysitterTexture, 2, 2);
        }

        public static Queen CreateQueen()
        {
            return new Queen(currentAntId++, CurrentFactionId, queenTexture, 4, 4);
        }

        public static Soldier CreateSoldier()
        {
            return new Soldier(currentAntId++, CurrentFactionId, soldierTexture, 2, 2);
        }

        public static Worker CreateWorker()
        {
            return new Worker(currentAntId++, CurrentFactionId, workerTexture, 2, 2);
        }
    }
}
