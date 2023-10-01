using System.Collections.Generic;

namespace Maze {
  class BreadthFirstSearch : PathFindingAlgorithm {

    bool[,] visited;
    Queue<Node> queue;

    readonly int[] deltaX = new int[4]{0,1,0,-1};
    readonly int[] deltaY = new int[4]{1,0,-1,0};
    
    public BreadthFirstSearch(Board board) : base(board) {
      queue = new Queue<Node>();
    }

    class Node {
      internal (int,int) pos;
      internal Node parent;

      internal Node((int,int) pos,Node parent) {
        this.pos = pos;
        this.parent = parent;
      }

      internal (int,int) mid {
        get {
          if (parent == null) return pos;
          return ((pos.Item1 + parent.pos.Item1) / 2,
                      (pos.Item2 + parent.pos.Item2) / 2);
        }
      }
    }
    
    public override void FindPath() {
      visited = new bool[_board.Size / 2,_board.Size / 2];
      visited[_board.Start.Item2 / 2,_board.Start.Item1 / 2] = true;
      queue.Enqueue(new Node(_board.Start,null));

      Node now = null;
      while(now == null || now.pos != _board.Dest) {
        now = queue.Dequeue();
        for (int i = 0; i < 4; i++) {
          int nextX = now.pos.Item1 + 2 * deltaX[i];
          int nextY = now.pos.Item2 + 2 * deltaY[i];
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
      while(n.parent != null) {
        _board.MakePath(n.mid);
        _board.MakePath(n.parent.pos);
        n = n.parent;
      }
    }
  }
}