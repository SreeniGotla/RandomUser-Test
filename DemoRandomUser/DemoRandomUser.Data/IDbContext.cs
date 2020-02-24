#region

using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using DemoRandomUser.Model;

#endregion

namespace DemoRandomUser.Data
{
    public interface IDbContext
    {
        IQueryable<T> Repository<T>() where T : class, IModel;
        IQueryable<T> Repository<T>(Expression<Func<T, object>> includeProperties) where T : class, IModel;
        void Add<T>(T entity) where T : class, IModel;
        void Update<T>(T entity) where T : class, IModel;
        void Delete<T>(T entity) where T : class, IModel;
        void Commit();
    }
}