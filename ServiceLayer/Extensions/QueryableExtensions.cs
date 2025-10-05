using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ServiceLayer.Extensions
{
    public static class QueryableExtensions
    {
        public static Task<T?> FirstOrDefaultAsync<T>(
            this IQueryable<T> source,
            Expression<Func<T, bool>> predicate) where T : class
        {
            return EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(source, predicate);
        }
    }
}