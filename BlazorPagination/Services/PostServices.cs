using BlazorPagination.Data;
using BlazorPagination.Models;
using System.Linq.Expressions;

namespace BlazorPagination.Services
{
    public class PostServices
    {
        public interface IPostService
        {
            Task<PaginatedList<Post>> GetList(int? pageNumber, string sortField, string sortOrder);
        }
        public class PostService : IPostService
        {
            private readonly ApplicationDbContext _context;
            public PostService(ApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<PaginatedList<Post>> GetList(int? pageNumber, string sortField, string sortOrder)
            {
                int pageSize = 5;
                var toDoList = _context.Posts.OrderByDynamic(sortField, sortOrder.ToUpper());
                return await PaginatedList<Post>.CreateAsync(toDoList, pageNumber ?? 1, pageSize);
            }
        }
    }
    public static class QueryableExtensions
    {
        public static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> query, string orderByMember, string direction)
        {
            var queryElementTypeParam = Expression.Parameter(typeof(T));
            var memberAccess = Expression.PropertyOrField(queryElementTypeParam, orderByMember);
            var keySelector = Expression.Lambda(memberAccess, queryElementTypeParam);
            var orderBy = Expression.Call(
                typeof(Queryable),
                direction == "ASC" ? "OrderBy" : "OrderByDescending",
                new Type[] { typeof(T), memberAccess.Type },
                query.Expression,
                Expression.Quote(keySelector));
            return query.Provider.CreateQuery<T>(orderBy);
        }
    }
}
