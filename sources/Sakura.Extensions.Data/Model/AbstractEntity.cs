namespace Sakura.Extensions.Data.Model
{
    using System;

    public abstract class AbstractEntity
    {
        public virtual Guid Id { get; set; }

        public virtual int Version { get; set; }
    }
}