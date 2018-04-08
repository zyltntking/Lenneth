﻿#if !NETCORE

using System;

namespace Lenneth.Core.Framework.HashLib.Crypto.BuildIn
{
    internal class SHA512CryptoServiceProvider : HashCryptoBuildIn
    {
        public SHA512CryptoServiceProvider() 
            : base(new System.Security.Cryptography.SHA512CryptoServiceProvider(), 128)
        {
        }
    }
}
#endif