namespace Sakura.Framework.Dependencies
{
    using System;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class NotDiscoverable : Attribute
    {
    }
}