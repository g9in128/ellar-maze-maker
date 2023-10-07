using System;

namespace Maze {
    class Node : IComparable<Node> {
      public (int,int) Pos {get; private set;}
      public Node Parent {get; private set;}
      public int F {get;set;}
      int _g,_h;
      public int G {
        get => _g;
        set {
          _g = value;
          F = G + H;
        }
      }
      public int H {
        get => _h;
        set {
          _h = value;
          F = G + H;
        }
      }

      public Node((int,int) pos,Node parent) {
        Pos = pos;
        this.Parent = parent;
      }

      public (int,int) mid {
        get {
          if (Parent == null) return Pos;
          return ((Pos.Item1 + Parent.Pos.Item1) / 2,
                      (Pos.Item2 + Parent.Pos.Item2) / 2);
        }
      }

      public int CompareTo(Node other) {
        if (F == other.F) return 0;
        return F > other.F ? 1 : -1;
      }
    }
}