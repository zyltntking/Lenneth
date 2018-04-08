namespace Lenneth.Core.Framework.HashLib.Crypto.BuildIn
{
    internal class Sha384Cng : HashCryptoBuildIn
    {
        public Sha384Cng() 
            : base(new System.Security.Cryptography.SHA384Cng(), 128)
        {
        }
    }
}