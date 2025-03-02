using System;
using System.Collections.Generic;
using System.Linq;
using BowlingHallManagement.Factories;
using BowlingHallManagement.Models;
using BowlingHallManagement.Services.Interfaces;

namespace BowlingHallManagement.Services
{
    /// <summary>
    /// Service for handling match-related operations
    /// </summary>
    public class MatchService : IMatchService
    {
        private readonly IDataStorage _dataStorage;
        private readonly Random _random;

        public MatchService(IDataStorage dataStorage)
        {
            _dataStorage = dataStorage ?? throw new ArgumentNullException(nameof(dataStorage));
            _random = new Random();
        }

        public void CreateMatch(Member player1, Member player2, int laneNumber)
        {
            var lane = _dataStorage.GetLaneByNumber(laneNumber);
            if (lane == null)
            {
                throw new ArgumentException($"Lane {laneNumber} does not exist");
            }
            
            if (!lane.IsAvailable)
            {
                throw new InvalidOperationException($"Lane {laneNumber} is not available");
            }
            
            var match = MatchFactory.CreateMatch(player1, player2, lane);
            _dataStorage.AddMatch(match);
            _dataStorage.SaveChanges();
        }

        public void RecordMatchResults(int matchId, int player1Score, int player2Score)
        {
            var match = _dataStorage.GetMatchById(matchId);
            if (match == null || match.IsComplete)
            {
                throw new InvalidOperationException("Match not found or already completed");
            }

            match.RecordScores(player1Score, player2Score);
            _dataStorage.SaveChanges();
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
                throw new InvalidOperationException("Match not found or already completed");
            }

            // Generate random scores between 0 and 300 (perfect game in bowling)
            int ScorePlayer1 = _random.Next(0, 301);
            int ScorePlayer2 = _random.Next(0, 301);

            match.RecordScores(ScorePlayer1, ScorePlayer2);
            _dataStorage.SaveChanges();
        }
    }
}