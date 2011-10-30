namespace Sakura.Extensions.Data.Model
{
    public abstract class AbstractEntity
    {
        public virtual int Id { get; set; }

        public virtual int Version { get; set; }
    }
}