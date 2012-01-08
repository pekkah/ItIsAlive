namespace Sakura.Composition.Discovery
{
    using System;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class NotDiscoverable : Attribute
    {
    }
}