#if !NETCORE

using System;

namespace Lenneth.Core.Framework.HashLib.Crypto.BuildIn
{
    internal class SHA384CryptoServiceProvider : HashCryptoBuildIn
    {
        public SHA384CryptoServiceProvider() 
            : base(new System.Security.Cryptography.SHA384CryptoServiceProvider(), 128)
        {
        }
    }
}
#endif