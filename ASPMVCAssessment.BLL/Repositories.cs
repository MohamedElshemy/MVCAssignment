using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ASPMVCAssessment.BLL
{
    public abstract class Repositories<T> : IRepositories<T> where T : class
    {

        protected IDbSet<T> DbSet;
        protected DbContext Dbcontext;
        internal Repositories<T> DataDa;

        #region Ctor

        protected Repositories(DbContext context)
        {
            Dbcontext = context;
            DbSet = context.Set<T>();
        }

        public Repositories()
            : this(new DbContext(ConfigurationManager.ConnectionStrings["AssessmentEntities"].ConnectionString))
        {
        }
        //protected Repositories()
        //    : this(new Entities2())
        //{
        //}
        #endregion

        #region IRepository<T> Members

        public DbContext Context => Dbcontext;

        public virtual int Add(T entity)
        {
            Dbcontext.Set<T>().Add(entity);
            return Save();

        }

        public virtual int Update(T entity)
        {
            try
            {
                var fqen = GetEntityName<T>();

                object originalItem;
                System.Data.Entity.Core.EntityKey key = ((IObjectContextAdapter)Dbcontext).ObjectContext.CreateEntityKey(fqen, entity);
                //_context.Entry(entity).State = EntityState.Modified;
                if (((IObjectContextAdapter)Dbcontext).ObjectContext.TryGetObjectByKey(key, out originalItem))
                {
                    ((IObjectContextAdapter)Dbcontext).ObjectContext.ApplyCurrentValues(key.EntitySetName, entity);
                }

                //_context.Set<T>().Attach(entity);
                //_context.Entry(entity).State = EntityState.Modified;
                return Save();
            }
            catch (Exception e)
            {
                throw;
            }

        }

        public virtual int Delete(T entity)
        {
            try
            {
                var fqen = GetEntityName<T>();

                object originalItem;
                System.Data.Entity.Core.EntityKey key = ((IObjectContextAdapter)Dbcontext).ObjectContext.CreateEntityKey(fqen, entity);
                if (((IObjectContextAdapter)Dbcontext).ObjectContext.TryGetObjectByKey(key, out originalItem))
                {
                    ((IObjectContextAdapter)Dbcontext).ObjectContext.DeleteObject(originalItem);
                }
                //_context.Set<T>().Attach(entity);
                //_context.Entry(entity).State = EntityState.Deleted;
                return Save();
            }
            catch (Exception e)
            {
                throw;
            }

        }

        public int Save()
        {
            try
            {
                return Dbcontext.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public virtual IEnumerable<T> Items
        {
            get { return Dbcontext.Set<T>().AsNoTracking(); }
        }
        public virtual IEnumerable<T> GetAll(List<string> includes)
        {
            return Dbcontext.Set<T>().AsNoTracking().WithIncludes(includes);
        }
        public virtual IQueryable<T> GetAll()
        {
            return Dbcontext.Set<T>().AsNoTracking();
        }
        public virtual IEnumerable<T> SearchBy(Expression<Func<T, bool>> predicate)
        {
            return Dbcontext.Set<T>().AsNoTracking().Where(predicate);
        }

        public virtual IEnumerable<T> Find(string filterExpression, List<string> includes)
        {
            if (!string.IsNullOrWhiteSpace(filterExpression))
                return Dbcontext.Set<T>().AsNoTracking().WithIncludes(includes).Where(filterExpression);

            else
                return Dbcontext.Set<T>().AsNoTracking().WithIncludes(includes);

        }
        public virtual T GetById(long id, List<string> includes)
        {
            return DataDa.Find(@"Id=" + id.ToString(), includes).FirstOrDefault<T>();

        }


        public virtual T GetById(string filterExpression, List<string> includes)
        {
            return DataDa.GetItemById(filterExpression, includes);

        }

        public virtual IEnumerable<T> FindRange(string filterExpression, string sortingExpression, int startIndex, int count, List<string> includes)
        {
            if (!string.IsNullOrWhiteSpace(filterExpression))

                return Dbcontext.Set<T>().AsNoTracking()
                    .WithIncludes(includes)
                    .Where(filterExpression)
                    .OrderBy(sortingExpression)
                    .Skip(startIndex)
                    .Take(count);
            else
                return Dbcontext.Set<T>().AsNoTracking()
                    .WithIncludes(includes)
                    .OrderBy(sortingExpression)
                    .Skip(startIndex)
                    .Take(count);
        }
        public virtual IEnumerable<T> FindRange(string filterExpression, Expression<Func<T, bool>> predicate, string sortingExpression, int startIndex, int count, List<string> includes)
        {
            if (!String.IsNullOrWhiteSpace(filterExpression) && predicate != null)

                return Dbcontext.Set<T>().AsNoTracking()
                    .WithIncludes(includes)
                    .Where(filterExpression).Where(predicate)
                    .OrderBy(sortingExpression)
                    .Skip(startIndex)
                    .Take(count);
            else if (!String.IsNullOrWhiteSpace(filterExpression))

                return Dbcontext.Set<T>().AsNoTracking()
                    .WithIncludes(includes)
                    .Where(filterExpression)
                    .OrderBy(sortingExpression)
                    .Skip(startIndex)
                    .Take(count);
            else if (predicate != null)
                return Dbcontext.Set<T>().AsNoTracking()
                    .WithIncludes(includes)
                    .Where(predicate)
                    .OrderBy(sortingExpression)
                    .Skip(startIndex)
                    .Take(count);
            else
                return Dbcontext.Set<T>().AsNoTracking()
                    .WithIncludes(includes)
                    .OrderBy(sortingExpression)
                    .Skip(startIndex)
                    .Take(count);
        }
        public virtual T GetItemById(string filterExpression, List<string> includes)
        {
            return Dbcontext.Set<T>().AsNoTracking().Where(filterExpression)
                .WithIncludes(includes)
                .FirstOrDefault();

        }
        public virtual T GetItemById(int id)
        {
            return Dbcontext.Set<T>().Find(id);

        }
        public virtual int GetCount(string filterExpression)
        {
            if (!string.IsNullOrWhiteSpace(filterExpression))
                return Dbcontext.Set<T>().AsNoTracking().Where(filterExpression).Count();
            else
                return Dbcontext.Set<T>().AsNoTracking().Count();
        }

        public virtual int GetCount(string filterExpression, Expression<Func<T, bool>> predicate)
        {
            if (predicate != null && !String.IsNullOrWhiteSpace(filterExpression))
                return Dbcontext.Set<T>().AsNoTracking().Where(filterExpression).Where(predicate).Count();
            else if (!String.IsNullOrWhiteSpace(filterExpression))
                return Dbcontext.Set<T>().AsNoTracking().Where(filterExpression).Count();
            else if (predicate != null)
                return Dbcontext.Set<T>().AsNoTracking().Where(predicate).Count();
            else
                return Dbcontext.Set<T>().AsNoTracking().Count();
        }
        private string GetEntityName<T>() where T : class
        {
            // PluralizationService pluralizer = PluralizationService.CreateService(CultureInfo.GetCultureInfo("en"));
            // return string.Format("{0}.{1}", ((IObjectContextAdapter)DbContext).ObjectContext.DefaultContainerName, pluralizer.Pluralize(typeof(TEntity).Name));

            var entitySetName = ((IObjectContextAdapter)Dbcontext).ObjectContext.MetadataWorkspace.GetEntityContainer(((IObjectContextAdapter)Dbcontext).ObjectContext.DefaultContainerName, System.Data.Entity.Core.Metadata.Edm.DataSpace.CSpace).BaseEntitySets.First(bes => bes.ElementType.Name == typeof(T).Name).Name;
            return $"{((IObjectContextAdapter)Dbcontext).ObjectContext.DefaultContainerName}.{entitySetName}";
        }

        #endregion
    }
}
