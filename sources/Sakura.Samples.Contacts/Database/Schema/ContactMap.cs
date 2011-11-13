namespace Sakura.Framework.Samples.Contacts.Database.Schema
{
    using Sakura.Framework.Samples.Contacts.Database.Entities;

    public class ContactMap : AbstractEntityMap<Contact>
    {
        public ContactMap()
        {
            this.Property(x => x.Name, pm => pm.Length(100));
        }
    }
}