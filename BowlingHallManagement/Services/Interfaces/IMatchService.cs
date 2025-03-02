using BowlingHallManagement.Models;

namespace BowlingHallManagement.Services.Interfaces
{
    /// <summary>
    /// Interface for match-related operations
    /// </summary>
    public interface IMatchService
    {
        void CreateMatch(Member player1, Member player2, int lane);
        void RecordMatchResults(int matchId, int ScorePlayer1, int Scoreplayer2);
        void SimulateMatch(int matchId);
        List<Match> GetAllMatches();
        List<Match> GetActiveMatches();
        Match GetMatchById(int id);
    }
}
