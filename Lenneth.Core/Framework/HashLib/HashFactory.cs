using System;

namespace Lenneth.Core.Framework.HashLib
{
    public static class HashFactory
    {
        public static class Hash32
        {
            public static IHash CreateAp()
            {
                return new HashLib.Hash32.AP();
            }
            
            public static IHash CreateBernstein()
            {
                return new HashLib.Hash32.Bernstein();
            }
            
            public static IHash CreateBernstein1()
            {
                return new HashLib.Hash32.Bernstein1();
            }
            
            public static IHash CreateBkdr()
            {
                return new HashLib.Hash32.BKDR();
            }
            
            public static IHash CreateDek()
            {
                return new HashLib.Hash32.DEK();
            }
            
            public static IHash CreateDjb()
            {
                return new HashLib.Hash32.DJB();
            }
            
            public static IHash CreateDotNet()
            {
                return new HashLib.Hash32.DotNet();
            }
            
            public static IHash CreateElf()
            {
                return new HashLib.Hash32.ELF();
            }
            
            public static IHash CreateFnv()
            {
                return new HashLib.Hash32.FNV();
            }
            
            public static IHash CreateFnv1A()
            {
                return new HashLib.Hash32.FNV1a();
            }
            
            public static IHash CreateJenkins3()
            {
                return new HashLib.Hash32.Jenkins3();
            }
            
            public static IHash CreateJs()
            {
                return new HashLib.Hash32.JS();
            }

            public static IHashWithKey CreateMurmur2()
            {
                return new HashLib.Hash32.Murmur2();
            }

            public static IHashWithKey CreateMurmur3()
            {
                return new HashLib.Hash32.Murmur3();
            }
            
            public static IHash CreateOneAtTime()
            {
                return new HashLib.Hash32.OneAtTime();
            }
            
            public static IHash CreatePjw()
            {
                return new HashLib.Hash32.PJW();
            }
            
            public static IHash CreateRotating()
            {
                return new HashLib.Hash32.Rotating();
            }
            
            public static IHash CreateRs()
            {
                return new HashLib.Hash32.RS();
            }
            
            public static IHash CreateSdbm()
            {
                return new HashLib.Hash32.SDBM();
            }
            
            public static IHash CreateShiftAndXor()
            {
                return new HashLib.Hash32.ShiftAndXor();
            }

            public static IHash CreateSuperFast()
            {
                return new HashLib.Hash32.SuperFast();
            }
        }

        public static class Checksum
        {
            /// <summary>
            /// IEEE 802.3, polynomial = 0xEDB88320
            /// </summary>
            /// <returns></returns>
            public static IHash CreateCRC32_IEEE()
            {
                return new HashLib.Checksum.Crc32Ieee();
            }

            /// <summary>
            /// Castagnoli, polynomial = 0x82F63B78
            /// </summary>
            /// <returns></returns>
            public static IHash CreateCRC32_CASTAGNOLI()
            {
                return new HashLib.Checksum.Crc32Castagnoli();
            }

            /// <summary>
            /// Koopman, polynomial = 0xEB31D82E
            /// </summary>
            /// <returns></returns>
            public static IHash CreateCRC32_KOOPMAN()
            {
                return new HashLib.Checksum.Crc32Koopman();
            }

            /// <summary>
            /// Q, polynomial = 0xD5828281
            /// </summary>
            /// <returns></returns>
            public static IHash CreateCRC32_Q()
            {
                return new HashLib.Checksum.Crc32Q();
            }

            public static IHash CreateCrc32(uint aPolynomial, uint aInitialValue = uint.MaxValue, uint aFinalXor = uint.MaxValue)
            {
                return new HashLib.Checksum.Crc32(aPolynomial, aInitialValue, aFinalXor);
            }

            public static IHash CreateAdler32()
            {
                return new HashLib.Checksum.Adler32();
            }

            /// <summary>
            /// ECMA 182, polynomial = 0xD800000000000000
            /// </summary>
            /// <returns></returns>
            public static IHash CreateCRC64_ISO()
            {
                return new HashLib.Checksum.Crc64Iso();
            }

            /// <summary>
            /// ISO, polynomial = 0xC96C5795D7870F42
            /// </summary>
            /// <returns></returns>
            public static IHash CreateCRC64_ECMA()
            {
                return new HashLib.Checksum.Crc64Ecma();
            }

            public static IHash CreateCrc64(ulong aPolynomial, ulong aInitialValue = ulong.MaxValue, ulong aFinalXor = ulong.MaxValue)
            {
                return new HashLib.Checksum.Crc64(aPolynomial, aInitialValue, aFinalXor);
            }
        }

        public static class Hash64
        {
            public static IHash CreateFnv1A()
            {
                return new HashLib.Hash64.FNV1a64();
            }

            public static IHash CreateFnv()
            {
                return new HashLib.Hash64.FNV64();
            }

            public static IHashWithKey CreateMurmur2()
            {
                return new HashLib.Hash64.Murmur2_64();
            }

            public static IHashWithKey CreateSipHash()
            {
                return new HashLib.Hash64.SipHash();
            }
        }

        public static class Hash128
        {
            public static IHashWithKey CreateMurmur3_128()
            {
                return new HashLib.Hash128.Murmur3_128();
            }
        }

        public static class Crypto
        {
            public static class Sha3
            {
                public static IHash CreateJh224()
                {
                    return new HashLib.Crypto.SHA3.JH224();
                }

                public static IHash CreateJh256()
                {
                    return new HashLib.Crypto.SHA3.JH256();
                }

                public static IHash CreateJh384()
                {
                    return new HashLib.Crypto.SHA3.JH384();
                }

                public static IHash CreateJh512()
                {
                    return new HashLib.Crypto.SHA3.JH512();
                }

                /// <summary>
                /// 
                /// </summary>
                /// <param name="aHashSize">224, 256, 384, 512</param>
                /// <returns></returns>
                public static IHash CreateJh(HashLib.HashSize aHashSize)
                {
                    switch (aHashSize)
                    {
                        case HashLib.HashSize.HashSize224: return CreateJh224();
                        case HashLib.HashSize.HashSize256: return CreateJh256();
                        case HashLib.HashSize.HashSize384: return CreateJh384();
                        case HashLib.HashSize.HashSize512: return CreateJh512();
                        default: throw new ArgumentException();
                    }
                }

                public static IHash CreateBlake224()
                {
                    return new HashLib.Crypto.SHA3.Blake224();
                }

                public static IHash CreateBlake256()
                {
                    return new HashLib.Crypto.SHA3.Blake256();
                }

                public static IHash CreateBlake384()
                {
                    return new HashLib.Crypto.SHA3.Blake384();
                }

                public static IHash CreateBlake512()
                {
                    return new HashLib.Crypto.SHA3.Blake512();
                }

                /// <summary>
                /// 
                /// </summary>
                /// <param name="aHashSize">224, 256, 384, 512</param>
                /// <returns></returns>
                public static IHash CreateBlake(HashLib.HashSize aHashSize)
                {
                    switch (aHashSize)
                    {
                        case HashLib.HashSize.HashSize224: return CreateBlake224();
                        case HashLib.HashSize.HashSize256: return CreateBlake256();
                        case HashLib.HashSize.HashSize384: return CreateBlake384();
                        case HashLib.HashSize.HashSize512: return CreateBlake512();
                        default: throw new ArgumentException();
                    }
                    
                }

                public static IHash CreateBlueMidnightWish224()
                {
                    return new HashLib.Crypto.SHA3.BlueMidnightWish224();
                }

                public static IHash CreateBlueMidnightWish256()
                {
                    return new HashLib.Crypto.SHA3.BlueMidnightWish256();
                }

                public static IHash CreateBlueMidnightWish384()
                {
                    return new HashLib.Crypto.SHA3.BlueMidnightWish384();
                }

                public static IHash CreateBlueMidnightWish512()
                {
                    return new HashLib.Crypto.SHA3.BlueMidnightWish512();
                }

                /// <summary>
                /// 
                /// </summary>
                /// <param name="aHashSize">224, 256, 384, 512</param>
                /// <returns></returns>
                public static IHash CreateBlueMidnightWish(HashLib.HashSize aHashSize)
                {
                    switch (aHashSize)
                    {
                        case HashLib.HashSize.HashSize224: return CreateBlueMidnightWish224();
                        case HashLib.HashSize.HashSize256: return CreateBlueMidnightWish256();
                        case HashLib.HashSize.HashSize384: return CreateBlueMidnightWish384();
                        case HashLib.HashSize.HashSize512: return CreateBlueMidnightWish512();
                        default: throw new ArgumentException();
                    }
                }

                public static IHash CreateCubeHash224()
                {
                    return new HashLib.Crypto.SHA3.CubeHash224();
                }

                public static IHash CreateCubeHash256()
                {
                    return new HashLib.Crypto.SHA3.CubeHash256();
                }

                public static IHash CreateCubeHash384()
                {
                    return new HashLib.Crypto.SHA3.CubeHash384();
                }

                public static IHash CreateCubeHash512()
                {
                    return new HashLib.Crypto.SHA3.CubeHash512();
                }

                /// <summary>
                /// 
                /// </summary>
                /// <param name="aHashSize">224, 256, 384, 512</param>
                /// <returns></returns>
                public static IHash CreateCubeHash(HashLib.HashSize aHashSize)
                {
                    switch (aHashSize)
                    {
                        case HashLib.HashSize.HashSize224: return CreateCubeHash224();
                        case HashLib.HashSize.HashSize256: return CreateCubeHash256();
                        case HashLib.HashSize.HashSize384: return CreateCubeHash384();
                        case HashLib.HashSize.HashSize512: return CreateCubeHash512();
                        default: throw new ArgumentException();
                    }
                }

                public static IHash CreateEcho224()
                {
                    return new HashLib.Crypto.SHA3.Echo224();
                }

                public static IHash CreateEcho256()
                {
                    return new HashLib.Crypto.SHA3.Echo256();
                }

                public static IHash CreateEcho384()
                {
                    return new HashLib.Crypto.SHA3.Echo384();
                }

                public static IHash CreateEcho512()
                {
                    return new HashLib.Crypto.SHA3.Echo512();
                }

                /// <summary>
                /// 
                /// </summary>
                /// <param name="aHashSize">224, 256, 384, 512</param>
                /// <returns></returns>
                public static IHash CreateEcho(HashLib.HashSize aHashSize)
                {
                    switch (aHashSize)
                    {
                        case HashLib.HashSize.HashSize224: return CreateEcho224();
                        case HashLib.HashSize.HashSize256: return CreateEcho256();
                        case HashLib.HashSize.HashSize384: return CreateEcho384();
                        case HashLib.HashSize.HashSize512: return CreateEcho512();
                        default: throw new ArgumentException();
                    }
                }

                public static IHash CreateFugue224()
                {
                    return new HashLib.Crypto.SHA3.Fugue224();
                }

                public static IHash CreateFugue256()
                {
                    return new HashLib.Crypto.SHA3.Fugue256();
                }

                public static IHash CreateFugue384()
                {
                    return new HashLib.Crypto.SHA3.Fugue384();
                }

                public static IHash CreateFugue512()
                {
                    return new HashLib.Crypto.SHA3.Fugue512();
                }

                /// <summary>
                /// 
                /// </summary>
                /// <param name="aHashSize">224, 256, 384, 512</param>
                /// <returns></returns>
                public static IHash CreateFugue(HashLib.HashSize aHashSize)
                {
                    switch (aHashSize)
                    {
                        case HashLib.HashSize.HashSize224: return CreateFugue224();
                        case HashLib.HashSize.HashSize256: return CreateFugue256();
                        case HashLib.HashSize.HashSize384: return CreateFugue384();
                        case HashLib.HashSize.HashSize512: return CreateFugue512();
                        default: throw new ArgumentException();
                    }
                }

                public static IHash CreateGroestl224()
                {
                    return new HashLib.Crypto.SHA3.Groestl224();
                }

                public static IHash CreateGroestl256()
                {
                    return new HashLib.Crypto.SHA3.Groestl256();
                }

                public static IHash CreateGroestl384()
                {
                    return new HashLib.Crypto.SHA3.Groestl384();
                }

                public static IHash CreateGroestl512()
                {
                    return new HashLib.Crypto.SHA3.Groestl512();
                }

                /// <summary>
                /// 
                /// </summary>
                /// <param name="aHashSize">224, 256, 384, 512</param>
                /// <returns></returns>
                public static IHash CreateGroestl(HashLib.HashSize aHashSize)
                {
                    switch (aHashSize)
                    {
                        case HashLib.HashSize.HashSize224: return CreateGroestl224();
                        case HashLib.HashSize.HashSize256: return CreateGroestl256();
                        case HashLib.HashSize.HashSize384: return CreateGroestl384();
                        case HashLib.HashSize.HashSize512: return CreateGroestl512();
                        default: throw new ArgumentException();
                    }
                }

                public static IHash CreateHamsi224()
                {
                    return new HashLib.Crypto.SHA3.Hamsi224();
                }

                public static IHash CreateHamsi256()
                {
                    return new HashLib.Crypto.SHA3.Hamsi256();
                }

                public static IHash CreateHamsi384()
                {
                    return new HashLib.Crypto.SHA3.Hamsi384();
                }

                public static IHash CreateHamsi512()
                {
                    return new HashLib.Crypto.SHA3.Hamsi512();
                }

                /// <summary>
                /// 
                /// </summary>
                /// <param name="aHashSize">224, 256, 384, 512</param>
                /// <returns></returns>
                public static IHash CreateHamsi(HashLib.HashSize aHashSize)
                {
                    switch (aHashSize)
                    {
                        case HashLib.HashSize.HashSize224: return CreateHamsi224();
                        case HashLib.HashSize.HashSize256: return CreateHamsi256();
                        case HashLib.HashSize.HashSize384: return CreateHamsi384();
                        case HashLib.HashSize.HashSize512: return CreateHamsi512();
                        default: throw new ArgumentException();
                    }
                }

                public static IHash CreateKeccak224()
                {
                    return new HashLib.Crypto.SHA3.Keccak224();
                }

                public static IHash CreateKeccak256()
                {
                    return new HashLib.Crypto.SHA3.Keccak256();
                }

                public static IHash CreateKeccak384()
                {
                    return new HashLib.Crypto.SHA3.Keccak384();
                }

                public static IHash CreateKeccak512()
                {
                    return new HashLib.Crypto.SHA3.Keccak512();
                }

                /// <summary>
                /// 
                /// </summary>
                /// <param name="aHashSize">224, 256, 384, 512</param>
                /// <returns></returns>
                public static IHash CreateKeccak(HashLib.HashSize aHashSize)
                {
                    switch (aHashSize)
                    {
                        case HashLib.HashSize.HashSize224: return CreateKeccak224();
                        case HashLib.HashSize.HashSize256: return CreateKeccak256();
                        case HashLib.HashSize.HashSize384: return CreateKeccak384();
                        case HashLib.HashSize.HashSize512: return CreateKeccak512();
                        default: throw new ArgumentException();
                    }
                }

                public static IHash CreateLuffa224()
                {
                    return new HashLib.Crypto.SHA3.Luffa224();
                }

                public static IHash CreateLuffa256()
                {
                    return new HashLib.Crypto.SHA3.Luffa256();
                }

                public static IHash CreateLuffa384()
                {
                    return new HashLib.Crypto.SHA3.Luffa384();
                }

                public static IHash CreateLuffa512()
                {
                    return new HashLib.Crypto.SHA3.Luffa512();
                }

                /// <summary>
                /// 
                /// </summary>
                /// <param name="aHashSize">224, 256, 384, 512</param>
                /// <returns></returns>
                public static IHash CreateLuffa(HashLib.HashSize aHashSize)
                {
                    switch (aHashSize)
                    {
                        case HashLib.HashSize.HashSize224: return CreateLuffa224();
                        case HashLib.HashSize.HashSize256: return CreateLuffa256();
                        case HashLib.HashSize.HashSize384: return CreateLuffa384();
                        case HashLib.HashSize.HashSize512: return CreateLuffa512();
                        default: throw new ArgumentException();
                    }
                }

                public static IHash CreateShabal224()
                {
                    return new HashLib.Crypto.SHA3.Shabal224();
                }

                public static IHash CreateShabal256()
                {
                    return new HashLib.Crypto.SHA3.Shabal256();
                }

                public static IHash CreateShabal384()
                {
                    return new HashLib.Crypto.SHA3.Shabal384();
                }

                public static IHash CreateShabal512()
                {
                    return new HashLib.Crypto.SHA3.Shabal512();
                }

                /// <summary>
                /// 
                /// </summary>
                /// <param name="aHashSize">224, 256, 384, 512</param>
                /// <returns></returns>
                public static IHash CreateShabal(HashLib.HashSize aHashSize)
                {
                    switch (aHashSize)
                    {
                        case HashLib.HashSize.HashSize224: return CreateShabal224();
                        case HashLib.HashSize.HashSize256: return CreateShabal256();
                        case HashLib.HashSize.HashSize384: return CreateShabal384();
                        case HashLib.HashSize.HashSize512: return CreateShabal512();
                        default: throw new ArgumentException();
                    }
                }

                public static IHash CreateSHAvite3_224()
                {
                    return new HashLib.Crypto.SHA3.SHAvite3_224();
                }

                public static IHash CreateSHAvite3_256()
                {
                    return new HashLib.Crypto.SHA3.SHAvite3_256();
                }

                public static IHash CreateSHAvite3_384()
                {
                    return new HashLib.Crypto.SHA3.SHAvite3_384();
                }

                public static IHash CreateSHAvite3_512()
                {
                    return new HashLib.Crypto.SHA3.SHAvite3_512();
                }

                public static IHash CreateSHAvite3_512_Custom()
                {
                    return new HashLib.Crypto.SHA3.Custom.SHAvite3_512();
                }

                /// <summary>
                /// 
                /// </summary>
                /// <param name="aHashSize">224, 256, 384, 512</param>
                /// <returns></returns>
                public static IHash CreateShAvite3(HashLib.HashSize aHashSize)
                {
                    switch (aHashSize)
                    {
                        case HashLib.HashSize.HashSize224: return CreateSHAvite3_224();
                        case HashLib.HashSize.HashSize256: return CreateSHAvite3_256();
                        case HashLib.HashSize.HashSize384: return CreateSHAvite3_384();
                        case HashLib.HashSize.HashSize512: return CreateSHAvite3_512();
                        default: throw new ArgumentException();
                    }
                }

                public static IHash CreateSimd224()
                {
                    return new HashLib.Crypto.SHA3.SIMD224();
                }

                public static IHash CreateSimd256()
                {
                    return new HashLib.Crypto.SHA3.SIMD256();
                }

                public static IHash CreateSimd384()
                {
                    return new HashLib.Crypto.SHA3.SIMD384();
                }

                public static IHash CreateSimd512()
                {
                    return new HashLib.Crypto.SHA3.SIMD512();
                }

                /// <summary>
                /// 
                /// </summary>
                /// <param name="aHashSize">224, 256, 384, 512</param>
                /// <returns></returns>
                public static IHash CreateSimd(HashLib.HashSize aHashSize)
                {
                    switch (aHashSize)
                    {
                        case HashLib.HashSize.HashSize224: return CreateSimd224();
                        case HashLib.HashSize.HashSize256: return CreateSimd256();
                        case HashLib.HashSize.HashSize384: return CreateSimd384();
                        case HashLib.HashSize.HashSize512: return CreateSimd512();
                        default: throw new ArgumentException();
                    }
                }

                public static IHash CreateSkein224()
                {
                    return new HashLib.Crypto.SHA3.Skein224();
                }

                public static IHash CreateSkein256()
                {
                    return new HashLib.Crypto.SHA3.Skein256();
                }

                public static IHash CreateSkein384()
                {
                    return new HashLib.Crypto.SHA3.Skein384();
                }

                public static IHash CreateSkein512()
                {
                    return new HashLib.Crypto.SHA3.Skein512();
                }
                public static IHash CreateSkein512_Custom()
                {
                    return new HashLib.Crypto.SHA3.Custom.Skein512();
                }

                /// <summary>
                /// 
                /// </summary>
                /// <param name="aHashSize">224, 256, 384, 512</param>
                /// <returns></returns>
                public static IHash CreateSkein(HashLib.HashSize aHashSize)
                {
                    switch (aHashSize)
                    {
                        case HashLib.HashSize.HashSize224: return CreateSkein224();
                        case HashLib.HashSize.HashSize256: return CreateSkein256();
                        case HashLib.HashSize.HashSize384: return CreateSkein384();
                        case HashLib.HashSize.HashSize512: return CreateSkein512();
                        default: throw new ArgumentException();
                    }
                }
            }

            public static class BuildIn
            {
#if !NETCORE
                public static IHash CreateMd5CryptoServiceProvider()
                {
                    return new HashLib.Crypto.BuildIn.Md5CryptoServiceProvider();
                }

                public static IHash CreateRipemd160Managed()
                {
                    return new HashLib.Crypto.BuildIn.Ripemd160Managed();
                }

                public static IHash CreateSha1Cng()
                {
                    return new HashLib.Crypto.BuildIn.Sha1Cng();
                }

                public static IHash CreateSha1CryptoServiceProvider()
                {
                    return new HashLib.Crypto.BuildIn.Sha1CryptoServiceProvider();
                }

                public static IHash CreateSha1Managed()
                {
                    return new HashLib.Crypto.BuildIn.Sha1Managed();
                }

                public static IHash CreateSha256Cng()
                {
                    return new HashLib.Crypto.BuildIn.Sha256Cng();
                }

                public static IHash CreateSha256CryptoServiceProvider()
                {
                    return new HashLib.Crypto.BuildIn.Sha256CryptoServiceProvider();
                }	

                public static IHash CreateSha256Managed()
                {
                    return new HashLib.Crypto.BuildIn.Sha256Managed();
                }

                public static IHash CreateSha384Cng()
                {
                    return new HashLib.Crypto.BuildIn.Sha384Cng();
                }

                public static IHash CreateSha384CryptoServiceProvider()
                {
                    return new HashLib.Crypto.BuildIn.Sha384CryptoServiceProvider();
                }

                public static IHash CreateSha384Managed()
                {
                    return new HashLib.Crypto.BuildIn.Sha384Managed();
                }

                public static IHash CreateSha512Cng()
                {
                    return new HashLib.Crypto.BuildIn.Sha512Cng();
                }

                public static IHash CreateSha512CryptoServiceProvider()
                {
                    return new HashLib.Crypto.BuildIn.Sha512CryptoServiceProvider();
                }

                public static IHash CreateSha512Managed()
                {
                    return new HashLib.Crypto.BuildIn.Sha512Managed();
                }
#endif
            }

            public static IHash CreateGost()
            {
                return new HashLib.Crypto.Gost();
            }

            public static IHash CreateGrindahl256()
            {
                return new HashLib.Crypto.Grindahl256();
            }

            public static IHash CreateGrindahl512()
            {
                return new HashLib.Crypto.Grindahl512();
            }

            public static IHash CreateHas160()
            {
                return new HashLib.Crypto.HAS160();
            }

            public static IHash CreateHaval_3_128()
            {
                return new HashLib.Crypto.Haval_3_128();
            }

            public static IHash CreateHaval_4_128()
            {
                return new HashLib.Crypto.Haval_4_128();
            }

            public static IHash CreateHaval_5_128()
            {
                return new HashLib.Crypto.Haval_5_128();
            }

            public static IHash CreateHaval_3_160()
            {
                return new HashLib.Crypto.Haval_3_160();
            }

            public static IHash CreateHaval_4_160()
            {
                return new HashLib.Crypto.Haval_4_160();
            }

            public static IHash CreateHaval_5_160()
            {
                return new HashLib.Crypto.Haval_5_160();
            }

            public static IHash CreateHaval_3_192()
            {
                return new HashLib.Crypto.Haval_3_192();
            }

            public static IHash CreateHaval_4_192()
            {
                return new HashLib.Crypto.Haval_4_192();
            }

            public static IHash CreateHaval_5_192()
            {
                return new HashLib.Crypto.Haval_5_192();
            }

            public static IHash CreateHaval_3_224()
            {
                return new HashLib.Crypto.Haval_3_224();
            }

            public static IHash CreateHaval_4_224()
            {
                return new HashLib.Crypto.Haval_4_224();
            }

            public static IHash CreateHaval_5_224()
            {
                return new HashLib.Crypto.Haval_5_224();
            }

            public static IHash CreateHaval_3_256()
            {
                return new HashLib.Crypto.Haval_3_256();
            }

            public static IHash CreateHaval_4_256()
            {
                return new HashLib.Crypto.Haval_4_256();
            }

            public static IHash CreateHaval_5_256()
            {
                return new HashLib.Crypto.Haval_5_256();
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="aRounds">3, 4, 5</param>
            /// <param name="aHashSize">128, 160, 192, 224, 256</param>
            /// <returns></returns>
            public static IHash CreateHaval(HashRounds aRounds, HashLib.HashSize aHashSize)
            {
                switch (aRounds)
                {
                    case HashRounds.Rounds3:

                        switch (aHashSize)
                        {
                            case HashLib.HashSize.HashSize128: return CreateHaval_3_128();
                            case HashLib.HashSize.HashSize160: return CreateHaval_3_160();
                            case HashLib.HashSize.HashSize192: return CreateHaval_3_192();
                            case HashLib.HashSize.HashSize224: return CreateHaval_3_224();
                            case HashLib.HashSize.HashSize256: return CreateHaval_3_256();
                            default: throw new ArgumentException();
                        }

                    case HashRounds.Rounds4:

                        switch (aHashSize)
                        {
                            case HashLib.HashSize.HashSize128: return CreateHaval_4_128();
                            case HashLib.HashSize.HashSize160: return CreateHaval_4_160();
                            case HashLib.HashSize.HashSize192: return CreateHaval_4_192();
                            case HashLib.HashSize.HashSize224: return CreateHaval_4_224();
                            case HashLib.HashSize.HashSize256: return CreateHaval_4_256();
                            default: throw new ArgumentException();
                        }

                    case HashRounds.Rounds5:

                        switch (aHashSize)
                        {
                            case HashLib.HashSize.HashSize128: return CreateHaval_5_128();
                            case HashLib.HashSize.HashSize160: return CreateHaval_5_160();
                            case HashLib.HashSize.HashSize192: return CreateHaval_5_192();
                            case HashLib.HashSize.HashSize224: return CreateHaval_5_224();
                            case HashLib.HashSize.HashSize256: return CreateHaval_5_256();
                            default: throw new ArgumentException();
                        }

                    default: throw new ArgumentException();
                }
            }

            public static IHash CreateMd2()
            {
                return new HashLib.Crypto.MD2();
            }

            public static IHash CreateMd4()
            {
                return new HashLib.Crypto.MD4();
            }

            public static IHash CreateMd5()
            {
                return new HashLib.Crypto.Md5();
            }

            public static IHash CreatePanama()
            {
                return new HashLib.Crypto.Panama();
            }

            public static IHash CreateRadioGatun32()
            {
                return new HashLib.Crypto.RadioGatun32();
            }

            public static IHash CreateRadioGatun64()
            {
                return new HashLib.Crypto.RadioGatun64();
            }

            public static IHash CreateRipemd()
            {
                return new HashLib.Crypto.RIPEMD();
            }

            public static IHash CreateRipemd128()
            {
                return new HashLib.Crypto.RIPEMD128();
            }

            public static IHash CreateRipemd160()
            {
                return new HashLib.Crypto.RIPEMD160();
            }

            public static IHash CreateRipemd256()
            {
                return new HashLib.Crypto.RIPEMD256();
            }

            public static IHash CreateRipemd320()
            {
                return new HashLib.Crypto.RIPEMD320();
            }

            public static IHash CreateSha0()
            {
                return new HashLib.Crypto.SHA0();
            }

            public static IHash CreateSha1()
            {
                return new HashLib.Crypto.SHA1();
            }

            public static IHash CreateSha224()
            {
                return new HashLib.Crypto.SHA224();
            }

            public static IHash CreateSha256()
            {
                return new HashLib.Crypto.SHA256();
            }

            public static IHash CreateSha384()
            {
                return new HashLib.Crypto.SHA384();
            }

            public static IHash CreateSha512()
            {
                return new HashLib.Crypto.SHA512();
            }

            public static IHash CreateSnefru_4_128()
            {
                return new HashLib.Crypto.Snefru_4_128();
            }

            public static IHash CreateSnefru_4_256()
            {
                return new HashLib.Crypto.Snefru_4_256();
            }

            public static IHash CreateSnefru_8_128()
            {
                return new HashLib.Crypto.Snefru_8_128();
            }

            public static IHash CreateSnefru_8_256()
            {
                return new HashLib.Crypto.Snefru_8_256();
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="aRounds">4, 8</param>
            /// <param name="aHashSize">128, 256</param>
            /// <returns></returns>
            public static IHash CreateSnefru(HashRounds aRounds, HashLib.HashSize aHashSize)
            {
                switch (aRounds)
                {
                    case HashRounds.Rounds4:

                        switch (aHashSize)
                        {
                            case HashLib.HashSize.HashSize128: return CreateSnefru_4_128();
                            case HashLib.HashSize.HashSize256: return CreateSnefru_4_256();
                            default: throw new ArgumentException();
                        }

                    case HashRounds.Rounds8:

                        switch (aHashSize)
                        {
                            case HashLib.HashSize.HashSize128: return CreateSnefru_8_128();
                            case HashLib.HashSize.HashSize256: return CreateSnefru_8_256();
                            default: throw new ArgumentException();
                        }

                    default: throw new ArgumentException();
                }
            }

            public static IHash CreateTiger_3_192()
            {
                return new HashLib.Crypto.Tiger_3_192();
            }

            public static IHash CreateTiger_4_192()
            {
                return new HashLib.Crypto.Tiger_4_192();
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="aRounds">3, 4</param>
            /// <returns></returns>
            public static IHash CreateTiger(HashRounds aRounds)
            {
                switch (aRounds)
                {
                    case HashRounds.Rounds3: return CreateTiger_3_192();
                    case HashRounds.Rounds4: return CreateTiger_4_192();
                    default: throw new ArgumentException();
                }
            }

            public static IHash CreateTiger2()
            {
                return new HashLib.Crypto.Tiger2();
            }

            public static IHash CreateWhirlpool()
            {
                return new HashLib.Crypto.Whirlpool();
            }
        }

        public static class Hmac
        {
            public static IHMAC CreateHmac(IHash aHash)
            {
                if (aHash is IHMAC)
                {
                    return (IHMAC)aHash;
                }
#if !USEBC
				else if (aHash is IHasHMACBuildIn)
                {
					IHasHMACBuildIn h = (IHasHMACBuildIn)aHash;
                    return new HMACBuildInAdapter(h.GetBuildHMAC(), h.BlockSize);
				}
#endif
				else
				{
                    return new HMACNotBuildInAdapter(aHash);
                }
            }
        }

#if !USEBC

        public static class Wrappers
        {
            public static System.Security.Cryptography.HashAlgorithm HashToHashAlgorithm(IHash aHash)
            {
                return new HashAlgorithmWrapper(aHash);
            }

            public static IHash HashAlgorithmToHash(System.Security.Cryptography.HashAlgorithm aHash, 
                int aBlockSize = -1)
            {
                return new HashCryptoBuildIn(aHash, aBlockSize);
            }
        }
#endif
	}
}
