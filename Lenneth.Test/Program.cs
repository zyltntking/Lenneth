using System;
using System.Collections.Generic;

namespace Lenneth.Test
{
    using Core;

    internal static class Program
    {
        private static void Main()
        {
            Console.WriteLine(Facade.Test);
            //Facade.Mail.To(new List<string>{"zyltntking@qq.com"}).Send("title","body");
            Console.ReadLine();
        }
    }
}