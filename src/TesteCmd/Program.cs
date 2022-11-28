using System;

namespace TesteCmd
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Console.WriteLine(args.Length);
            foreach (var item in args)
            {
                Console.WriteLine(item);
            }
        }
    }
}
