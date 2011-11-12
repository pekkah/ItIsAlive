namespace Sakura.Extensions.Data
{
    using System;
    using System.Linq.Expressions;

    using NHibernate;

    public class WorkContext : IWorkContext
    {
        private readonly ISession session;

        public WorkContext(ISession session)
        {
            this.session = session;
        }

        public TEntity Get<TEntity, TId>(TId id)
        {
            return this.session.Get<TEntity>(id);
        }

        public TEntity Load<TEntity, TId>(TId id)
        {
            return this.session.Load<TEntity>(id);
        }

        public IQueryOver<T, T> QueryOver<T>() where T : class
        {
            return this.session.QueryOver<T>();
        }

        public IQueryOver<T, T> QueryOver<T>(Expression<Func<T>> alias) where T : class
        {
            return this.session.QueryOver<T>(alias);
        }

        public IQueryOver<T, T> QueryOver<T>(string entityName) where T : class
        {
            return this.session.QueryOver<T>(entityName);
        }

        public IQueryOver<T, T> QueryOver<T>(string entityName, Expression<Func<T>> alias) where T : class
        {
            return this.session.QueryOver<T>(entityName, alias);
        }

        public void RollbackChanges()
        {
            if (this.session.Transaction == null)
            {
                throw new InvalidOperationException("Current transaction is not set.");
            }

            if (!this.session.Transaction.IsActive)
            {
                throw new InvalidOperationException("Current transaction is not active.");
            }

            this.session.Transaction.Rollback();
        }

        public void Save<TEntity>(TEntity entity)
        {
            this.session.Save(entity);
        }

        public void Update<TEntity>(TEntity entity)
        {
            this.session.Save(entity);
        }
    }
}