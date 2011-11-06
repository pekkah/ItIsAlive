namespace Sakura.Extensions.Data.Tests.DatabaseModel
{
    using System;

    public abstract class AbstractEntity
    {
        public virtual Guid Id { get; set; }
    }
}