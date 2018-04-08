using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Lenneth.Core.Extensions
{
    [DebuggerStepThrough]
    public static class MarshalExtensions
    {
        public static byte[] StructureToArray<T>(T aStruct) where T : struct
        {
            var len = Marshal.SizeOf(typeof(T));
            var arr = new byte[len];
            var ptr = Marshal.AllocHGlobal(len);
            Marshal.StructureToPtr(aStruct, ptr, true);
            Marshal.Copy(ptr, arr, 0, len);
            Marshal.FreeHGlobal(ptr);
            return arr;
        }

        public static void ArrayToStruct<T>(byte[] aBytes, T aStruct) where T : struct
        {
            var size = Marshal.SizeOf(typeof(T));
            var ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(aStruct, ptr, true);
            Marshal.Copy(aBytes, 0, ptr, size);
            // var obj = Marshal.PtrToStructure(ptr, typeof(T));
            Marshal.FreeHGlobal(ptr);
        }

        public static byte[] StructurePtrToArray<T>(IntPtr aStruct) where T : struct
        {
            var len = Marshal.SizeOf(typeof(T));
            var arr = new byte[len];
            Marshal.Copy(aStruct, arr, 0, len);
            return arr;
        }
    }
}