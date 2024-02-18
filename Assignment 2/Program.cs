//Develop a console-based 2D game called "Gem Hunters" where players compete to collect the most gems within a set number of turns.

using System;

public class Position
{
    public int X { get; set; }
    public int Y { get; set; }

    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }
}

public class Player
{
    public string Name { get; set; }
    public Position Position { get; set; }
    public int GemCount { get; set; }

    public Player(string name, Position position)
    {
        Name = name;
        Position = position;
        GemCount = 0;
    }

    public void Move(char direction)
    {
        switch (direction)
        {
            case 'U': //UP Direction
                Position.Y--;
                break;
            case 'D': //DOWN Direction
                Position.Y++;
                break;
            case 'L': //LEFT Direction
                Position.X--;
                break;
            case 'R': //RIGHT Direction
                Position.X++;
                break;
            default:
                Console.WriteLine("Invalid direction");
                break;
        }
    }
}

public class Cell
{
    public string Occupant { get; set; }

    public Cell(string occupant)
    {
        Occupant = occupant;
    }
}

public class Board
{
    public Cell[,] Grid { get; }

    public Board()
    {
        Grid = new Cell[6, 6];
        InitializeBoard();
    }

    private void InitializeBoard()
    {
        // Initialize grid with empty cells
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                Grid[i, j] = new Cell("-");
            }
        }

        // Place players
        Grid[0, 0].Occupant = "P1";
        Grid[5, 5].Occupant = "P2";

        // Place obstacles (randomly for demonstration purposes)
        Random rand = new Random();
        for (int i = 0; i < 5; i++)
        {
            int x = rand.Next(6);
            int y = rand.Next(6);
            if (Grid[x, y].Occupant == "-")
            {
                Grid[x, y].Occupant = "O";
            }
            else
            {
                i--; // Retry placing obstacle if the cell is already occupied
            }
        }

        // Place gems (randomly for demonstration purposes)
        for (int i = 0; i < 10; i++)
        {
            int x = rand.Next(6);
            int y = rand.Next(6);
            if (Grid[x, y].Occupant == "-")
            {
                Grid[x, y].Occupant = "G";
            }
            else
            {
                i--; // Retry placing gem if the cell is already occupied
            }
        }
    }

    //Prints the current state of the board to the console.
    public void Display()
    {
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                Console.Write(Grid[i, j].Occupant + " ");
            }
            Console.WriteLine();
        }
    }

    //Checks if the move is valid
    public bool IsValidMove(Player player, char direction)
    {
        int newX = player.Position.X;
        int newY = player.Position.Y;

        switch (direction)
        {
            case 'U': //Up Direction
                newY--;
                break;
            case 'D': //Down Direction
                newY++;
                break;
            case 'L': //Left Direction
                newX--;
                break;
            case 'R': //Right Direction
                newX++;
                break;
            default:
                return false;
        }

        if (newX < 0 || newX >= 6 || newY < 0 || newY >= 6)
            return false; // Out of bounds

        if (Grid[newY, newX].Occupant == "O")
            return false; // Obstacle

        return true;
    }

    //Checks if the player's new position contains a gem and updates the player's GemCount.
    public bool CollectGem(Player player)
    {
        if (Grid[player.Position.Y, player.Position.X].Occupant == "G")
        {
            player.GemCount++;
            Grid[player.Position.Y, player.Position.X].Occupant = "-";
            return true;
        }
        return false;
    }
}

public class Game
{
    public Board Board { get; }
    public Player Player1 { get; }
    public Player Player2 { get; }
    public Player CurrentTurn { get; private set; }
    public int TotalTurns { get; private set; }

    public Game()
    {
        Board = new Board();
        Player1 = new Player("P1", new Position(0, 0));
        Player2 = new Player("P2", new Position(5, 5));
        CurrentTurn = Player1;
        TotalTurns = 0;
    }

    // Begins the game, displays the board, and alternates player turns.
    public void Start()
    {
        while (!IsGameOver())
        {
            //Console.WriteLine($"\nCurrent turn: {CurrentTurn.Name}");
            //Console.WriteLine($"\nPlayer {CurrentTurn.Name}'s Turn");
            Console.WriteLine($"\nTurn {TotalTurns + 1}: Player {CurrentTurn.Name}'s turn");
            Board.Display();

            Console.Write("\nEnter the direction (U/D/L/R): ");
            char move = char.ToUpper(Console.ReadKey().KeyChar);

            if (Board.IsValidMove(CurrentTurn, move))
            {
                CurrentTurn.Move(move);
                if (Board.CollectGem(CurrentTurn))
                {
                    Console.WriteLine($"\nPlayer {CurrentTurn.Name} collected a gem!");
                }

                SwitchTurn();
                TotalTurns++;
            }
            else
            {
                Console.WriteLine("\nInvalid move..!");
            }
        }

        AnnounceWinner();
    }

    //Switches between Player1 and Player2.
    public void SwitchTurn()
    {
        CurrentTurn = (CurrentTurn == Player1) ? Player2 : Player1;
    }

    //Checks if the game has reached its end condition
    public bool IsGameOver()
    {
        return TotalTurns >= 30;
    }

    //Determines and announces the winner based on GemCount of both players.
    public void AnnounceWinner()
    {
        Console.WriteLine($"\nPlayer 1 gems: {Player1.GemCount}");
        Console.WriteLine($"\nPlayer 2 gems: {Player2.GemCount}");
        Console.WriteLine("\nGame over..!");

        if (Player1.GemCount > Player2.GemCount)
        {
            Console.WriteLine("Player 1 wins..!");
        }
        else if (Player2.GemCount > Player1.GemCount)
        {
            Console.WriteLine("Player 2 wins..!");
        }
        else
        {
            Console.WriteLine("\nIt's a tie..!");
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Game game = new Game();
        game.Start();
    }
} 
