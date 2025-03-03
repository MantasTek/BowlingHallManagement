using System;
using System.IO;

namespace BowlingHallManagement.Services.Logging
{
    /// <summary>
    /// Singleton pattern for logging
    /// </summary>
    public sealed class SingletonLogger : IObserver
    {
        // Eager initialization of the singleton instance
        private static readonly SingletonLogger _instance = new SingletonLogger();
        private readonly string _logFilePath = "bowling_hall.log";

        // Private constructor to prevent external instantiation
        private SingletonLogger()
        {
            // Add a header to the log file
            Log($"Logging started at {DateTime.Now}");
        }

        // Public accessor for the singleton instance
        public static SingletonLogger Instance => _instance;

        public void Log(string message)
        {
            string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
            
            // Write to file
            try
            {
                File.AppendAllText(_logFilePath, logEntry + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to log file: {ex.Message}");
            }
            
            // Also write to console
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"[LOG] {message}");
            Console.ResetColor();
        }

        // Implementation of IObserver.Update
        public void Update(string message)
        {
            Log(message);
        }
    }

    /// <summary>
    /// Observer interface for the Observer Pattern
    /// </summary>
    public interface IObserver
    {
        void Update(string message);
    }

    /// <summary>
    /// Subject interface for the Observer Pattern
    /// </summary>
    public interface ISubject
    {
        void Attach(IObserver observer);
        void Detach(IObserver observer);
        void Notify(string message);
    }
}