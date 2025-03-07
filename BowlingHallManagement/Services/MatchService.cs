using BowlingHallManagement.Models;
using BowlingHallManagement.Services.Interfaces;
using BowlingHallManagement.Services.Logging;

namespace BowlingHallManagement.Services
{
    /// <summary>
    /// Service for handling match-related operations
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
            
            // Reserve the lane
            lane.Reserve(TimeSpan.FromHours(1));
            
            // Create a new match
            var match = new Match
            {
                Id = GenerateNextMatchId(),
                Player1 = player1,
                Player2 = player2,
                Lane = lane,
                Date = DateTime.Now,
                IsComplete = false,
                Duration = TimeSpan.FromHours(1)
            };
            
            _dataStorage.AddMatch(match);
            _dataStorage.SaveChanges();
            
            Notify($"Match created: {player1.Name} vs {player2.Name} on Lane {laneNumber}");
        }

        private int GenerateNextMatchId()
        {
            var matches = _dataStorage.GetAllMatches();
            return matches.Any() ? matches.Max(m => m.Id) + 1 : 1;
        }

        public void RecordMatchResults(int matchId, int player1Score, int player2Score)
        {
            var match = _dataStorage.GetMatchById(matchId);
            if (match == null || match.IsComplete)
            {
                Notify("Error: Match not found or already completed");
                throw new InvalidOperationException("Match not found or already completed");
            }

            // Update scores
            match.ScorePlayer1 = player1Score;
            match.ScorePlayer2 = player2Score;
            
            // Determine winner
            DetermineWinner(match);
            
            // Release the lane
            match.Lane.Release();
            
            _dataStorage.SaveChanges();
            
            string resultMessage = FormatMatchResult(match);
            Notify(resultMessage);
        }

        private void DetermineWinner(Match match)
        {
            if (match.ScorePlayer1 > match.ScorePlayer2)
                match.Winner = match.Player1;
            else if (match.ScorePlayer2 > match.ScorePlayer1)
                match.Winner = match.Player2;
            // If scores are equal, winner remains null (draw)
            
            match.IsComplete = true;
        }

        private string FormatMatchResult(Match match)
        {
            string resultMessage = $"Match {match.Id} results: {match.Player1.Name} ({match.ScorePlayer1}) vs {match.Player2.Name} ({match.ScorePlayer2})";
            
            if (match.Winner != null)
            {
                resultMessage += $" - Winner: {match.Winner.Name}";
            }
            else
            {
                resultMessage += " - Draw";
            }
            
            return resultMessage;
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

            // Record the simulated scores
            RecordMatchResults(matchId, scorePlayer1, scorePlayer2);
        }
    }
}