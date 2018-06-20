using System;
using System.Collections.Generic;
using System.Linq;
using EntityModelExtensions.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EntityModelExtensions.Extensions
{
    /// <summary>
    /// Вспомогательный класс для добавления своих соглашений по атрибутам для EntityFramework
    /// https://stackoverflow.com/questions/32049742/ef-7-set-initial-default-value-for-datetime-column
    /// </summary>
    public static class ModelBuilderAttributesExtensions
    {
        /// <summary>
        /// Вспомогательный класс для добавления своих соглашений по атрибутам для EntityFramework
        /// </summary>
        public static ModelBuilder UseAdditionalAttributes(this ModelBuilder builder)
        {
            SetSqlValueForPropertiesWithAttribute<SqlDefaultValueAttribute>(builder, x => x.DefaultValue);
            return builder;
        }

        private static void SetSqlValueForPropertiesWithAttribute<TAttribute>(ModelBuilder builder, Func<TAttribute, string> lambda) where TAttribute : class
        {
            SetPropertyValue<TAttribute>(builder).ForEach((x) =>
            {
                x.Item1.Relational().DefaultValueSql = lambda(x.Item2);
            });
        }

        private static List<Tuple<IMutableProperty, TAttribute>> SetPropertyValue<TAttribute>(ModelBuilder builder) where TAttribute : class
        {
            var propsToModify = new List<Tuple<IMutableProperty, TAttribute>>();
            foreach (var entity in builder.Model.GetEntityTypes())
            {
                var properties = entity.GetProperties();
                foreach (var property in properties)
                {
                    if (property?.PropertyInfo
                        ?.GetCustomAttributes(typeof(TAttribute), false)
                        .FirstOrDefault() is TAttribute attribute)
                    {
                        propsToModify.Add(new Tuple<IMutableProperty, TAttribute>(property, attribute));
                    }
                }
            }
            return propsToModify;
        }
    }
}
