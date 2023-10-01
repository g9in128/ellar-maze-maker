using System;
using Maze;

class Program {
  public static void Main (string[] args) {
    Console.CursorVisible = false;
    Board board = new Board(49,(1,1),(47,1));
    
    PathFindingAlgorithm a = new RightHand(board);
    a.FindPath();
    board.Render();
    board.ClearPath();
    Console.WriteLine();

    PathFindingAlgorithm b = new BreadthFirstSearch(board);
    b.FindPath();
    
    board.Render();
  }
}