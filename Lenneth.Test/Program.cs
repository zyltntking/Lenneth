using System;

namespace Lenneth.Test
{
    using Core;

    internal static class Program
    {
        private static void Main()
        {
            Console.WriteLine(Facade.Test);
            Facade.Logger.Error("Something Wrong!");
            Console.ReadLine();
        }
    }
}