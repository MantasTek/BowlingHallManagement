using BowlingHallManagement.Models;

namespace BowlingHallManagement.Services.Interfaces
{
    /// <summary>
    /// Interface for lane-related operations
    /// </summary>
    public interface ILaneService
    {
        Lane GetLaneByNumber(int laneNumber);
        List<Lane> GetAllLanes();
        List<Lane> GetAvailableLanes();
        void AddLane(Lane lane);
        void UpdateLaneStatus(int laneNumber, bool isAvailable);
    }
}