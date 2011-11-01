namespace Sakura.Framework.Samples.ContactsWeb.Database
{
    using Sakura.Extensions.Data.Model;

    public class Contact : AbstractEntity
    {
        public string Name
        {
            get;
            set;
        }
    }
}