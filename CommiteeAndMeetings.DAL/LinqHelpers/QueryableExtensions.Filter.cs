using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;

namespace HelperServices.LinqHelpers
{
    public static partial class QueryableExtensions
    {
        public static IQueryable<T> Filter<T>(this IQueryable<T> queryable, Filter filter)
        {
            if (filter != null && (!string.IsNullOrEmpty(filter.Field) || (filter.Filters != null && filter.Filters.Any())))
            {
                // Collect a flat list of all filters
                var filters = filter.All();

                #region arabic characters
                //character conflicts in filter
                //  filters.ToList().ForEach(f => { f.Value = f.Value.Replace('ة', 'ه').Replace('أ', 'ا').Replace('إ', 'ا'); });

                //character conflicts in querable
                //  queryable = OperateOnArabicCharacters(queryable);
                #endregion

                // Create a predicate expression e.g. Field1 = @0 And Field2 > @1
                string predicate = filter.ToExpression(filters);

                // Get all filter values as array (needed by the Where method of Dynamic Linq)
                object[] values = filters.Select(f => f.Value).ToArray();
                for (int i = 0; i <= values.Length - 1; i++)
                {
                    if (values[i]?.ToString().ToLower() == "null")
                        values[i] = null;
                }
                // Use the Where method of Dynamic Linq to filter the data
                queryable = queryable.Where(predicate, values);
            }

            return queryable;
        }


        public static IQueryable<T> OperateOnArabicCharacters<T>(IQueryable<T> querable)
        {
            Type myType = typeof(T);
            string className = myType.Name;
            string propertyName = className.Replace("DetailsDTO", "").Replace("SummaryDTO", "") + "NameAr";
            foreach (var item in querable)
            {
                PropertyInfo _prop = myType.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
                if (_prop != null)
                {
                    string value = (string)_prop.GetValue(item);
                    if (!string.IsNullOrEmpty(value))
                    {
                        value = value.Replace('ة', 'ه').Replace('إ', 'ا').Replace('أ', 'ا');
                        _prop.SetValue(item, value);
                    }
                }
            }
            return querable;
        }

        public static bool checkLike(string match, string pattern)
        {
            return EF.Functions.Like(match, pattern);
        }

    }
}