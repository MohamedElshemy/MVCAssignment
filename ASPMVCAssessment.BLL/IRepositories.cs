using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ASPMVCAssessment.BLL
{
    public interface IRepositories<T> where T : class
    {
        DbContext Context { get; }
        IEnumerable<T> Items { get; }

        int Add(T entity);
        int Delete(T entity);
        IEnumerable<T> Find(string filterExpression, List<string> includes);
        IEnumerable<T> FindRange(string filterExpression, string sortingExpression, int startIndex, int count, List<string> includes);
        IEnumerable<T> FindRange(string filterExpression, Expression<Func<T, bool>> predicate, string sortingExpression, int startIndex, int count, List<string> includes);
        IEnumerable<T> GetAll(List<string> includes);
        int GetCount(string filterExpression);
        int GetCount(string filterExpression, Expression<Func<T, bool>> predicate);
        T GetItemById(string filterExpression, List<string> includes);
        int Save();
        IEnumerable<T> SearchBy(Expression<Func<T, bool>> predicate);
        int Update(T entity);
    }
}
