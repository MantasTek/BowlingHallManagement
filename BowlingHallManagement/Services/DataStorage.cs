using BowlingHallManagement.Factories;
using BowlingHallManagement.Models;
using BowlingHallManagement.Services.Interfaces;

namespace BowlingHallManagement.Services
{
    /// <summary>
    /// In-memory implementation of IDataStorage.
    /// </summary>
    public class DataStorage : IDataStorage
    {
        // Singleton implementation for the data storage
        private static DataStorage _instance;
        private static readonly object _lock = new object();

        public static DataStorage Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new DataStorage();
                        }
                    }
                }
                return _instance;
            }
        }

        // In-memory collections
        private List<Member> Members { get; set; }
        private List<Match> Matches { get; set; }
        private List<Lane> Lanes { get; set; }

        private DataStorage()
        {
            // Initialize empty collections
            Members = new List<Member>();
            Matches = new List<Match>();
            Lanes = new List<Lane>();
            
            // Initialize with default lanes (1-10)
            Lanes.AddRange(LaneFactory.CreateLanes(10));
        }

        // Member management methods
        public void AddMember(Member member)
        {
            Members.Add(member);
        }

        public List<Member> GetAllMembers()
        {
            return Members.ToList(); // Return a copy to prevent external modification
        }

        public Member GetMemberById(int id)
        {
            return Members.FirstOrDefault(m => m.Id == id);
        }

        // Match management methods
        public void AddMatch(Match match)
        {
            Matches.Add(match);
        }

        public List<Match> GetAllMatches()
        {
            return Matches.ToList(); // Return a copy to prevent external modification
        }

        public Match GetMatchById(int id)
        {
            return Matches.FirstOrDefault(m => m.Id == id);
        }
        
        // Lane management methods
        public void AddLane(Lane lane)
        {
            if (lane == null)
                throw new ArgumentNullException(nameof(lane));
                
            if (Lanes.Any(l => l.LaneNumber == lane.LaneNumber))
                throw new ArgumentException($"Lane {lane.LaneNumber} already exists");
                
            Lanes.Add(lane);
        }
        
        public List<Lane> GetAllLanes()
        {
            // Update availability of all lanes before returning
            foreach (var lane in Lanes)
            {
                lane.UpdateAvailability();
            }
            
            return Lanes.ToList(); // Return a copy to prevent external modification
        }
        
        public Lane GetLaneByNumber(int laneNumber)
        {
            var lane = Lanes.FirstOrDefault(l => l.LaneNumber == laneNumber);
            if (lane != null)
            {
                lane.UpdateAvailability();
            }
            return lane;
        }
        
        // This is for database implementation later
        public void SaveChanges()
        {
            // For in-memory storage, this does nothing
            // Will be implemented for database storage later
        }
    }
}