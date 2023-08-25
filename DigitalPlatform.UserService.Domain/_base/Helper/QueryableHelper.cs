using System.Linq.Expressions;
using System.Text;
using DigitalPlatform.UserService.Domain._base.RequestBase.Query;
using DigitalPlatform.UserService.Domain._base.ResultBase;
using DigitalPlatform.UserService.Share;
using Microsoft.EntityFrameworkCore;

namespace DigitalPlatform.UserService.Domain._base.Helper
{
    public static class QueryableHelper
    {
        public static IQueryable<T> OrderThenPaging<T>(IQueryListBase<T> request, ref IQueryable<T> q, out int totalCount) where T : BaseGetViewModel
        {
            //Get total record
            totalCount = q.Count();

            //Order
            q = request.OrderBy.IsValid() ? q.OrderBy(request.OrderBy, request.OrderByDirection) : q.OrderByDescending(c => c.UpdatedAt);

            // No paging if export
            if (request.IsPaged)
            {
                q = q.Skip(request.PageNumber * request.PageSize).Take(request.PageSize);
            }

            return q;
        }

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> query, string sortColumn, string direction)
        {
            string methodName = $"OrderBy{(direction.ToLower() == "asc" ? "" : "descending")}";

            ParameterExpression parameter = Expression.Parameter(query.ElementType, "p");

            MemberExpression memberAccess = sortColumn.Split('.').Aggregate<string, MemberExpression>(null, (current, property) => Expression.Property(current ?? (Expression)parameter, property));

            LambdaExpression orderByLambda = Expression.Lambda(memberAccess, parameter);

            MethodCallExpression result = Expression.Call(
                typeof(Queryable),
                methodName,
                new[] { query.ElementType, memberAccess.Type },
                query.Expression,
                Expression.Quote(orderByLambda));

            return query.Provider.CreateQuery<T>(result);
        }

        public static IQueryable<TEntity> Include<TEntity>(this IQueryable<TEntity> source, int levelIndex, Expression<Func<TEntity, object>> expression) where TEntity : class
        {
            if (levelIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(levelIndex));
            var member = (MemberExpression)expression.Body;
            var property = member.Member.Name;
            var sb = new StringBuilder();
            for (int i = 0; i < levelIndex; i++)
            {
                if (i > 0)
                    sb.Append(Type.Delimiter);
                sb.Append(property);
            }
            return source.Include(sb.ToString());
        }
    }
}
