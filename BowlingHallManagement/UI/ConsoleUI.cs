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
            var menuItems = new List<string>
            {
                "=== Welcome to Bowling Hall! ===",
                "Please enter a number to select an option:",
                "1. Register a new member",
                "2. List all members",
                "3. Create a new match",
                "4. Record match results",
                "5. List all matches",
                "6. Simulate match results",
                "0. Exit"
            };
            
            bool exit = false;
            
            while (!exit)
            {
                Console.Clear();
                PrintMenu(menuItems);

                exit = int.TryParse(Console.ReadLine(), out int choice) 
                    ? HandleMenuChoice(choice)
                    : DisplayError("Please enter a valid number.");
            }

            Console.WriteLine("Thank you for using the Bowling Hall Management System!");
        }
        
        private void PrintMenu(List<string> menuItems)
        {
            foreach (var item in menuItems)
            {
                Console.WriteLine(item);
            }
            Console.Write("\nEnter your choice: ");
        }

        private bool HandleMenuChoice(int choice) => choice switch
        {
            0 => true, // Exit the application
            1 => ExecuteAction("Register a New Member", RegisterMember),
            2 => ExecuteAction("All Members", ListAllMembers),
            3 => ExecuteAction("Create a New Match", CreateMatch),
            4 => ExecuteAction("Record Match Results", RecordMatchResults),
            5 => ExecuteAction("All Matches", ListAllMatches),
            6 => ExecuteAction("Simulate Match Results", SimulateMatchResults),
            _ => DisplayError("Invalid choice.")
        };

        private bool ExecuteAction(string title, Action action)
        {
            Console.Clear();
            Console.WriteLine($"=== {title} ===\n");
            
            try
            {
                action();
            }
            catch (Exception ex)
            {
                DisplayMessage($"Error: {ex.Message}");
            }
            
            return false; // Don't exit the application
        }

        private bool DisplayError(string message)
        {
            DisplayMessage(message);
            return false; // Don't exit the application
        }

        private void DisplayMessage(string message)
        {
            var messageList = new List<string> { message, "\nPress any key to continue..." };
            PrintMenu(messageList);
            Console.ReadKey();
        }

        private int GetValidIntInput(string prompt, Func<int, bool> validator = null, string errorMessage = "Invalid input.")
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out int result) && (validator == null || validator(result)))
                {
                    return result;
                }
                Console.WriteLine(errorMessage);
            }
        }

        private void DisplayList<T>(IEnumerable<T> items, string emptyMessage = "No items found.")
        {
            var displayList = new List<string>();
            
            if (items.Any())
            {
                foreach (var item in items)
                {
                    displayList.Add(item.ToString());
                    displayList.Add(new string('-', 50));
                }
            }
            else
            {
                displayList.Add(emptyMessage);
            }
            
            displayList.Add("\nPress any key to continue...");
            PrintMenu(displayList);
            Console.ReadKey();
        }

        private void RegisterMember()
        {
            Console.Write("Enter member name: ");
            string name = Console.ReadLine();
            
            Console.Write("Enter member email: ");
            string email = Console.ReadLine();

            _memberService.RegisterMember(name, email);
            Console.WriteLine("\nMember registered successfully!");
        }

        private void ListAllMembers()
        {
            var members = _memberService.GetAllMembers();
            DisplayList(members, "No members registered yet.");
        }

        private void CreateMatch()
        {
            var members = _memberService.GetAllMembers();
            
            if (members.Count < 2)
            {
                DisplayMessage("You need at least 2 members to create a match. Please register more members.");
                return;
            }

            // List available members
            var memberList = new List<string> { "Available Members:" };
            memberList.AddRange(members.Select(m => $"{m.Id}. {m.Name}"));
            PrintMenu(memberList);

            // Select player 1
            int player1Id = GetValidIntInput("\nSelect Player 1 (ID): ", 
                id => _memberService.GetMemberById(id) != null,
                "Invalid member ID.");
            var player1 = _memberService.GetMemberById(player1Id);

            // Select player 2
            int player2Id = GetValidIntInput("Select Player 2 (ID): ", 
                id => _memberService.GetMemberById(id) != null && id != player1Id,
                "Invalid member ID or same as Player 1.");
            var player2 = _memberService.GetMemberById(player2Id);

            // Select lane
            int lane = GetValidIntInput("Enter lane number: ", 
                ln => ln > 0, 
                "Invalid lane number. Must be a positive number.");

            _matchService.CreateMatch(player1, player2, lane);
            Console.WriteLine("\nMatch created successfully!");
        }

        private void RecordMatchResults()
        {
            var matches = _matchService.GetActiveMatches();
            
            if (!matches.Any())
            {
                DisplayMessage("No active matches found. Please create a match first.");
                return;
            }

            // List active matches
            var matchList = new List<string> { "Active Matches:" };
            matchList.AddRange(matches.Select(m => $"{m.Id}. {m.Player1.Name} vs {m.Player2.Name} (Lane {m.Lane.LaneNumber})"));
            PrintMenu(matchList);

            // Select match
            int matchId = GetValidIntInput("\nSelect Match (ID): ",
                id => _matchService.GetMatchById(id) != null && !_matchService.GetMatchById(id).IsComplete,
                "Invalid match ID or match already completed.");
            
            var selectedMatch = _matchService.GetMatchById(matchId);

            // Enter scores
            int player1Score = GetValidIntInput($"Enter score for {selectedMatch.Player1.Name}: ",
                score => score >= 0,
                "Invalid score. Must be a non-negative number.");

            int player2Score = GetValidIntInput($"Enter score for {selectedMatch.Player2.Name}: ",
                score => score >= 0,
                "Invalid score. Must be a non-negative number.");

            // Record scores and determine winner
            _matchService.RecordMatchResults(matchId, player1Score, player2Score);
            
            // Get the updated match with winner information
            selectedMatch = _matchService.GetMatchById(matchId);
            
            var resultMessage = new List<string> 
            {
                "\nMatch results recorded successfully!",
                selectedMatch.ToString()
            };
            PrintMenu(resultMessage);
        }

        private void ListAllMatches()
        {
            var matches = _matchService.GetAllMatches();
            DisplayList(matches, "No matches created yet.");
        }

        private void SimulateMatchResults()
        {
            var matches = _matchService.GetActiveMatches();
            
            if (!matches.Any())
            {
                DisplayMessage("No active matches found. Please create a match first.");
                return;
            }

            // List active matches
            var matchList = new List<string> { "Active Matches:" };
            matchList.AddRange(matches.Select(m => $"{m.Id}. {m.Player1.Name} vs {m.Player2.Name} (Lane {m.Lane.LaneNumber})"));
            PrintMenu(matchList);

            // Select match
            int matchId = GetValidIntInput("\nSelect Match to Simulate (ID): ",
                id => _matchService.GetMatchById(id) != null && !_matchService.GetMatchById(id).IsComplete,
                "Invalid match ID or match already completed.");

            // Simulate the match
            _matchService.SimulateMatch(matchId);
            
            // Get the updated match with simulation results
            var simulatedMatch = _matchService.GetMatchById(matchId);
            
            var resultMessage = new List<string> 
            {
                "\nMatch simulation completed successfully!",
                simulatedMatch.ToString()
            };
            PrintMenu(resultMessage);
        }
    }
}