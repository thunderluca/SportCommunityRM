using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SportCommunityRM.Data.Helpers
{
    public static class DbContextHelper
    {
        private const string DefaultLookupStringFormat = "{0}_{1}";

        public static string GetFormattedLookupTableName(
            string contextName,
            Type contextType,
            Type entityType,
            string format = DefaultLookupStringFormat)
        {
            return string.Format(
                DefaultLookupStringFormat,
                contextName,
                GetTableName(contextType, entityType));
        }

        private static IEnumerable<Tuple<string, Type>> GetTablesNamesAndTypes(Type contextType)
        {
            return contextType.GetProperties()
                .Where(pi => pi.PropertyType.Name == typeof(DbSet<>).Name)
                .Select(pi => Tuple.Create(pi.Name, pi.PropertyType.GetGenericArguments().First()))
                .ToArray();
        }

        public static IEnumerable<Type> GetTablesTypes(this DbContext dbContext) => GetTablesTypes(dbContext.GetType());

        public static IEnumerable<Type> GetTablesTypes<T>() => GetTablesTypes(typeof(T));

        public static IEnumerable<Type> GetTablesTypes(Type contextType)
        {
            return contextType.GetProperties()
                .Where(pi => pi.PropertyType.Name == typeof(DbSet<>).Name)
                .Select(pi => pi.PropertyType.GetGenericArguments().First())
                .ToArray();
        }

        public static string GetTableName(Type contextType, Type entityType)
        {
            var dbSetProperties = GetTablesNamesAndTypes(contextType);

            var selectedDbSetProperty = dbSetProperties.FirstOrDefault(t => t.Item2.Name == entityType.Name);

            return selectedDbSetProperty != null ? selectedDbSetProperty.Item1 : entityType.Name;
        }
    }
}
