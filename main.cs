using System;
using Maze;

class Program {
  public static void Main (string[] args) {
    Console.CursorVisible = false;
    Board board = new Board(49);
    
    Player player = new RightHand(board);
    player.FindPath();
    Console.WriteLine();
    
    board.Render();
  }
}