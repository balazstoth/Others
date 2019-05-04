using System;

namespace Queue
{
    class Program
    {
        static void Main(string[] args)
        {
            Queue<int> queue = new Queue<int>(5);

            queue.Push(1);
            queue.Push(2);
            queue.Push(3);
            queue.Pop();
            queue.Pop();
            queue.Pop();
            queue.Push(4);
            queue.Push(5);
            queue.Push(6);
            queue.Push(7);
            queue.Push(8);

            foreach (var item in queue)
            {
                Console.WriteLine(item);
            }
            Console.ReadKey();
        }
    }
}
