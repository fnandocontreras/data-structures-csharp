using System.Collections.Generic;
using System.Linq;

namespace Libs
{
    public class HeapNode<T>
    {
        public HeapNode(T element, int freq)
        {
            Element = element;
            Freq = freq;
        }
        public int Freq { get; set; }
        public T Element { get; set; }
    }
    public class BinaryHeap<T>
    {
        List<HeapNode<T>> _nodes;

        public BinaryHeap()
        {
            _nodes = new List<HeapNode<T>>();
        }

        public void BuildHeap(IEnumerable<HeapNode<T>> elements)
        {
            foreach (var item in elements)
            {
                _nodes.Add(item);
                HeapifyBottomUp(HeapSize);
            }
        }

        private HeapNode<T> this[int index]
        {
            get { return _nodes[index - 1]; }
            set { _nodes[index - 1] = value; }
        }

        public void Insert(HeapNode<T> obj)
        {
            _nodes.Add(obj);
            HeapifyBottomUp(HeapSize);
        }

        public HeapNode<T> ExtractMin()
        {
            var min = this[1];
            
            this[1] = this[HeapSize];
            _nodes.RemoveAt(HeapSize - 1);

            Heapify_TopDown(1);
            return min;
        }

        public void HeapifyBottomUp(int i)
        {
            if (i == 1)
                return;
            int parent = i / 2;
            if (this[i].Freq < this[parent].Freq)
            {
                Exchange(i, parent);
                HeapifyBottomUp(parent);
            }
        }

        public void Heapify_TopDown(int i)
        {
            int left = i * 2, right = (i * 2) + 1;
            if (left > HeapSize)
                return;

            int min = i;
            if (this[left].Freq < this[i].Freq)
                min = left;
            if (right <= HeapSize && this[right].Freq < this[min].Freq)
                min = right;
            if (min != i)
            {
                Exchange(i, min);
                Heapify_TopDown(min);
            }
        }

        private void Exchange(int i, int j)
        {
            var tmp = this[i];
            this[i] = this[j];
            this[j] = tmp;
        }

        public int HeapSize => _nodes.Count();
    }
}
