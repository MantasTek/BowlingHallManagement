using BowlingHallManagement.Factories;
using BowlingHallManagement.Models;
using BowlingHallManagement.Services.Interfaces;
using System.Text.Json;

namespace BowlingHallManagement.Services
{
    /// <summary>
    /// JSON file-based implementation of IDataStorage.
    /// </summary>
    public class DataStorage : IDataStorage
    {
        // Singleton implementation for the data storage
        private static DataStorage _instance;
        private static readonly object _lock = new object();

        // File paths for JSON storage
        private readonly string _membersFilePath = "members.json";
        private readonly string _matchesFilePath = "matches.json";
        private readonly string _lanesFilePath = "lanes.json";

        // In-memory collections
        private List<Member> Members { get; set; }
        private List<Match> Matches { get; set; }
        private List<Lane> Lanes { get; set; }

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

        private DataStorage()
        {
            // Initialize collections
            LoadDataFromFiles();
            
            // Initialize with default lanes if none exist
            if (Lanes.Count == 0)
            {
                Lanes.AddRange(LaneFactory.CreateLanes(10));
                SaveChanges();
            }
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
        
        // Save data to JSON files
        public void SaveChanges()
        {
            try
            {
                // Save members
                var membersJson = JsonSerializer.Serialize(Members, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_membersFilePath, membersJson);
                
                // Save lanes
                var lanesJson = JsonSerializer.Serialize(Lanes, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_lanesFilePath, lanesJson);
                
                // Save matches
                var matchDtos = Matches.Select(m => new
                {
                    m.Id,
                    Player1Id = m.Player1?.Id,
                    Player2Id = m.Player2?.Id,
                    m.Date,
                    LaneNumber = m.Lane?.LaneNumber,
                    m.ScorePlayer1,
                    m.ScorePlayer2,
                    WinnerId = m.Winner?.Id,
                    m.IsComplete,
                    Duration = m.Duration.ToString()
                }).ToList();
                
                var matchesJson = JsonSerializer.Serialize(matchDtos, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_matchesFilePath, matchesJson);
                
                Console.WriteLine("Data saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving data: {ex.Message}");
            }
        }
        
        // Load data from JSON files
        private void LoadDataFromFiles()
        {
            try
            {
                // Load members
                if (File.Exists(_membersFilePath))
                {
                    var membersJson = File.ReadAllText(_membersFilePath);
                    Members = JsonSerializer.Deserialize<List<Member>>(membersJson) ?? new List<Member>();
                }
                else
                {
                    Members = new List<Member>();
                }
                
                // Load lanes
                if (File.Exists(_lanesFilePath))
                {
                    var lanesJson = File.ReadAllText(_lanesFilePath);
                    Lanes = JsonSerializer.Deserialize<List<Lane>>(lanesJson) ?? new List<Lane>();
                }
                else
                {
                    Lanes = new List<Lane>();
                }
                
                // Load matches
                if (File.Exists(_matchesFilePath))
                {
                    var matchesJson = File.ReadAllText(_matchesFilePath);
                    var matchDtos = JsonSerializer.Deserialize<List<MatchDto>>(matchesJson) ?? new List<MatchDto>();
            
                    Matches = new List<Match>();
                    foreach (var dto in matchDtos)
                    {
                        var player1 = GetMemberById(dto.Player1Id ?? 0);
                        var player2 = GetMemberById(dto.Player2Id ?? 0);
                        var lane = GetLaneByNumber(dto.LaneNumber ?? 0);
                
                        if (player1 != null && player2 != null && lane != null)
                        {
                            // Create a new match with the constructor
                            var match = new Match(dto.Id, player1, player2, lane)
                            {
                                Date = dto.Date
                            };
                    
                            // Set duration
                            if (TimeSpan.TryParse(dto.Duration, out TimeSpan duration))
                            {
                                match.Duration = duration;
                            }
                    
                            // If the match is complete, record the scores which will determine the winner properly
                            if (dto.IsComplete)
                            {      
                                // This will set Winner and IsComplete internally via DetermineWinner
                                match.RecordScores(dto.ScorePlayer1, dto.ScorePlayer2);
                            }
                    
                            Matches.Add(match);
                        }
                    }
                }
            
                else
                {
                    Matches = new List<Match>();
                }
        
                Console.WriteLine("Data loaded successfully.");
            }
                
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading data: {ex.Message}");
        
                // Initialize with empty collections if loading fails
                Members = new List<Member>();
                Matches = new List<Match>();
                Lanes = new List<Lane>();
            }
        }
    
        
        // Helper class for serializing/deserializing matches
        private class MatchDto
        {
            public int Id { get; set; }
            public int? Player1Id { get; set; }
            public int? Player2Id { get; set; }
            public DateTime Date { get; set; }
            public int? LaneNumber { get; set; }
            public int ScorePlayer1 { get; set; }
            public int ScorePlayer2 { get; set; }
            public int? WinnerId { get; set; }
            public bool IsComplete { get; set; }
            public string Duration { get; set; }
        }
    }
}