namespace Sakura.Framework.Dependencies.Discovery
{
    using System;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class NotDiscoverable : Attribute
    {
    }
}