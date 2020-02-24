#region

using System;
using System.Collections.Generic;
//using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DemoRandomUser.Data;
using DemoRandomUser.Model;


#endregion

namespace DemoRandomUser.Repository
{
    public abstract class ServiceBase<T>
        where T : class, IModel, IUpdatable, IDeletable
    {
        protected ServiceBase(IDbContext db)
        {
            Db = db;
        }

        protected IDbContext Db { get; }


        public IEnumerable<T> GetList()
        {
            return Db.Repository<T>().ToList();
        }

        public IEnumerable<T> GetList(Expression<Func<T, bool>> predicate)
        {
            var query = Db.Repository<T>();

            query = query.Where(predicate);

            return query.ToList();
        }

        public IEnumerable<TResult> GetList<TResult>(Expression<Func<T, TResult>> select, Expression<Func<T, bool>> predicate)
        {
            var query = Db.Repository<T>().Where(predicate).Select(select);
            
            return query.ToList();
        }

        //public async Task<IEnumerable<TResult>> GetListAsync<TResult>(Expression<Func<T, TResult>> select, Expression<Func<T, bool>> predicate)
        //{
        //    var query = Db.Repository<T>().Where(predicate).Select(select);

        //    return await query.ToListAsync().ConfigureAwait(false);
        //}

        public IEnumerable<TResult> GetDistinctList<TResult>(Expression<Func<T, TResult>> select, Expression<Func<T, bool>> predicate)
        {
            var query = Db.Repository<T>().Where(predicate).Select(select).Distinct();
            
            return query.ToList();
        }

        public int Count(Expression<Func<T, bool>> predicate)
        {
            var query = Db.Repository<T>().Where(predicate);

            return query.Count();
        }

        //public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        //{
        //    var query = Db.Repository<T>().Where(predicate);

        //    return await query.CountAsync().ConfigureAwait(false);
        //}

        public T Get(Expression<Func<T, bool>> predicate)
        {
            var row = Db.Repository<T>().FirstOrDefault(predicate);

            return row;
        }

        public T GetSingleOrDefault(Expression<Func<T, bool>> predicate)
        {
            return Db.Repository<T>().SingleOrDefault(predicate);
        }

        public T GetFirst(Expression<Func<T, bool>> predicate)
        {
            return Db.Repository<T>().First(predicate);
        }

        public T GetFirstOrDefault()
        {
            var query = Db.Repository<T>();
            
            return query.FirstOrDefault();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            var query = Db.Repository<T>();

            query = query.Where(predicate);

            return query.FirstOrDefault();
        }

        public bool Exist(Expression<Func<T, bool>> predicate)
        {
            var query = Db.Repository<T>();

            query = query.Where(predicate);

            return query.Any();
        }

        public int Add(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            Db.Add(entity);
            Db.Save();
            return entity.ObjectId;
        }

        //public int AddFromSource(T entity)
        //{
        //    if (entity == null)
        //        throw new ArgumentNullException(nameof(entity));

        //    Db.AddFromSource(entity);
        //    Db.Save();

        //    return entity.ObjectId;
        //}

        public void Update(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            Db.Update(entity);
            Db.Save();

        }

        public void UpdateNoTrack(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            Db.UpdateNoTrack(entity);
            Db.Save();

        }

        //public void UpdateFromSource(T entity)
        //{
        //    if (entity == null)
        //        throw new ArgumentNullException(nameof(entity));
            
        //    Db.UpdateFromSource(entity);
        //    Db.Save();

        //}

        public void Delete(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            Db.Delete(entity);
            Db.Save();

        }

        public int AddOrUpdate(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var row = Db.Find<T>(entity.ObjectId);

            if (row == null)
            {
                Db.Add(entity);
                Db.Save();

            }
            else
            {
                Db.Update(entity);
                Db.Save();

            }

            return entity.ObjectId;
        }

        //public int AddOrUpdateFromSource(T entity)
        //{
        //    if (entity == null)
        //        throw new ArgumentNullException(nameof(entity));

        //    var row = Db.Find<T>(entity.ObjectId);

        //    if (row == null)
        //    {
        //        Db.AddFromSource(entity);
        //        Db.Save();

        //    }
        //    else
        //    {
        //        Db.UpdateFromSource(entity);
        //        Db.Save();

        //    }

        //    return entity.ObjectId;
        //}
    }
}