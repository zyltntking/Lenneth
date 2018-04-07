using System;

namespace Lenneth.Core.Framework.ObjectMapper.CodeGenerators.Emitters
{
    internal interface IEmitterType : IEmitter
    {
        Type ObjectType { get; }
    }
}
