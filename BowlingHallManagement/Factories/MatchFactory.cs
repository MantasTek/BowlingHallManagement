using BowlingHallManagement.Models;

namespace BowlingHallManagement.Factories
{
    /*
     * Factory Pattern Implementation for Match
     */
    public class MatchFactory
    {
        private static int nextId = 1;

        public static Match CreateMatch(Member player1, Member player2, Lane lane)
        {
            if (player1 == null || player2 == null)
                throw new ArgumentException("Both players must be members");

            if (player1.Id == player2.Id)
                throw new ArgumentException("You can not paly against yourself!");

            if (lane == null)
                throw new ArgumentException("Please select a valid lane");
            if (!lane.IsAvailable)
                throw new ArgumentException("The lane you selected is ocupied");

            // Creating a new Match object with sequential ID
            var match = new Match
            {
                Id = nextId,
                Player1 = player1,
                Player2 = player2,
                Lane = lane,
                Date = DateTime.Now,
                IsComplete = false,
                Duration = TimeSpan.FromHours(1)
            };
            
            nextId++;

              // Reserve the lane
            lane.Reserve(match.Duration);

            return match;
        }
    }
}
