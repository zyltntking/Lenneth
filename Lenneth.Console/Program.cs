using Lenneth.Core.Framework.BlockChain;
using Newtonsoft.Json;

namespace Lenneth.Console
{
    internal static class Program
    {
        private static void Main()
        {
            System.Console.WriteLine("The Init Block Is:");
            var block = new Block<string>(0,null);
            Generator<string>.BlockChain.Add(block);
            System.Console.WriteLine(JsonConvert.SerializeObject(block));
            System.Console.WriteLine("Now Generator Next Block:");
            var nextBlock = Generator<string>.GeneratorBlock(block);
            System.Console.WriteLine($"Vali It: The Result is {Generator<string>.IsBlockValid(block,nextBlock)}");
            Generator<string>.BlockChain.Add(nextBlock);
            System.Console.WriteLine(JsonConvert.SerializeObject(nextBlock));
            System.Console.WriteLine("Now BlockChain Contains:");
            System.Console.WriteLine(JsonConvert.SerializeObject(Generator<string>.BlockChain));
            System.Console.ReadLine();
        }
    }
}