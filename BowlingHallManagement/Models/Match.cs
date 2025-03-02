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
        public Member? Winner { get; private set; }
        public bool IsComplete { get; private set; }
        public TimeSpan Duration { get; set; } = TimeSpan.FromHours(1);// Default duration is 1 hour

        public Match(int id, Member player1, Member player2, Lane lane)
        {
            Id = id;
            Player1 = player1;
            Player2 = player2;
            Lane = lane;
            Date = DateTime.Now;
            IsComplete = false;

            Lane.Reserve(Duration);
        }

        public void RecordScores(int scorePlayer1, int scorePlayer2)
        {
            ScorePlayer1 = scorePlayer1;
            ScorePlayer2 = scorePlayer2;
            DetermineWinner();

            Lane.Release();
        }

        private void DetermineWinner()
        {
            if (ScorePlayer1 > ScorePlayer2)
                Winner = Player1;
            else if (ScorePlayer2 > ScorePlayer1)
                Winner = Player2;
            // If the scores are equal, the winner is not assigned (null)

            IsComplete = true;
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
    