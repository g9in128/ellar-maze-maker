

namespace Maze
{
  class RightHand : PathFindingAlgorithm
  {
    (int,int) facing;
    int posX,posY;
    public RightHand(Board board) : base(board) {
      facing = (1,0);
      posX = board.Start.Item1;
      posY = board.Start.Item2;
    }

    private (int,int) turn(bool isRight) {
      (int,int) reverse;
      if (isRight) {
        reverse = (-1 * facing.Item2,facing.Item1);
      }else {
        reverse = (facing.Item2,-1 * facing.Item1);
      }
      return reverse;
    }

    private (int,int) forward(bool move = false) { return forward(facing,move); }

    private (int,int) forward((int,int) dirction,bool move = false) {
      if (move) {
        posX += dirction.Item1;
        posY += dirction.Item2;
        return (posX,posY);
      }
      return (posX + dirction.Item1,posY + dirction.Item2);
    }
    
    public override void FindPath()
    {
      while(posX != _board.Dest.Item1 || posY != _board.Dest.Item2) {
        (int,int) rightLoc = forward(turn(true));
        (int,int) forwardLoc = forward();
        if (_board.Tile[rightLoc.Item2,rightLoc.Item1] != Board.TileType.WALL) {
          posX = rightLoc.Item1;
          posY = rightLoc.Item2;
          facing = turn(true);
          bool b = _board.MakePath(posX,posY);
        }
        else if (_board.Tile[forwardLoc.Item2,forwardLoc.Item1] != Board.TileType.WALL) {
          bool b = _board.MakePath(forward(true));
        }else {
          facing = turn(false);
        }
      }
    }
  }
}