namespace Sakura.Samples.Contacts.Database.Schema
{
    using NHibernate.Mapping.ByCode;
    using NHibernate.Mapping.ByCode.Conformist;

    using Sakura.Samples.Contacts.Database.Entities;

    public abstract class AbstractEntityMap<T> : ClassMapping<T>
        where T : AbstractEntity
    {
        protected AbstractEntityMap()
        {
            this.Id(x => x.Id, map => map.Generator(Generators.GuidComb));
        }
    }
}