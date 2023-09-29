

namespace Maze {
  abstract class Player {
    
    public int PosX {get; protected set;}
    public int PosY {get; protected set;}
    

    protected Board _board;

    public Player(Board board) {
      _board = board;
      PosX = board.Start.Item1;
      PosY = board.Start.Item2;
    }

    public abstract void FindPath();
    
  }
}