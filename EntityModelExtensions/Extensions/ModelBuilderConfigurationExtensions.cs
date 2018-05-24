using System;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace EntityModelExtensions.Extensions
{
    /// <summary>
    /// Вспомогательный класс для добавления своих соглашений по атрибутам для EntityFramework
    /// https://stackoverflow.com/questions/32049742/ef-7-set-initial-default-value-for-datetime-column
    /// </summary>
    public static class ModelBuilderConfigurationExtensions
    {
        /// <summary>
        /// Авто поиск конфигурационных классов моделей (интерфейс IEntityTypeConfiguration)
        /// и запуск их конфигурационных методов
        /// https://stackoverflow.com/questions/26957519/ef-core-mapping-entitytypeconfiguration
        /// </summary>
        public static ModelBuilder UseAutoRegisterModelConfiguration<TDbContext>(this ModelBuilder builder)
        {
            // Interface that all of our Entity maps implement
            var mappingInterface = typeof(IEntityTypeConfiguration<>);

            // Types that do entity mapping
            var mappingTypes = typeof(TDbContext).GetTypeInfo()
                .Assembly
                .GetTypes()
                .Where(x => x.GetInterfaces()
                    .Any(y => y.GetTypeInfo().IsGenericType && y.GetGenericTypeDefinition() == mappingInterface));

            // Get the generic Entity method of the ModelBuilder type
            var entityMethod = typeof(ModelBuilder).GetMethods()
                .Single(x => x.Name == "Entity" &&
                             x.IsGenericMethod &&
                             x.ReturnType.Name == "EntityTypeBuilder`1");

            foreach (var mappingType in mappingTypes)
            {
                // Get the type of entity to be mapped
                var genericTypeArg = mappingType.GetInterfaces().Single().GenericTypeArguments.Single();

                // Get the method builder.Entity<TEntity>
                var genericEntityMethod = entityMethod.MakeGenericMethod(genericTypeArg);

                // Invoke builder.Entity<TEntity> to get a builder for the entity to be mapped
                var entityBuilder = genericEntityMethod.Invoke(builder, null);

                // Create the mapping type and do the mapping
                var mapper = Activator.CreateInstance(mappingType);
                mapper.GetType().GetMethod("Configure").Invoke(mapper, new[] { entityBuilder });
            }

            return builder;
        }
    }
}
