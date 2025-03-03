using BowlingHallManagement.Services;
using BowlingHallManagement.Services.Logging;
using BowlingHallManagement.UI;

namespace BowlingHallManagement
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Initialize logger
                var logger = SingletonLogger.Instance;
                logger.Log("Application starting");
                
                // Initialize data storage
                logger.Log("Loading data...");
                var dataStorage = DataStorage.Instance;
                logger.Log("Data loaded successfully");
                
                // Create and run the console UI
                logger.Log("Starting user interface");
                var ui = new ConsoleUI();
                ui.Run();
                
                logger.Log("Application shutting down normally");
            }
            catch (Exception ex)
            {
                var logger = SingletonLogger.Instance;
                logger.Log($"Error in application: {ex.Message}");
                
                Console.WriteLine($"Error initializing application: {ex.Message}");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }
    }
}