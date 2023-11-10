using System.Linq.Expressions;

namespace aaa_aspdotnet.src.Common.shared
{
    public static class SharedClass
    {
        public static Expression<Func<T, object>> GetKeySelector<T>(string columnName)
        {
            // Sử dụng Reflection để tạo biểu thức sắp xếp động dựa trên tên cột
            var parameter = Expression.Parameter(typeof(T), "entity");
            var property = Expression.Property(parameter, columnName);
            var conversion = Expression.Convert(property, typeof(object));
            var keySelector = Expression.Lambda<Func<T, object>>(conversion, parameter);
            return keySelector;
        }
    }
}
