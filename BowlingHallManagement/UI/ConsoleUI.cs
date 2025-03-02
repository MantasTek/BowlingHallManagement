using System;
using System.Linq;
using BowlingHallManagement.Models;
using BowlingHallManagement.Services;
using BowlingHallManagement.Services.Interfaces;

namespace BowlingHallManagement.UI
{
    public class ConsoleUI
    {
        private readonly IMemberService _memberService;
        private readonly IMatchService _matchService;

        public ConsoleUI()
        {
            // Initialize services with data storage
            var dataStorage = DataStorage.Instance;
            _memberService = new MemberService(dataStorage);
            _matchService = new MatchService(dataStorage);
        }

        public ConsoleUI(IMemberService memberService, IMatchService matchService)
        {
            // Constructor for dependency injection (useful for testing)
            _memberService = memberService ?? throw new ArgumentNullException(nameof(memberService));
            _matchService = matchService ?? throw new ArgumentNullException(nameof(matchService));
        }

        public void Run()
        {
            bool exit = false;
            
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("=== Bowling Hall Management System ===");
                Console.WriteLine("1. Register a new member");
                Console.WriteLine("2. List all members");
                Console.WriteLine("3. Create a new match");
                Console.WriteLine("4. Record match results");
                Console.WriteLine("5. List all matches");
                Console.WriteLine("6. Simulate match results");
                Console.WriteLine("0. Exit");
                Console.Write("\nEnter your choice: ");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.Clear();
                    
                    switch (choice)
                    {
                        case 0:
                            exit = true;
                            break;
                        case 1:
                            RegisterMember();
                            break;
                        case 2:
                            ListAllMembers();
                            break;
                        case 3:
                            CreateMatch();
                            break;
                        case 4:
                            RecordMatchResults();
                            break;
                        case 5:
                            ListAllMatches();
                            break;
                        case 6:
                            SimulateMatchResults();
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Press any key to continue...");
                            Console.ReadKey();
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Please enter a valid number. Press any key to continue...");
                    Console.ReadKey();
                }
            }

            Console.WriteLine("Thank you for using the Bowling Hall Management System!");
        }

        private void RegisterMember()
        {
            Console.WriteLine("=== Register a New Member ===\n");
            
            Console.Write("Enter member name: ");
            string name = Console.ReadLine();
            
            Console.Write("Enter member email: ");
            string email = Console.ReadLine();

            try
            {
                _memberService.RegisterMember(name, email);
                
                Console.WriteLine("\nMember registered successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private void ListAllMembers()
        {
            Console.WriteLine("=== All Members ===\n");
            
            var members = _memberService.GetAllMembers();
            
            if (members.Any())
            {
                foreach (var member in members)
                {
                    Console.WriteLine(member);
                    Console.WriteLine(new string('-', 50));
                }
            }
            else
            {
                Console.WriteLine("No members registered yet.");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private void CreateMatch()
        {
            Console.WriteLine("=== Create a New Match ===\n");
            
            var members = _memberService.GetAllMembers();
            
            if (members.Count < 2)
            {
                Console.WriteLine("You need at least 2 members to create a match. Please register more members.");
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                return;
            }

            // List available members
            Console.WriteLine("Available Members:");
            foreach (var member in members)
            {
                Console.WriteLine($"{member.Id}. {member.Name}");
            }

            // Select player 1
            Console.Write("\nSelect Player 1 (ID): ");
            if (!int.TryParse(Console.ReadLine(), out int player1Id))
            {
                Console.WriteLine("Invalid ID format.");
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                return;
            }

            var player1 = _memberService.GetMemberById(player1Id);
            if (player1 == null)
            {
                Console.WriteLine("Member not found.");
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                return;
            }

            // Select player 2
            Console.Write("Select Player 2 (ID): ");
            if (!int.TryParse(Console.ReadLine(), out int player2Id))
            {
                Console.WriteLine("Invalid ID format.");
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                return;
            }

            var player2 = _memberService.GetMemberById(player2Id);
            if (player2 == null)
            {
                Console.WriteLine("Member not found.");
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                return;
            }

            // Select lane
            Console.Write("Enter lane number: ");
            if (!int.TryParse(Console.ReadLine(), out int lane) || lane < 1)
            {
                Console.WriteLine("Invalid lane number.");
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                return;
            }

            try
            {
                _matchService.CreateMatch(player1, player2, lane);
                
                Console.WriteLine("\nMatch created successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private void RecordMatchResults()
        {
            Console.WriteLine("=== Record Match Results ===\n");
            
            var matches = _matchService.GetActiveMatches();
            
            if (!matches.Any())
            {
                Console.WriteLine("No active matches found. Please create a match first.");
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                return;
            }

            // List active matches
            Console.WriteLine("Active Matches:");
            foreach (var match in matches)
            {
                Console.WriteLine($"{match.Id}. {match.Player1.Name} vs {match.Player2.Name} (Lane {match.Lane})");
            }

            // Select match
            Console.Write("\nSelect Match (ID): ");
            if (!int.TryParse(Console.ReadLine(), out int matchId))
            {
                Console.WriteLine("Invalid ID format.");
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                return;
            }

            var selectedMatch = _matchService.GetMatchById(matchId);
            if (selectedMatch == null || selectedMatch.IsComplete)
            {
                Console.WriteLine("Match not found or already completed.");
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                return;
            }

            // Enter scores
            Console.WriteLine($"\nEnter score for {selectedMatch.Player1.Name}: ");
            if (!int.TryParse(Console.ReadLine(), out int player1Score) || player1Score < 0)
            {
                Console.WriteLine("Invalid score. Must be a non-negative number.");
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"Enter score for {selectedMatch.Player2.Name}: ");
            if (!int.TryParse(Console.ReadLine(), out int player2Score) || player2Score < 0)
            {
                Console.WriteLine("Invalid score. Must be a non-negative number.");
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                return;
            }

            try
            {
                // Record scores and determine winner
                _matchService.RecordMatchResults(matchId, player1Score, player2Score);
                
                // Get the updated match with winner information
                selectedMatch = _matchService.GetMatchById(matchId);
                
                Console.WriteLine("\nMatch results recorded successfully!");
                Console.WriteLine(selectedMatch);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private void ListAllMatches()
        {
            Console.WriteLine("=== All Matches ===\n");
            
            var matches = _matchService.GetAllMatches();
            
            if (matches.Any())
            {
                foreach (var match in matches)
                {
                    Console.WriteLine(match);
                    Console.WriteLine(new string('-', 50));
                }
            }
            else
            {
                Console.WriteLine("No matches created yet.");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private void SimulateMatchResults()
        {
            Console.WriteLine("=== Simulate Match Results ===\n");
            
            var matches = _matchService.GetActiveMatches();
            
            if (!matches.Any())
            {
                Console.WriteLine("No active matches found. Please create a match first.");
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                return;
            }

            // List active matches
            Console.WriteLine("Active Matches:");
            foreach (var match in matches)
            {
                Console.WriteLine($"{match.Id}. {match.Player1.Name} vs {match.Player2.Name} (Lane {match.Lane.LaneNumber})");
            }

            // Select match
            Console.Write("\nSelect Match to Simulate (ID): ");
            if (!int.TryParse(Console.ReadLine(), out int matchId))
            {
                Console.WriteLine("Invalid ID format.");
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                return;
            }

            try
            {
                // Simulate the match
                _matchService.SimulateMatch(matchId);
                
                // Get the updated match with simulation results
                var simulatedMatch = _matchService.GetMatchById(matchId);
                if (simulatedMatch != null && simulatedMatch.IsComplete)
                {
                    Console.WriteLine("\nMatch simulation completed successfully!");
                    Console.WriteLine(simulatedMatch);
                }
                else
                {
                    Console.WriteLine("\nError: Match simulation failed.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}