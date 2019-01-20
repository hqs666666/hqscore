﻿using System;
using System.Linq;
using System.Reflection;
using Hqs.Model;
using Microsoft.EntityFrameworkCore;

namespace Hqs.Service
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var assemblyNames = Assembly.GetEntryAssembly().GetReferencedAssemblies().Where(p => p.Name.EndsWith("Model"))
                .ToArray();

            var assemblyList = assemblyNames.Select(Assembly.Load).ToList();
            var mappingInterface = typeof(IEntityTypeConfiguration<>);
            foreach (var assembly in assemblyList)
            {
                var mappingTypes = assembly.GetTypes().Where(p => p.GetInterfaces().Any(x =>
                    x.GetTypeInfo().IsGenericType && x.GetGenericTypeDefinition() == mappingInterface));

                var entityMethod = typeof(ModelBuilder).GetMethods()
                    .Single(x => x.Name == "Entity" && x.IsGenericMethod && x.ReturnType.Name == "EntityTypeBuilder`1");

                foreach (var mappingType in mappingTypes)
                {
                    var genericTypeArg = mappingType.GetInterfaces().Single().GenericTypeArguments.Single();
                    var genericEntityMethod = entityMethod.MakeGenericMethod(genericTypeArg);
                    var entityBuilder = genericEntityMethod.Invoke(builder, null);
                    var mapper = Activator.CreateInstance(mappingType);
                    mapper.GetType().GetMethod("Configure").Invoke(mapper, new[] { entityBuilder });
                }
            }
        }

        public new virtual DbSet<TEntity> Set<TEntity>() where TEntity : AbstractEntity
        {
            return base.Set<TEntity>();
        }
    }
}