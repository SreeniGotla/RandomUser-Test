#region

using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Linq.Expressions;
using DemoRandomUser.Model;
using DemoRandomUser.Common;
using RandomUser.Model;

#endregion

namespace DemoRandomUser.Data
{
    public class SqlDb : DbContext, IDbContext
    {
        //private readonly IUser _user;

        public SqlDb() : base("SqlDb")
        {
            Database.SetInitializer<SqlDb>(null);

            Configuration.LazyLoadingEnabled = false;
            Configuration.ValidateOnSaveEnabled = false;
        }

        public IQueryable<T> Repository<T>() where T : class, IModel
        {
            return Set<T>().AsNoTracking();
        }

        public void Add<T>(T entity) where T : class, IModel
        {
            var model = entity as AbstractModel;

            if (model != null)
            {
                model.CreatedOn = DateTime.UtcNow;
                model.LastUpdatedOn = DateTime.UtcNow;

                model.CreatedBy = "admin";//_user.Email;
                model.LastUpdatedBy = "admin";//_user.Email;
            }

            Set<T>().Add(entity);
        }

        public void Update<T>(T entity) where T : class, IModel
        {
            var original = Set<T>().Find(entity.ObjectId);

            Ensure.NotNull(original);

            var entry = Entry(original);

            var model = entity as AbstractModel;
            var current = original as AbstractModel;


            if (model != null && current != null)
            {
                model.CreatedBy = current.CreatedBy;
                model.CreatedOn = current.CreatedOn;
                model.LastUpdatedOn = DateTime.UtcNow;
                model.LastUpdatedBy = "admin";//_user.Email;
            }

            entry.CurrentValues.SetValues(entity);
        }
        public void Delete<T>(T entity) where T : class, IModel
        {
            var local = Set<T>()
                       .Local
                       .FirstOrDefault(f => f.ObjectId == entity.ObjectId);

            if (local != null)
                 Set<T>().Remove(local);

            var entry = Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                Set<T>().Attach(entity);
            }

            Set<T>().Remove(entity);
        }

        public void Commit()
        {
            SaveChanges();
        }

        /// <summary>
        ///     Simple Navigation: i => new { i.NavigationProperty }
        ///     Collection Navigation: i => new { i.CollectionEntity, j = i.CollectionEntity.GetPropertyLevel(p => p.Property) }
        ///     Where CollectionEntity is an ICollection of T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        public IQueryable<T> Repository<T>(Expression<Func<T, object>> includeProperties) where T : class, IModel
        {
            var query = IncludeNavigation(includeProperties.Body, Set<T>());

            return query.AsNoTracking();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            base.OnModelCreating(modelBuilder);
        }

        private static IQueryable<T> IncludeNavigation<T>(Expression e, DbQuery<T> query) where T : class
        {
            var ne = e as NewExpression;

            if (ne == null)
                throw new InvalidOperationException("Anonymous type is required.");

            foreach (var args in ne.Arguments)
            {
                var pi = string.Empty;
                var me = args as MemberExpression;
                var mc = args as MethodCallExpression;

                if (mc != null)
                {
                    var npe = mc.Arguments[0] as MemberExpression;
                    var nue = mc.Arguments[1] as UnaryExpression;

                    if (npe == null)
                        throw new InvalidOperationException();

                    if (nue == null)
                        throw new InvalidOperationException();

                    var nueProp = ((MemberExpression) ((LambdaExpression) nue.Operand).Body).Member.Name;

                    query = query.Include($"{npe.Member.Name}.{nueProp}");

                    continue;
                }

                if (me == null)
                    throw new NotSupportedException();

                var pe = me.Expression as MemberExpression;

                if (pe != null)
                    pi = GetPropertyLevel(pe, pi);

                var m = me;

                query = query.Include(!string.IsNullOrWhiteSpace(pi)
                    ? $"{pi}{m.Member.Name}"
                    : m.Member.Name);
            }

            return query;
        }

        private static string GetPropertyLevel(MemberExpression pe, string pi)
        {
            var property = $"{pi}{pe.Member.Name}.";
            var cpe = pe.Expression as MemberExpression;

            if (cpe != null)
                property = GetPropertyLevel(cpe, property);

            return property;
        }

        #region Persisted Sets

        public DbSet<User> User { get; set; }
        #endregion
    }
}