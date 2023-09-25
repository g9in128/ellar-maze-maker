using System;
using Maze;

class Program {
  public static void Main (string[] args) {
    Console.CursorVisible = false;
    Board board = new Board(25);
    //Player player = new Player(board);

    board.Render();
  }
}