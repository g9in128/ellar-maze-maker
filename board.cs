using System;
using System.Collections.Generic;

namespace Maze {
  
  class Board {

    public enum TileType {
      EMPTY,
      WALL,
      PATH
    }

    const char EMPTY_CHAR = ' ';//\u28a1
    const char WALL_CHAR = '\u2588';
    const char PATH_CHAR = '\u25cf';
    public TileType[,] Tile {get;private set;}
    public int Size {
      get {return Tile.GetLength(0);}
    }
    private (int,int) dest;
    
    public Board(int size) {
      if (size % 2 == 0) size++;
      Tile = new TileType[size,size];
      dest = (size-2,size-2);
      Reset();
    }
    
    public void Reset() {
      for(int y = 0 ; y < Size; y++) {
        for (int x = 0; x < Size; x++) {
          if (x % 2 == 0 || y % 2 == 0) {
            Tile[y,x] = TileType.WALL;
          }
          else {
            Tile[y,x] = TileType.EMPTY;
          }
        }
      }
      try {
        GenerateByEller2();
      }catch (Exception e) {
        Console.WriteLine(e.Message);
        Console.WriteLine(e.StackTrace);
      }
      Tile[0,1] = Tile[0,Size-2] = TileType.EMPTY;
    }
    
    void GenerateByEller1() {
      Random rand = new Random();
      Dictionary<int,HashSet<(int,int)>> sets = new Dictionary<int,HashSet<(int, int)>>();
      HashSet<(int,int)> newset = new HashSet<(int, int)>();
      Dictionary<(int,int),int> universalSet = new Dictionary<(int,int),int>();
      
      newset.Add((1,1));
      int setCount = 0;
      universalSet[(1,1)] = setCount;
      
      for (int x = 3; x < Size ; x+=2) {
        if (rand.Next(0,2) == 0) {
          newset.Add((x,1));
          Tile[1,x-1] = TileType.EMPTY;
          universalSet[(x,1)] = setCount;
        }
        else {
          sets[setCount] = newset;
          newset = new HashSet<(int, int)>();
          newset.Add((x,1));
          universalSet[(x,1)] = ++setCount;
        }
      }
      sets[setCount] = newset;
      foreach(int i in sets.Keys) {
        HashSet<(int,int)> values = sets[i];
        int downs = rand.Next(1,values.Count + 1);
        
        List<(int,int)> temp = new List<(int,int)>(values);
        for (int j = 0; j < downs; j++) {
          (int,int) down = temp[rand.Next(0,temp.Count)];
          Tile[down.Item2 + 1,down.Item1] = TileType.EMPTY;
          universalSet[(down.Item1,down.Item2 + 2)] = i;
          values.Add((down.Item1,down.Item2 + 2));
          temp.Remove(down);
        }
      }
        
      for(int y = 3 ; y < Size - 2; y+=2) {
        if (!universalSet.ContainsKey((1,y))) {
          HashSet<(int,int)> temp = new HashSet<(int, int)>();
          temp.Add((1,y));
          sets[++setCount] = temp;
          universalSet[(1,y)] = setCount;
        }
        for (int x = 3; x < Size; x+=2) {
          int prevSet = universalSet[(x-2,y)];
          if (!universalSet.ContainsKey((x,y))) {
            if(rand.Next(0,2) == 0) {
              sets[prevSet].Add((x,y));
              universalSet[(x,y)] = prevSet;
              Tile[y,x-1] = TileType.EMPTY;
            }else {
              HashSet<(int,int)> temp = new HashSet<(int, int)>();
              temp.Add((x,y));
              sets[++setCount] = temp;
              universalSet[(x,y)] = setCount;
            }
            continue;
          }

          int mySet = universalSet[(x,y)];
          if (mySet != prevSet && rand.Next(0,2) == 0) {
            foreach((int,int) i in sets[mySet]) {
              universalSet[i] = prevSet;
            }
            Tile[y,x-1] = TileType.EMPTY;
            sets[prevSet].UnionWith(sets[mySet]);
            sets.Remove(mySet);
          }
        }

        foreach(int i in sets.Keys) {
          List<(int,int)> values = new List<(int,int)>(sets[i]);
          List<(int,int)> temp = values.FindAll(x => x.Item2 == y);
          int downs = rand.Next(1,temp.Count + 1);
          for (int j = 0; j < downs; j++) {
            int index = rand.Next(0,temp.Count);
            (int,int) down = temp[index];
            Tile[down.Item2 + 1,down.Item1] = TileType.EMPTY;
            universalSet[(down.Item1,down.Item2 + 2)] = i;
            sets[i].Add((down.Item1,down.Item2 + 2));
            temp.Remove(down);
          }
        }

      }
      
      for(int x = 3; x < Size - 1; x+=2) {
        (int,int) left = (x-2,Size-2),right = (x,Size-2);
        if (!universalSet.ContainsKey(left) || !universalSet.ContainsKey(right)) {
          Tile[Size-2,x-1] = TileType.EMPTY;
          continue;
        }
        if (universalSet[left] != universalSet[right]) {
          Tile[Size-2,x-1] = TileType.EMPTY;
        }
      }

      
    }
      
    void GenerateByEller2() {
      Random rand = new Random();
      int[] now = new int[Size / 2],next = new int[Size / 2];
      Dictionary<int,List<int>> setsNow = new Dictionary<int, List<int>>(),
        setsNext = new Dictionary<int, List<int>>();
      for(int i = 0; i < Size / 2; i++) {
        now[i] = next[i] = -1;
      }
      
      int setCount = 0;

      for (int y = 1;y < Size - 2; y+= 2) {
        if (now[0] < 0) {
          now[0] = setCount++;
          List<int> list = new List<int>(now.Length / 2);
          list.Add(0);
          setsNow[now[0]] = list;
        }
        for(int x = 1; x < now.Length;x++) {
          if (now[x] < 0) {
            if (rand.Next(2) == 0) {
              now[x] = now[x-1];
              Tile[y,2 * x] = TileType.EMPTY;
              setsNow[now[x]].Add(x);
            }else {
              now[x] = setCount++;
              List<int> list = new List<int>(now.Length / 3);
              list.Add(x);
              setsNow[now[x]] = list;
            }
            continue;
          }
          if (now[x] == now[x-1]) continue;
          if (rand.Next(2) == 0) {
            setsNow.Remove(now[x]);
            for(int i = 0; i < now.Length;i++) {
              if (now[i] == now[x]) now[i] = now[x-1];
            }
            Tile[y,2 * x] = TileType.EMPTY;
          }
        }

        
        Console.WriteLine($"----{y}----");
        foreach(KeyValuePair<int,List<int>> i in setsNow) {
          Console.Write(i.Key + " : ");
          foreach(int j in i.Value) Console.Write(j + ", ");
          Console.WriteLine();
        }
        Console.Write("[");
        foreach(int i in now) Console.Write($" {i} ");
        Console.WriteLine("]");
        Console.WriteLine("---------");
        
        foreach(KeyValuePair<int,List<int>> i in setsNow) {
          int downs = rand.Next(1,i.Value.Count + 1);
          for(int j = 0; j < downs;j++) {
            int choice = rand.Next(0,i.Value.Count);
            if (!setsNext.ContainsKey(i.Key)) {
              List<int> list = new List<int>(now.Length / 3);
              list.Add(i.Value[choice]);
              setsNext[i.Key] = list;
            }else {
              setsNext[i.Key].Add(i.Value[choice]);
            }
            next[i.Value[choice]] = i.Key;
            Tile[y + 1,2 * i.Value[choice] + 1] = TileType.EMPTY;
            i.Value.Remove(i.Value[choice]);
          }
        }

        var tmp = now;
        now = next;
        next = tmp;
        for(int x = 0; x < now.Length;x++) {
          next[x] = -1;
        }
        
        var tmp2 = setsNow;
        setsNow = setsNext;
        setsNext = tmp2;
        
        Console.WriteLine($"----{y + 2}(B)---");
        foreach(KeyValuePair<int,List<int>> i in setsNow) {
          Console.Write(i.Key + " : ");
          foreach(int j in i.Value) Console.Write(j + ", ");
          Console.WriteLine();
        }
        Console.Write("[");
        foreach(int i in now) Console.Write($" {i} ");
        Console.WriteLine("]");
        Console.WriteLine("---------");
        setsNext.Clear();
      }

      
      for(int x = 1; x < now.Length; x++) {
        if (now[x-1] != now[x] || now[x-1] < 0 || now[x] < 0) {
          Tile[Size - 2,2 * x] = TileType.EMPTY; 
        }
      }
    }
    
    //오류 있어서 앨러 알고리즘으로 바꿈
    void GenerateBySideWinder() {
      Random rand = new Random();
      int cnt = 1;
      for(int y = 0 ; y < Size; y++) {
        for (int x = 0; x < Size; x++) {
          if (x % 2 == 0 || y % 2 == 0) 
              continue;
          int rnum = rand.Next(0,2);
          if (rnum == 0) {
            if (x != dest.Item1) {
              Tile[y,x + 1] = TileType.EMPTY;
              cnt++;
            }
            else {
              if (cnt == 1) Tile[y,x - 1] = TileType.EMPTY;
              cnt = 1;
            }
          }
          else {
            int rindex = rand.Next(0,cnt);
            int rx = x - rindex * 2;
            Tile[y + (y == dest.Item2 ? -1:1),rx] = TileType.EMPTY;
            cnt = 1;
          }
        }
      }
    }

    //오류 있어서 앨러 알고리즘으로 바꿈
    void GenerateByBinaryTree() {
      Random rand = new Random();
      for(int y = 0 ; y < Size; y++) {
        for (int x = 0; x < Size; x++) {
          if (x % 2 == 0 || y % 2 == 0) 
              continue;
          
          int rnum = rand.Next(0,2);
          if (rnum == 0) {
            if (x != dest.Item1) Tile[y,x + 1] = TileType.EMPTY;
          }
          else if (y != dest.Item2) {
            Tile[y + 1,x] = TileType.EMPTY;
          }
        }
      }
    }

    public void MovePlayer((int,int) cordinate) {
      if (Tile[cordinate.Item2,cordinate.Item1] == TileType.EMPTY) {
        Tile[cordinate.Item2,cordinate.Item1] = TileType.PATH;
      }
    }

    public void MovePlayer(int x,int y) {MovePlayer((x,y));}
  

    public void Render() {
      for (int y = 0; y < Size; y++) {
        for (int x = 0; x < Size; x++) {
          char shape;
          Console.ForegroundColor = getTileShape(Tile[y,x],out shape);
          Console.Write(shape);
        }
        Console.WriteLine();
      }
    }

    ConsoleColor getTileShape(TileType tt,out char shape) {
      switch (tt) {
      case TileType.WALL:
        shape = WALL_CHAR;
        return ConsoleColor.Green;
      case TileType.PATH:
        shape = PATH_CHAR;
        return ConsoleColor.Blue;
      case TileType.EMPTY:
      default:
        shape = EMPTY_CHAR;
        return ConsoleColor.Red;
      }
    }
    
  }
}