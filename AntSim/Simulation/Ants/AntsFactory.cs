using SFML.Graphics;
using SFML.System;

namespace AntSim.Simulation.Ants
{
    static class AntsFactory
    {
        public static uint CurrentFactionId = 0;

        private static Sprite queenSprite;
        private static Sprite babysitterSprite;
        private static Sprite soldierSprite;
        private static Sprite workerSprite;

        private static uint currentAntId = 0;

        public static void Load()
        {
            var antSizeS = (float)Global.NumberConstants.Get("AntSizeS");
            var antSizeM = (float)Global.NumberConstants.Get("AntSizeM");
            var antSizeL = (float)Global.NumberConstants.Get("AntSizeL");

            Texture queenTexture = new Texture("Sprites/queen.png");
            Texture babysitterTexture = new Texture("Sprites/babysitter.png");
            Texture soldierTexture = new Texture("Sprites/soldier.png");
            Texture workerTexture = new Texture("Sprites/worker.png");

            queenSprite = new Sprite(queenTexture)
            {
                Scale = new Vector2f(
                            antSizeL / queenTexture.Size.X,
                            antSizeL / queenTexture.Size.Y
                        )
            };
            babysitterSprite = new Sprite(babysitterTexture)
            {
                Scale = new Vector2f(
                            antSizeM / babysitterTexture.Size.X,
                            antSizeM / babysitterTexture.Size.Y
                        )
            };
            soldierSprite = new Sprite(soldierTexture)
            {
                Scale = new Vector2f(
                            antSizeM / soldierTexture.Size.X,
                            antSizeM / soldierTexture.Size.Y
                        )
            };
            workerSprite = new Sprite(workerTexture)
            {
                Scale = new Vector2f(
                            antSizeM / workerTexture.Size.X,
                            antSizeM / workerTexture.Size.Y
                        )
            };

            MoveOrigin(queenSprite);
            MoveOrigin(babysitterSprite);
            MoveOrigin(soldierSprite);
            MoveOrigin(workerSprite);
        }

        public static Baby CreateBaby(AntType type)
        {
            switch (type)
            {
                case AntType.Babysitter:
                    return new Baby(AntType.Babysitter, currentAntId++, CurrentFactionId, babysitterSprite);

                case AntType.Soldier:
                    return new Baby(AntType.Soldier, currentAntId++, CurrentFactionId, soldierSprite);

                case AntType.Worker:
                    return new Baby(AntType.Worker, currentAntId++, CurrentFactionId, workerSprite);

                default:
                    throw new System.Exception("Unable to create baby of this type");
            }
        }

        public static Babysitter CreateBabysitter()
        {
            return new Babysitter(currentAntId++, CurrentFactionId, babysitterSprite);
        }

        public static Queen CreateQueen()
        {
            var queen = new Queen(currentAntId++, CurrentFactionId, queenSprite);
            queen.IsStatic = true;
            return queen;
        }

        public static Soldier CreateSoldier()
        {
            return new Soldier(currentAntId++, CurrentFactionId, soldierSprite);
        }

        public static Worker CreateWorker()
        {
            return new Worker(currentAntId++, CurrentFactionId, workerSprite);
        }

        public static void RescaleSprites(float factor)
        {
            queenSprite.Scale *= factor;
            babysitterSprite.Scale *= factor;
            soldierSprite.Scale *= factor;
            workerSprite.Scale *= factor;
        }

        private static void MoveOrigin(Sprite sprite)
        {
            var localBounds = sprite.GetLocalBounds();
            sprite.Origin += new Vector2f(localBounds.Height, localBounds.Width) / 2;
        }
    }
}
