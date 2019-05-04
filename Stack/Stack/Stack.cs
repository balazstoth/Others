using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stack
{
    class Stack<T> : IEnumerable<T>
    {
        private const int stackLimit = 100000;
        private T[] values;
        private int capacity;

        public int Count { get; private set; }

        public Stack(int count)
        {
            if (count > stackLimit)
                throw new ArgumentOutOfRangeException(count.ToString());

            values = new T[count];
            Count = 0;
            capacity = count;
        }

        public void Enqueue(T value)
        {
            if (Count == capacity)
                throw new InvalidOperationException("Stack is full!");

            values[Count++] = value;
        }

        public T Dequeue()
        {
            if (Count == 0)
                throw new InvalidOperationException("Stack is empty!");

            return values[--Count];
        }

        public override string ToString()
        {
            return $"Stack has {Count} items from {capacity}";
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = Count - 1; i >= 0; i--)
                yield return values[i];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
