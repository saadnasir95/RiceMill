using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;

namespace TheRiceMill.Persistence.Extensions
{
    public static class DbSetExtensions
    {

        public static IQueryable<T> GetMany<T>(this DbSet<T> set, Expression<Func<T, bool>> @where, Expression<Func<T, object>> @orderby = null,
            int page = 1, int pageSize = 10,
            bool isDescending = false, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null) where T: class 
        {
            IQueryable<T> query = set.Where(where);
            if (include != null)
            {
                query = include(query);
            }
            if (orderby != null)
            {
                query = isDescending ? query.OrderByDescending(@orderby) : query.OrderBy(@orderby);
            }

            return query.Pagination(page, pageSize);
        }

        public static IQueryable<T> GetMany<T>(this DbSet<T> set, Expression<Func<T, bool>> @where, string @orderby = "", int page = 1, int pageSize = 10, bool isDescending = false,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null) where T: class 
        {
            IQueryable<T> query = set.Where(where);
            if (include != null)
            {
                query = include(query);
            }
            query = query.OrderByCustom(@orderby, isDescending);
            return query.Pagination(page, pageSize);
        }

        public static IQueryable<T> GetManyReadOnly<T>(this DbSet<T> set, Expression<Func<T, bool>> @where, string @orderby = "", int page = 1, int pageSize = 10, bool isDescending = false,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null) where T : class
        {
            IQueryable<T> query = set.Where(where);
            if (include != null)
            {
                query = include(query);
            }
            query = query.OrderByCustom(@orderby, isDescending).AsNoTracking();
            return query.Pagination(page, pageSize);
        }

        public static async Task<decimal> SumAsync<T>(this DbSet<T> set, Expression<Func<T, bool>> @where,
            Expression<Func<T, decimal>> selector, CancellationToken cancellationToken = default(CancellationToken)) where T : class
        {
            IQueryable<T> query = set.Where(where);
            return await query.SumAsync(selector,cancellationToken);
        }
        public static async Task<double> SumAsync<T>(this DbSet<T> set, Expression<Func<T, bool>> @where,
            Expression<Func<T, double>> selector, CancellationToken cancellationToken = default(CancellationToken)) where T : class
        {
            IQueryable<T> query = set.Where(where);
            return await query.SumAsync(selector,cancellationToken);
        }
        public static T GetBy<T>(this DbSet<T> set,Expression<Func<T, bool>> @where,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null) where T: class 
        {
            IQueryable<T> query = set.Where(where);
            if (include != null)
            {
                query = include(query);
            }
            return query.FirstOrDefault();
        }
        public static T GetByReadOnly<T>(this DbSet<T> set, Expression<Func<T, bool>> @where,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null) where T : class
        {
            IQueryable<T> query = set.Where(where).AsNoTracking();
            if (include != null)
            {
                query = include(query);
            }
            return query.FirstOrDefault();
        }


        public static Func<T, object> GetSortExpression<T>(string sortExpressionStr)
        {
            ParameterExpression param = Expression.Parameter(typeof(T), "x");
            Expression<Func<T, object>> sortExpression = Expression.Lambda<Func<T, object>>(Expression.Convert(Expression.Property(param, sortExpressionStr), typeof(object)), param);
            return sortExpression.Compile();
        }

        private static readonly TypeInfo QueryCompilerTypeInfo = typeof(QueryCompiler).GetTypeInfo();

        private static readonly FieldInfo QueryCompilerField = typeof(EntityQueryProvider).GetTypeInfo().DeclaredFields.First(x => x.Name == "_queryCompiler");

        private static readonly FieldInfo QueryModelGeneratorField = QueryCompilerTypeInfo.DeclaredFields.First(x => x.Name == "_queryModelGenerator");

        private static readonly FieldInfo DataBaseField = QueryCompilerTypeInfo.DeclaredFields.Single(x => x.Name == "_database");

        private static readonly PropertyInfo DatabaseDependenciesField = typeof(Database).GetTypeInfo().DeclaredProperties.Single(x => x.Name == "Dependencies");

        public static string ToSql<TEntity>(this IQueryable<TEntity> query) where TEntity : class
        {
            var queryCompiler = (QueryCompiler)QueryCompilerField.GetValue(query.Provider);
            var modelGenerator = (QueryModelGenerator)QueryModelGeneratorField.GetValue(queryCompiler);
            var queryModel = modelGenerator.ParseQuery(query.Expression);
            var database = (IDatabase)DataBaseField.GetValue(queryCompiler);
            var databaseDependencies = (DatabaseDependencies)DatabaseDependenciesField.GetValue(database);
            var queryCompilationContext = databaseDependencies.QueryCompilationContextFactory.Create(false);
            var modelVisitor = (RelationalQueryModelVisitor)queryCompilationContext.CreateQueryModelVisitor();
            modelVisitor.CreateQueryExecutor<TEntity>(queryModel);
            var sql = modelVisitor.Queries.First().ToString();

            return sql;
        }
        public static IOrderedQueryable<T> OrderByCustom<T>(this IQueryable<T> query, string sortExpression, bool isDescending)
        {
            if (isDescending)
            {
                return query.OrderByDescending((sortExpression));
            }
            return query.OrderBy((sortExpression));
        }
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName)
        {
            return source.OrderBy(ToLambda<T>(propertyName));
        }

        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string propertyName)
        {
            return source.OrderByDescending(ToLambda<T>(propertyName));
        }


        private static Expression<Func<T, object>> ToLambda<T>(string propertyName)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T));
            string[] members = propertyName.Split('.');
            MemberExpression property = null;
            foreach (string member in members)
            {
                if (property == null)
                {
                    property = Expression.Property(parameter, member);
                }
                else
                {
                    property = Expression.Property(property, member);
                }
            }
            UnaryExpression propAsObject = Expression.Convert(property, typeof(object));

            return Expression.Lambda<Func<T, object>>(propAsObject, parameter);
        }

        public static IQueryable<T> Pagination<T>(this IQueryable<T> query, int page, int pageSize) where T : class
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }


        public static int PageCount<T>(this IQueryable<T> query, int pageSize) where T : class
        {
            if (pageSize < 1) pageSize = 10;
            return (int)Math.Ceiling((float)query.Count() / pageSize);
        }
        public static Expression<Func<T, bool>> AndAlso<T>(
            this Expression<Func<T, bool>> expr1,
            Expression<Func<T, bool>> expr2)
        {
            var parameter = Expression.Parameter(typeof(T));

            var leftVisitor = new ReplaceExpressionVisitor(expr1.Parameters[0], parameter);
            var left = leftVisitor.Visit(expr1.Body);

            var rightVisitor = new ReplaceExpressionVisitor(expr2.Parameters[0], parameter);
            var right = rightVisitor.Visit(expr2.Body);

            return Expression.Lambda<Func<T, bool>>(
                Expression.AndAlso(left, right), parameter);
        }


        public class ReplaceExpressionVisitor
            : ExpressionVisitor
        {
            private readonly Expression _oldValue;
            private readonly Expression _newValue;

            public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
            {
                _oldValue = oldValue;
                _newValue = newValue;
            }

            public override Expression Visit(Expression node)
            {
                if (node == _oldValue)
                    return _newValue;
                return base.Visit(node);
            }
        }
    }
}