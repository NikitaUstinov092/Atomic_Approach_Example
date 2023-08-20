using System;
using JetBrains.Annotations;

namespace Common
{
    [MeansImplicitUse]
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class ConstructAttribute : Attribute
    {
    }
}