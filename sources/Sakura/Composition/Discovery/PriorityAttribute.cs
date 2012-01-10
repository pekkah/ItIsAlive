namespace Sakura.Composition.Discovery
{
    using System;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public sealed class PriorityAttribute : Attribute, IPriorityMetadata
    {
        public int Priority { get; set; }
    }
}