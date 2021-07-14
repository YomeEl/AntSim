using SFML.Graphics;
using SFML.System;

namespace AntSim.Simulation.Objects
{
    static class ObjectsFactory
    {
        private static Sprite foodPileSprite;

        public static void Load()
        {
            var foodPileSize = (float)Global.NumberConstants.Get("FoodPileSize");

            Texture foodPileTexture = new Texture(new Image(10, 10, new Color(255, 128, 0)));
            foodPileSprite = new Sprite(foodPileTexture)
            {
                Scale = new Vector2f(
                            foodPileSize / foodPileTexture.Size.X,
                            foodPileSize / foodPileTexture.Size.Y
                        )
            };

            MoveOrigin(foodPileSprite);
        }
        public static FoodPile CreateFoodPile()
        {
            {
                return new FoodPile((uint)Global.NumberConstants.Get("FoodCount"), foodPileSprite);
            }
        }

        public static void RescaleSprites(float factor)
        {
            foodPileSprite.Scale *= factor;
        }

        private static void MoveOrigin(Sprite sprite)
        {
            var localBounds = sprite.GetLocalBounds();
            sprite.Origin += new Vector2f(localBounds.Height, localBounds.Width) / 2;
        }
    }
}
