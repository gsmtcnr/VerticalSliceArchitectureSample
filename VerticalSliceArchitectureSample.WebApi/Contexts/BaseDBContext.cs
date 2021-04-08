using Corex.Data.Derived.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;
using VerticalSliceArchitectureSample.WebApi.Dependency;
using VerticalSliceArchitectureSample.WebApi.Domain;

namespace VerticalSliceArchitectureSample.WebApi.Contexts
{
    public class BaseDBContext<TContext> : DbContext
        where TContext : DbContext
    {
        private readonly IDependencyManager dependencyManager;
        public BaseDBContext(IDependencyManager _dependencyManager, DbContextOptions<TContext> option) : base(option)
        {
            dependencyManager = _dependencyManager;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Console.WriteLine("OnModelCreating");
            base.OnModelCreating(modelBuilder);
            MethodInfo entityMethod = modelBuilder.GetType().GetTypeInfo().GetMethods().First(p => p.Name == nameof(ModelBuilder.Entity) && p.IsGenericMethod);

            var configurations = dependencyManager.ResolveAll<IEntityTypeConfiguration>();
            foreach (IEntityTypeConfiguration item in configurations)
            {
                TypeInfo typeInfo = item.GetType().GetTypeInfo();
                System.Type baseType = item.GetType().BaseType;
                MethodInfo mapMethod = GetMapMethod(typeInfo, baseType);
                TypeInfo baseTypeInfo = typeInfo.BaseType.GetTypeInfo();
                System.Type entityGenericType = baseTypeInfo.GenericTypeArguments[0];
                MethodInfo genericMethod = entityMethod.MakeGenericMethod(entityGenericType);
                object methodResult = genericMethod.Invoke(modelBuilder, null);
                mapMethod.Invoke(item, new object[] { methodResult });
            }
        }
        private static MethodInfo GetMapMethod(TypeInfo typeInfo, Type baseType)
        {
            MethodInfo mapMethod = null;
            string entityGuidType = typeof(BaseGuidKeyEntityConfiguration<BaseGuidKeyEntity>).Name;
            string entityIntType = typeof(BaseIntKeyEntityConfiguration<BaseIntKeyEntity>).Name;
            string baseTypeName = baseType.Name;
            if (baseTypeName == entityGuidType)
                mapMethod = typeInfo.GetMethod(nameof(BaseGuidKeyEntityConfiguration<BaseGuidKeyEntity>.Map));
            else if (baseTypeName == entityIntType)
                mapMethod = typeInfo.GetMethod(nameof(BaseIntKeyEntityConfiguration<BaseIntKeyEntity>.Map));
            return mapMethod;
        }
    }
}
