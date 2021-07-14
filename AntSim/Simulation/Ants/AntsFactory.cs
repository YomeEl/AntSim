using SFML.Graphics;
using SFML.System;

namespace AntSim.Simulation.Ants
{
    static class AntsFactory
    {
        public static uint CurrentFactionId = 0;

        private static Sprite queenSprite;
        private static Sprite workerSprite;

        private static uint currentAntId = 0;

        public static void Load()
        {
            var antSizeS = (float)Global.NumberConstants.Get("AntSizeS");
            var antSizeM = (float)Global.NumberConstants.Get("AntSizeM");
            var antSizeL = (float)Global.NumberConstants.Get("AntSizeL");

            Texture queenTexture = new Texture("Sprites/queen.png");
            Texture workerTexture = new Texture("Sprites/worker.png");

            queenSprite = new Sprite(queenTexture)
            {
                Scale = new Vector2f(
                            antSizeL / queenTexture.Size.X,
                            antSizeL / queenTexture.Size.Y
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
            MoveOrigin(workerSprite);
        }

        public static Queen CreateQueen()
        {
            var queen = new Queen(currentAntId++, CurrentFactionId, queenSprite);
            queen.IsStatic = true;
            return queen;
        }

        public static Worker CreateWorker()
        {
            return new Worker(currentAntId++, CurrentFactionId, workerSprite);
        }

        public static void RescaleSprites(float factor)
        {
            queenSprite.Scale *= factor;
            workerSprite.Scale *= factor;
        }

        private static void MoveOrigin(Sprite sprite)
        {
            var localBounds = sprite.GetLocalBounds();
            sprite.Origin += new Vector2f(localBounds.Height, localBounds.Width) / 2;
        }
    }
}
