#if !NETCORE

using System;

namespace Lenneth.Core.Framework.HashLib.Crypto.BuildIn
{
    internal class SHA384Cng : HashCryptoBuildIn
    {
        public SHA384Cng() 
            : base(new System.Security.Cryptography.SHA384Cng(), 128)
        {
        }
    }
}
#endif