using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace Lenneth.Core.Framework.HashLib
{
    public static class Hashes
    {
        public static readonly ReadOnlyCollection<Type> All;
        public static readonly ReadOnlyCollection<Type> AllUnique;
        public static readonly ReadOnlyCollection<Type> Hash32;
        public static readonly ReadOnlyCollection<Type> Hash64;
        public static readonly ReadOnlyCollection<Type> Hash128;
        public static readonly ReadOnlyCollection<Type> CryptoAll;
        public static readonly ReadOnlyCollection<Type> CryptoNotBuildIn;
        public static readonly ReadOnlyCollection<Type> CryptoBuildIn;
        public static readonly ReadOnlyCollection<Type> HasHmacBuildIn;

        public static readonly ReadOnlyCollection<Type> NonBlock;
        public static readonly ReadOnlyCollection<Type> FastComputes;
        public static readonly ReadOnlyCollection<Type> Checksums;
        public static readonly ReadOnlyCollection<Type> WithKey;

        static Hashes()
        {
#if !NETCORE
			All = (from hf in Assembly.GetAssembly(typeof(IHash)).GetTypes()
                   where hf.IsClass
                   where !hf.IsAbstract
                   where hf != typeof(HMACNotBuildInAdapter)
                   where hf != typeof(HashCryptoBuildIn)
                   where hf != typeof(HMACBuildInAdapter)
                   where hf.IsImplementInterface(typeof(IHash))
                   where !hf.IsNested
                   select hf).ToList().AsReadOnly();

            All = (from hf in All
                   orderby hf.Name
                   select hf).ToList().AsReadOnly();

            var x2 = new[] 
            {

                typeof(Crypto.BuildIn.Sha1Cng), 
                typeof(Crypto.BuildIn.Sha1Managed), 
                typeof(Crypto.BuildIn.Sha256Cng), 
                typeof(Crypto.BuildIn.Sha256Managed), 
                typeof(Crypto.BuildIn.Sha384Cng), 
                typeof(Crypto.BuildIn.Sha384Managed), 
                typeof(Crypto.BuildIn.Sha512Cng), 
                typeof(Crypto.BuildIn.Sha512Managed), 
			typeof(Crypto.Md5),
                typeof(Crypto.RIPEMD160),
                typeof(Crypto.SHA1),
                typeof(Crypto.SHA256),
                typeof(Crypto.SHA384),
                typeof(Crypto.SHA512),
            };

            AllUnique = (from hf in All
                         where !(hf.IsDerivedFrom(typeof(Hash32.DotNet)))
                         where !x2.Contains(hf)
                         where !hf.IsNested
                         select hf).ToList().AsReadOnly();

            Hash32 = (from hf in All
                      where hf.IsImplementInterface(typeof(IHash32))
                      where !hf.IsImplementInterface(typeof(IChecksum))
                      select hf).ToList().AsReadOnly();

            Hash64 = (from hf in All
                      where hf.IsImplementInterface(typeof(IHash64))
                      where !hf.IsImplementInterface(typeof(IChecksum))
                      select hf).ToList().AsReadOnly();

            Hash128 = (from hf in All
                       where hf.IsImplementInterface(typeof(IHash128))
                       where !hf.IsImplementInterface(typeof(IChecksum))
                       select hf).ToList().AsReadOnly();

            Checksums = (from hf in All
                         where hf.IsImplementInterface(typeof(IChecksum))
                         select hf).ToList().AsReadOnly();

            FastComputes = (from hf in All
                            where hf.IsImplementInterface(typeof(IFastHash32))
                            select hf).ToList().AsReadOnly();

            NonBlock = (from hf in All
                        where hf.IsImplementInterface(typeof(INonBlockHash))
                        select hf).ToList().AsReadOnly();

            WithKey = (from hf in All
                       where hf.IsImplementInterface(typeof(IWithKey))
                       select hf).ToList().AsReadOnly();

            CryptoAll = (from hf in All
                         where hf.IsImplementInterface(typeof(ICrypto))
                         select hf).ToList().AsReadOnly();

            CryptoNotBuildIn = (from hf in CryptoAll
                                where hf.IsImplementInterface(typeof(ICryptoNotBuildIn))
                                select hf).ToList().AsReadOnly();

            CryptoBuildIn = (from hf in CryptoAll
                             where hf.IsImplementInterface(typeof(ICryptoBuildIn))
                             select hf).ToList().AsReadOnly();

            HasHmacBuildIn = (from hf in CryptoBuildIn
                              where hf.IsImplementInterface(typeof(IHasHMACBuildIn))
                              select hf).ToList().AsReadOnly();
#endif

		}
	}
}
