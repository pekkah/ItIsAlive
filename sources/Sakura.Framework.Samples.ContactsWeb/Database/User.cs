namespace Sakura.Framework.Samples.ContactsWeb.Database
{
    using Sakura.Extensions.Data.Model;

    public class User : AbstractEntity
    {
        public virtual string Name
        {
            get;
            set;
        }

        public virtual string Password
        {
            get;
            set;
        }

        public virtual string Email
        {
            get;
            set;
        }
    }
}