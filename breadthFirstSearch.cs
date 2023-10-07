using System.Collections.Generic;

namespace Maze {
  class BreadthFirstSearch : PathFindingAlgorithm {

    readonly int[] deltaX = new int[4]{0,1,0,-1};
    readonly int[] deltaY = new int[4]{1,0,-1,0};
    
    public BreadthFirstSearch(Board board) : base(board) {}

    
    public override void FindPath() {
      bool[,] visited = new bool[_board.Size / 2,_board.Size / 2];
      Queue<Node> queue = new Queue<Node>();
      visited[_board.Start.Item2 / 2,_board.Start.Item1 / 2] = true;
      queue.Enqueue(new Node(_board.Start,null));

      Node now = null;
      while(now == null || now.Pos != _board.Dest) {
        now = queue.Dequeue();
        for (int i = 0; i < 4; i++) {
          int nextX = now.Pos.Item1 + 2 * deltaX[i];
          int nextY = now.Pos.Item2 + 2 * deltaY[i];
          
          if (nextX >= _board.Size || nextX < 0 || nextY >= _board.Size || nextY < 0) 
            continue;
          
          if (visited[nextY / 2,nextX / 2]) continue;
          Node next = new Node((nextX,nextY),now);

          if (_board.Tile[next.mid.Item2,next.mid.Item1] == Board.TileType.WALL ) 
            continue;

          visited[nextY / 2,nextX / 2] = true;
          queue.Enqueue(next);
        }
      }

      Node n = now;
      while(n.Parent != null) {
        _board.MakePath(n.mid);
        _board.MakePath(n.Parent.Pos);
        n = n.Parent;
      }
      queue.Clear();
    }
  }
}