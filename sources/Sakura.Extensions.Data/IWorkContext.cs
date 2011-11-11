namespace Sakura.Extensions.Data
{
    using System;
    using System.Linq.Expressions;

    using NHibernate;

    public interface IWorkContext
    {
        TEntity Get<TEntity, TId>(TId id);

        TEntity Load<TEntity, TId>(TId id);

        IQueryOver<T, T> QueryOver<T>() where T : class;

        IQueryOver<T, T> QueryOver<T>(Expression<Func<T>> alias) where T : class;

        IQueryOver<T, T> QueryOver<T>(string entityName) where T : class;

        IQueryOver<T, T> QueryOver<T>(string entityName, Expression<Func<T>> alias) where T : class;

        void RollbackChanges();

        void Save<TEntity>(TEntity entity);

        void Update<TEntity>(TEntity entity);

        void CommitChanges();
    }
}