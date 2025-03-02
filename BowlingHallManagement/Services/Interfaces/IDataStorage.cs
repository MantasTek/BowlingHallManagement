using BowlingHallManagement.Models;

namespace BowlingHallManagement.Services.Interfaces
{
    /// <summary>
    /// Interface for data storage operations
    /// </summary>
    public interface IDataStorage
    {
        // Member operations
        void AddMember(Member member);
        List<Member> GetAllMembers();
        Member GetMemberById(int id);

        // Match operations
        void AddMatch(Match match);
        List<Match> GetAllMatches();
        Match GetMatchById(int id);

        // Lane operations
        void AddLane(Lane lane);
        List<Lane> GetAllLanes();
        Lane GetLaneByNumber(int laneNumber);
        
        void SaveChanges();
    }
}
