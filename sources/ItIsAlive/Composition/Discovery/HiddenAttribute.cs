namespace ItIsAlive.Composition.Discovery
{
    using System;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public sealed class HiddenAttribute : Attribute
    {
    }
}