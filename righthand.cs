using System;

namespace Maze
{
  class RightHand : Player
  {
    private (int,int) facing;
    public RightHand(Board board) : base(board) {
      facing = (1,0);
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
        PosX += dirction.Item1;
        PosY += dirction.Item2;
        return (PosX,PosY);
      }
      return (PosX + dirction.Item1,PosY + dirction.Item2);
    }
    
    public override void FindPath()
    {
      while(PosX != _board.Dest.Item1 || PosY != _board.Dest.Item2) {
        (int,int) rightLoc = forward(turn(true));
        (int,int) forwardLoc = forward();
        if (_board.Tile[rightLoc.Item2,rightLoc.Item1] != Board.TileType.WALL) {
          PosX = rightLoc.Item1;
          PosY = rightLoc.Item2;
          facing = turn(true);
          bool b = _board.MovePlayer(PosX,PosY);
        }
        else if (_board.Tile[forwardLoc.Item2,forwardLoc.Item1] != Board.TileType.WALL) {
          bool b = _board.MovePlayer(forward(true));
        }else {
          facing = turn(false);
        }
      }
      Console.WriteLine();
    }
  }
}