namespace BowlingHallManagement.Models
{
    public class Match
    {
        public int Id { get; set; }
        public Member Player1 { get; set; }
        public Member Player2 { get; set; }
        public DateTime Date { get; set; }
        public Lane Lane { get; set; }
        public int ScorePlayer1 { get; set; }
        public int ScorePlayer2 { get; set; }
        public Member? Winner { get; set; }
        public bool IsComplete { get; set; }
        public TimeSpan Duration { get; set; } = TimeSpan.FromHours(1);
        
        // Default constructor for JSON serialization
        public Match()
        {
            Date = DateTime.Now;
            IsComplete = false;
        }
        
        public Match(int id, Member player1, Member player2, Lane lane)
        {
            Id = id;
            Player1 = player1;
            Player2 = player2;
            Lane = lane;
            Date = DateTime.Now;
            IsComplete = false;
        }

        public override string ToString()
        {
            string status = IsComplete ? "Complete" : "In Progress";
            string result = IsComplete ?
                $"Result: {Player1.Name}: {ScorePlayer1} vs {Player2.Name}: {ScorePlayer2}":
                "No result yet";
            string winnerInfo = "";
            if (IsComplete)
            {
                winnerInfo = Winner != null
                    ? $"Winner: {Winner.Name}"
                    : "Match ended in a draw";
            }
            return $"Match #{Id} | Lane: {Lane.LaneNumber} | Date: {Date.ToShortDateString()} | Status: {status}\n"
                + $"{result}\n{winnerInfo}";
        }
    }
}