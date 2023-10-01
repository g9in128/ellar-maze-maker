

namespace Maze {
  abstract class PathFindingAlgorithm {
    
    protected Board _board;

    public PathFindingAlgorithm(Board board) {
      _board = board;
    }

    public abstract void FindPath();
    
  }
}