using System;
using System.Linq;
using System.Reflection;
using Hqs.Model;
using Hqs.Model.Users;
using Hqs.Model.Logs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Hqs.Service
{
    public class DataContext : DbContext
    {
        private IConfiguration _configuration;
        public DataContext(DbContextOptions<DataContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            var dataType = _configuration["ConnectionStrings:DataType"] == "MySql";
            if (dataType)
                MySqlMapping(builder);
            else
                MsSqlMapping(builder);
        }

        private void MySqlMapping(ModelBuilder builder)
        {
            builder.Entity<User>().ToTable("User");
            builder.Entity<UserProfile>().ToTable("UserProfile");
            builder.Entity<Log>().ToTable("LogApi");
        }

        private void MsSqlMapping(ModelBuilder builder)
        {
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
