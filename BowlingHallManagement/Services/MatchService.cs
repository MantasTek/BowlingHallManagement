using System;
using System.Collections.Generic;
using System.Linq;
using BowlingHallManagement.Factories;
using BowlingHallManagement.Models;
using BowlingHallManagement.Services.Interfaces;
using BowlingHallManagement.Services.Logging;

namespace BowlingHallManagement.Services
{
    /// <summary>
    /// Service for handling match-related operations
    /// Implements ISubject for the Observer Pattern
    /// </summary>
    public class MatchService : IMatchService, ISubject
    {
        private readonly IDataStorage _dataStorage;
        private readonly Random _random;
        private readonly List<IObserver> _observers = new List<IObserver>();

        public MatchService(IDataStorage dataStorage)
        {
            _dataStorage = dataStorage ?? throw new ArgumentNullException(nameof(dataStorage));
            _random = new Random();
            
            // Attach logger by default
            Attach(SingletonLogger.Instance);
        }

        #region Observer Pattern Implementation
        
        public void Attach(IObserver observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }
        }

        public void Detach(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public void Notify(string message)
        {
            foreach (var observer in _observers)
            {
                observer.Update(message);
            }
        }
        
        #endregion

        public void CreateMatch(Member player1, Member player2, int laneNumber)
        {
            var lane = _dataStorage.GetLaneByNumber(laneNumber);
            if (lane == null)
            {
                Notify($"Error: Lane {laneNumber} does not exist");
                throw new ArgumentException($"Lane {laneNumber} does not exist");
            }
            
            if (!lane.IsAvailable)
            {
                Notify($"Error: Lane {laneNumber} is not available");
                throw new InvalidOperationException($"Lane {laneNumber} is not available");
            }
            
            var match = MatchFactory.CreateMatch(player1, player2, lane);
            _dataStorage.AddMatch(match);
            _dataStorage.SaveChanges();
            
            Notify($"Match created: {player1.Name} vs {player2.Name} on Lane {laneNumber}");
        }

        public void RecordMatchResults(int matchId, int player1Score, int player2Score)
        {
            var match = _dataStorage.GetMatchById(matchId);
            if (match == null || match.IsComplete)
            {
                Notify("Error: Match not found or already completed");
                throw new InvalidOperationException("Match not found or already completed");
            }

            match.RecordScores(player1Score, player2Score);
            _dataStorage.SaveChanges();
            
            string resultMessage = $"Match {matchId} results: {match.Player1.Name} ({player1Score}) vs {match.Player2.Name} ({player2Score})";
            if (match.Winner != null)
            {
                resultMessage += $" - Winner: {match.Winner.Name}";
            }
            else
            {
                resultMessage += " - Draw";
            }
            
            Notify(resultMessage);
        }

        public List<Match> GetAllMatches()
        {
            return _dataStorage.GetAllMatches();
        }

        public List<Match> GetActiveMatches()
        {
            return _dataStorage.GetAllMatches().Where(m => !m.IsComplete).ToList();
        }

        public Match GetMatchById(int id)
        {
            return _dataStorage.GetMatchById(id);
        }

        public void SimulateMatch(int matchId)
        {
            var match = _dataStorage.GetMatchById(matchId);
            if (match == null || match.IsComplete)
            {
                Notify("Error: Match not found or already completed");
                throw new InvalidOperationException("Match not found or already completed");
            }

            Notify($"Simulating match {matchId}: {match.Player1.Name} vs {match.Player2.Name}");
            
            // Generate random scores between 0 and 300 (perfect game in bowling)
            int scorePlayer1 = _random.Next(0, 301);
            int scorePlayer2 = _random.Next(0, 301);

            match.RecordScores(scorePlayer1, scorePlayer2);
            _dataStorage.SaveChanges();
            
            string resultMessage = $"Match {matchId} simulation complete: {match.Player1.Name} ({scorePlayer1}) vs {match.Player2.Name} ({scorePlayer2})";
            if (match.Winner != null)
            {
                resultMessage += $" - Winner: {match.Winner.Name}";
            }
            else
            {
                resultMessage += " - Draw";
            }
            
            Notify(resultMessage);
        }
    }
}