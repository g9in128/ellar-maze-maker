using System;
using Maze;

class Program {
  public static void Main (string[] args) {
    Console.CursorVisible = false;
    
    Board board = new Board(121);

    board.Render();
  }
}