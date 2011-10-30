namespace Sakura.Extensions.Data.Tests.DatabaseModel
{
    using Sakura.Extensions.Data.Model;

    public class Person : AbstractEntity
    {
        public virtual string Name
        {
            get;
            set;
        }
    }
}