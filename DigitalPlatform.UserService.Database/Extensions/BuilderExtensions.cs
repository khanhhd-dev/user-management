using System.Linq.Expressions;
using DigitalPlatform.UserService.Entity._base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace DigitalPlatform.UserService.Database.Extensions
{
    public static class BuilderExtensions
    {
        public static void ApplyGlobalFilters<TInterface>(this ModelBuilder modelBuilder, Expression<Func<TInterface, bool>> expression)
        {
            var entities = modelBuilder.Model
                .GetEntityTypes()
                .Where(e => e.ClrType.GetInterface(typeof(TInterface).Name) != null)
                .Select(e => e.ClrType);
            foreach (var entity in entities)
            {
                var newParam = Expression.Parameter(entity);
                var newBody = ReplacingExpressionVisitor.Replace(expression.Parameters.Single(), newParam, expression.Body);
                modelBuilder.Entity(entity).HasQueryFilter(Expression.Lambda(newBody, newParam));
            }
        }

        public static void CreateIndexIsDeleted(this ModelBuilder modelBuilder)
        {
            var entities = modelBuilder.Model
                .GetEntityTypes()
                .Where(e => e.ClrType.GetInterface(nameof(IEntityBase)) != null)
                .Select(e => e.ClrType);
            foreach (var entity in entities)
            {
                modelBuilder.Entity(entity).HasIndex("IsDeleted");
            }
        }

        public static void CreateIndex(this ModelBuilder modelBuilder)
        {

        }
    }
}
