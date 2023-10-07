using System;
using System.Collections.Generic;

namespace Maze {
  class PriorityQueue<T> where T : IComparable<T> {
    List<T> _heap;
    readonly bool _reverse;

    public PriorityQueue(bool reverse = false) {
      _heap = new List<T>();
      _heap.Add(default(T));
      _reverse = reverse;
      }

    public void EnQueue(T data) {
      _heap.Add(data);
      int now = _heap.Count - 1;
      while(now > 1) {
        int next = now / 2;
        if (_heap[now].CompareTo(_heap[next]) > 0 ^ _reverse) {
          T tmp = _heap[now];
          _heap[now] = _heap[next];
          _heap[next] = tmp;
        }
        now = next;
      }
    }

    public T DeQueue() {
      T ret = _heap[1];
      int last = _heap.Count - 1;
      _heap[1] = _heap[last];
      _heap.RemoveAt(last);
      int now = 1;
      while(true) {
        int left = now * 2;
        int right = now * 2 + 1;
        if (right >= last) break;
        int change;
        if (_heap[left].CompareTo(_heap[right]) > 0 ^ _reverse) {
          change = left;
        }else {
          change = right;
        }
        if (_heap[change].CompareTo(_heap[now]) > 0 ^ _reverse) {
          T tmp = _heap[now];
          _heap[now] = _heap[change];
          _heap[change] = tmp;
          now = change;
        }else break;
       }
      return ret;
    }

    public void Clear() {
      _heap.Clear();
    }
  }
}