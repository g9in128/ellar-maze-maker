using System;

namespace Maze {
  class AStar : PathFindingAlgorithm {

                           // ↑  ↗  →  ↘  ↓  ↙  ←  ↖
    readonly int[] deltaX = { 0, 1, 1, 1, 0,-1,-1,-1};
    readonly int[] deltaY = { 1, 1, 0,-1,-1,-1, 0, 1};
    readonly int[] cost =   {10,14,10,14,10,14,10,14};

    int distance((int,int) a,(int,int) b) {
      return (int) Math.Sqrt(Math.Pow(a.Item1 - b.Item1,2) + Math.Pow(a.Item2 - b.Item2,2));
    }

    public AStar(Board board) : base(board) {}

    public override void FindPath() {
      PriorityQueue<Node> ptq = new PriorityQueue<Node>(true);
      bool[,] visited = new bool[_board.Size,_board.Size];
      ptq.EnQueue(new Node(_board.Start,null) {F = 0,G = 0,H = 0});
      Node now = null;
      while(now == null || now.Pos != _board.Dest) {
        now = ptq.DeQueue();
        visited[now.Pos.Item2,now.Pos.Item1] = true;
        for (int i = 0; i < 8;i++) {
          int nextX = now.Pos.Item1 + deltaX[i];
          int nextY = now.Pos.Item2 + deltaY[i];
          
          if (nextX < 0 || nextX >= _board.Size || nextY < 0 || nextY >= _board.Size) 
            continue;

          if (visited[nextY,nextX]) continue;

          if (_board.Tile[nextY,nextX] == Board.TileType.WALL) 
            continue;

          Node next = new Node((nextX,nextY),now) {G = now.G + cost[i], H =distance((nextX,nextY),_board.Dest)};
          ptq.EnQueue(next);
        }
      }

      Node n = now;
      while(n.Parent != null) {
        _board.MakePath(n.Pos);
        n = n.Parent;
      }
    }
    
  }
}