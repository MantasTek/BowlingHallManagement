# Wireframe Documentation: Match Creation Screen

## Overview
This wireframe represents a potential graphical user interface (GUI) for the "Create a Match" functionality in the Bowling Hall Management System.
The design focuses on simplicity and usability while incorporating the core functionality of the current console application.

## Key Components

### Header & Navigation
- **Header Bar**: Contains the application name "Bowling Hall Management System"
- **Navigation Menu**: Provides access to different sections of the application (Dashboard, Members, Matches, Lanes, Reports)
- **Current Section**: "Matches" is highlighted to indicate the current section

### Match Creation Form
- **Player Selection**: Dropdown menus to select Player 1 and Player 2 from registered members
- **Lane Information**: Field to select a lane number for the match
- **Duration**: Input field to set the match duration (defaulted to 60 minutes)
- **Match Type**: Dropdown to select the type of match (e.g., Friendly, Competition)

### Available Lanes Display
- **Lane Status Panel**: Shows real-time information about lane availability
- **Lane Details**: Displays each lane's number and current status (Available or Reserved)
- **Time Information**: For reserved lanes, shows when they will become available

### Action Controls
- **Cancel Button**: Returns to the matches list without creating a match
- **Create Match Button**: Creates a new match with the selected players and lane
- **Create and Simulate Button**: Creates a match and automatically simulates the results

## Design Considerations

1. **Usability**: The interface is designed to be intuitive, with clear labeling and logical grouping of related elements.

2. **Information Accessibility**: Available lanes are displayed alongside the form, making it easy for the user to select an available lane without having to navigate to a different screen.

3. **Error Prevention**: By providing dropdown menus for member selection and displaying available lanes, the interface helps prevent users from entering invalid data.

4. **Efficiency**: The "Create and Simulate" button offers a shortcut for users who want to quickly generate match results without manually entering scores.

5. **Consistency**: The design follows a consistent color scheme and layout that would be maintained throughout the application.

## Relation to the Current Console Application
This wireframe translates the current console-based match creation functionality into a graphical interface, maintaining all existing features while enhancing usability.
It represents how the system could evolve from a console application to a GUI application while preserving the core business logic implemented in the current version.
