using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stack
{
    class Program
    {
        static void Main(string[] args)
        {
            Stack<int> stack = new Stack<int>(5);

            for(int i = 0; i < 5; i++)
                stack.Enqueue(i + 1);

            foreach (var item in stack)
                Console.WriteLine(item);

            Console.ReadKey();
        }
    }
}
