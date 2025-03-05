# Use Case: Create a Match
## Description
This use case describes the process for creating a new match between two members in the bowling hall.
## Actors
- Bowling hall staff
## Preconditions
- At least two members are registered in the system
- At least one lane is available
## Main Flow
1. The staff selects "Create a new match" from the main menu
2. The system displays a list of all registered members
3. The staff selects the first player by entering their ID
4. The system validates the first player's selection
5. The staff selects the second player by entering their ID
6. The system validates the second player's selection and confirms they are different from the first player
7. The staff enters a lane number to use for the match
8. The system validates the lane number and checks if the lane is available
9. The system creates a new match with the selected players and lane
10. The system confirms the match creation and displays the match information
11. The system reserves the lane for the duration of the match
## Alternative Flows
- **A1: Insufficient number of members**
  - If fewer than two members are registered, the system displays an error message and returns to the main menu
- **A2: Invalid member ID**
  - If an invalid member ID is entered, the system displays an error message and asks the user to try again
- **A3: Same member selected for both players**
  - If the same member is selected for both players, the system displays an error message and asks the user to select a different second player
- **A4: Invalid lane number**
  - If an invalid lane number is entered, the system displays an error message and asks the user to try again
- **A5: Lane not available**
  - If the selected lane is not available, the system displays an error message and asks the user to select a different lane
## Postconditions
- A new match is created in the system
- The selected lane is reserved for the duration of the match
- The match is added to the list of active matches
## Business Rules
- A member cannot play against themselves
- Each match must have exactly two players
- The lane must be available for reservation
- The match duration is set to 1 hour by default