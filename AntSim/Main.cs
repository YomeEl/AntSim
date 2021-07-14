using AntSim.Simulation;

namespace AntSim
{
    class Program
    {
        static void Main(string[] args)
        {
            //Load everything
            Simulation.Global.NumberConstants.Load();

            //Run simulation
            Logic logic = new Logic();
            logic.RunSimulation();
        }
    }
}
