# Memory Game – WPF & C# Application

This is a "Memory" game application developed in C# using WPF (Windows Presentation Foundation). It follows the MVVM (Model-View-ViewModel) architecture and utilizes data binding for efficient interaction between the UI and the logic.

## Main Features

- **User Authentication**
  - Create new user account with an associated image (jpg/png/gif)
  - Select an existing user to continue
  - Delete user (along with all associated data)

- **Main Menu**
  - Choose image category (3 predefined categories)
  - Start a new game (standard or custom)
  - Load a saved game
  - Save game progress
  - View statistics
  - Exit to login screen

- **Game Modes**
  - **Standard**: 4x4 grid
  - **Custom**: M x N grid (values between 2 and 6), with an even number of tiles

- **Game Logic**
  - Random distribution of image pairs
  - Prevents clicking the same tile twice
  - Matching pairs disappear
  - Countdown timer
  - Game state saving (layout + time)

- **Statistics**
  - Saves individual statistics per user
  - Displayed format: `Name - Games Played - Games Won- Win Rate`

## 🗂 Project Structure

- `Model/`: contains models for users, game data, and statistics
- `ViewModel/`: contains logic and implemented commands (ICommand)
- `Views/`: UI pages (login, game, statistics, etc.)
- `Resources/`: images for tiles and users
- `Services/`: saving/loading to and from files (JSON/XML)
- `Converters/`: convert bool to visibility and string to image

## Technologies Used

- C# with .NET (WPF)
- MVVM Design Pattern
- Data Binding
- ICommand implementation
- JSON/XML serialization
- WPF XAML GUI

## How to Run

1. Open the solution in Visual Studio
2. Build and run the application (F5)
3. Create a new user and start playing
