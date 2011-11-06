namespace Sakura.Framework.Samples.Contacts.Database.Schema
{
    using NHibernate.Mapping.ByCode;

    using Sakura.Framework.Samples.Contacts.Database.Entities;

    public class UserMap : AbstractEntityMap<User>
    {
        public UserMap()
        {
            this.Property(x => x.Name, pm => pm.Length(100));
            this.Property(x => x.Password, pm => pm.Length(100));
            this.Property(x => x.Email, pm => pm.Length(100));
            this.Bag(
                x => x.Contacts,
                cm =>
                {
                    cm.Access(Accessor.Field);
                    cm.Cascade(Cascade.All);
                    cm.Key(map =>
                        {
                            map.Column("UserId");
                            map.ForeignKey("FK_UserContacts");
                        });
                },
                r => r.OneToMany());
        }
    }
}