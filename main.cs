using System;
using Maze;

class Program {
  public static void Main (string[] args) {
    Console.CursorVisible = false;
    
    Board board = new Board(25);

    board.Render();
    
    // int lastTick = 0;
    // const int WAIT_TICK = 1000/30;
    // while (true) {
    //   int deltaTick = System.Environment.TickCount - lastTick;
    //   if (deltaTick < WAIT_TICK) continue;
    //   lastTick += deltaTick;

    //   Console.SetCursorPosition(0,0);
    //   board.Render();
    // }
  }
}