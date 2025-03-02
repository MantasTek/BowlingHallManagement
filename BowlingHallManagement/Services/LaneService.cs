using BowlingHallManagement.Factories;
using BowlingHallManagement.Models;
using BowlingHallManagement.Services.Interfaces;

namespace BowlingHallManagement.Services
{
    /// <summary>
    /// Service for handling lane-related operations
    /// </summary>
    public class LaneService : ILaneService
    {
        private readonly List<Lane> _lanes = new List<Lane>();
        
        public LaneService()
        {
            // Initialize with some default lanes (1-10)
            _lanes.AddRange(LaneFactory.CreateLanes(10));
        }

        public Lane GetLaneByNumber(int laneNumber)
        {
            return _lanes.FirstOrDefault(l => l.LaneNumber == laneNumber);
        }

        public List<Lane> GetAllLanes()
        {
            // Update availability of all lanes before returning
            foreach (var lane in _lanes)
            {
                lane.UpdateAvailability();
            }
            
            return _lanes.ToList();
        }

        public List<Lane> GetAvailableLanes()
        {
            // Update availability of all lanes before checking
            foreach (var lane in _lanes)
            {
                lane.UpdateAvailability();
            }
            
            return _lanes.Where(l => l.IsAvailable).ToList();
        }

        public void AddLane(Lane lane)
        {
            if (lane == null)
                throw new ArgumentNullException(nameof(lane));
                
            if (_lanes.Any(l => l.LaneNumber == lane.LaneNumber))
                throw new ArgumentException($"Lane {lane.LaneNumber} already exists");
                
            _lanes.Add(lane);
        }

        public void UpdateLaneStatus(int laneNumber, bool isAvailable)
        {
            var lane = GetLaneByNumber(laneNumber);
            if (lane == null)
                throw new ArgumentException($"Lane {laneNumber} does not exist");
                
            if (isAvailable)
                lane.Release();
            else
                lane.Reserve(TimeSpan.FromHours(1)); // Default reservation time
        }
    }
}