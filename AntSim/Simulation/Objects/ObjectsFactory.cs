using SFML.Graphics;

namespace AntSim.Simulation.Objects
{
    static class ObjectsFactory
    {
        private static Texture foodPileTexture = new Texture(new Image(10, 10, new Color(255, 128, 0)));

        public static FoodPile CreateFoodPile()
        {
            return new FoodPile(1000, foodPileTexture, 1, 1);
        }
    }
}
