namespace ItIsAlive.Extensions.NHibernate.ExtensionMethods
{
    using global::NHibernate;

    public static class SessionExtensions
    {
        public static bool IsActive(this ISession session)
        {
            return session.Transaction != null && session.Transaction.IsActive;
        }
    }
}