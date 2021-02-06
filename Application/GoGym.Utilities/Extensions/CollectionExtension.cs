using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GoGym.Utilities.Extensions
{
    public static class CollectionExtension
    {
        public static List<T2> CastAs<T1, T2>(this IEnumerable<T1> list, Func<T1, T2> fn)
        {
            List<T2> list2 = new List<T2>();
            foreach (T1 item in list)
            {
                list2.Add(fn(item));
            }
            return list2;
        }

        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (T item in collection)
                action(item);
        }

        public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> list, string sortExpression)
        {
            sortExpression += "";
            string[] parts = sortExpression.Split(' ');
            bool descending = false;
            string property = "";

            if (parts.Length > 0 && parts[0] != "")
            {
                property = parts[0];

                if (parts.Length > 1)
                {
                    descending = parts[1].ToLower().Contains("desc") || parts[1].ToLower().Contains("descending");
                }

                PropertyInfo prop = typeof(T).GetProperty(property);

                if (prop == null)
                {
                    throw new Exception("No property '" + property + "' in + " + typeof(T).Name + "'");
                }

                if (descending)
                    return list.OrderByDescending(x => prop.GetValue(x, null));
                else
                    return list.OrderBy(x => prop.GetValue(x, null));
            }

            return list;
        }
    }
}
