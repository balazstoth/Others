using System;
using System.Collections;
using System.Collections.Generic;

namespace Queue
{
    class Queue<T> : IEnumerable<T>
    {
        private const int limit = 100000;
        private T[] values;
        private int capacity;
        private int InsertPointer;
        private int RemovePointer;

        public int Count { get; private set; }

        public Queue(int capacity)
        {
            if (capacity < 1 || capacity > limit)
                throw new ArgumentOutOfRangeException(capacity.ToString());

            values = new T[capacity];
            Count = 0;
            InsertPointer = 0;
            RemovePointer= -1;
            this.capacity = capacity;
        }

        public void Push(T value)
        {
            if (Count == capacity)
                throw new IndexOutOfRangeException("Queue is full!");

            values[InsertPointer] = value;
            InsertPointer = ChangePointer(InsertPointer);
            Count++;
        }
        public T Pop()
        {
            if (Count == 0)
                throw new IndexOutOfRangeException("Queue has no items!");

            RemovePointer = ChangePointer(RemovePointer);
            Count--;
            return values[RemovePointer];
        }
        public T Peek()
        {
            if (Count == 0)
                throw new IndexOutOfRangeException("Queue has no items!");

            int currentPointer = ChangePointer(RemovePointer);
            return values[currentPointer];
        }
        private int ChangePointer(int pointer)
        {
            return pointer == capacity - 1 ?  0 : pointer + 1;
        }
        public override string ToString()
        {
            return $"Capacity: {capacity} - Count: {Count}";
        }
        public IEnumerator<T> GetEnumerator()
        {
            int c = Count;
            int pointer = RemovePointer;

            while (c > 0)
            {
                pointer = ChangePointer(pointer);
                yield return values[pointer];
                c--;
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
