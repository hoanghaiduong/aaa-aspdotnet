using aaa_aspdotnet.src.Common.Enums;
using aaa_aspdotnet.src.Common.pagination;
using aaa_aspdotnet.src.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Linq.Expressions;

namespace aaa_aspdotnet.src.Common.shared
{
    public static class SharedClass
    {
     

        public static PagedList<T> GetDataEntitiesWithPaging<T>(
             DbContext dbContext,
             FormattableString storedProcedureQuery,
             PaginationFilterDto dto) where T : class
        {
            try
            {
                var query = dbContext.Set<T>().FromSqlInterpolated(storedProcedureQuery).ToList().AsQueryable();

                if (!dto.SearchColumns.IsNullOrEmpty() && !dto.Search.IsNullOrEmpty())
                {
                    // Split the search columns into an array
                    var columns = dto.SearchColumns.Split(',');

                    // Create an expression for the search
                    var searchExpression = BuildSearchExpression<T>(columns, dto.Search);

                    // Apply the search expression to the query
                    query = query.Where(searchExpression);
                }

                if (!dto.OrderByColumn.IsNullOrEmpty())
                {
                    query = (dto.SortDirection == ESortDirection.DESC)
                        ? query.OrderByDescending(GetKeySelector<T>(dto.OrderByColumn))
                        : query.OrderBy(GetKeySelector<T>(dto.OrderByColumn));
                }

                // Xây dựng biểu thức sắp xếp động từ chuỗi sắp xếp được truyền vào từ dto

                PagedList<T> pagedEntities = PagedList<T>.ToPagedList(query, dto.Page, dto.PageSize);
                return pagedEntities;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public static Expression<Func<T, object>> GetKeySelector<T>(string columnName)
        {
            // Sử dụng Reflection để tạo biểu thức sắp xếp động dựa trên tên cột
            var parameter = Expression.Parameter(typeof(T), "entity");
            var property = Expression.Property(parameter, columnName);
            var conversion = Expression.Convert(property, typeof(object));
            var keySelector = Expression.Lambda<Func<T, object>>(conversion, parameter);
            return keySelector;
        }
        public static Expression<Func<T, bool>> BuildSearchExpression<T>(string[] columns, string searchKeyword)
        {
            var parameter = Expression.Parameter(typeof(T), "entity");
            var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });

            Expression searchExpression = null; // Biểu thức tìm kiếm gốc

            foreach (var column in columns)
            {
                var propertyExpression = Expression.Property(parameter, column);
                var searchValue = Expression.Constant(searchKeyword);

                var containsExpression = Expression.Call(propertyExpression, containsMethod, searchValue);

                // Thêm biểu thức con vào biểu thức tìm kiếm gốc
                if (searchExpression == null)
                {
                    searchExpression = containsExpression;
                }
                else
                {
                    searchExpression = Expression.OrElse(searchExpression, containsExpression);
                }
            }

            return Expression.Lambda<Func<T, bool>>(searchExpression, parameter);
        }

    }
}
