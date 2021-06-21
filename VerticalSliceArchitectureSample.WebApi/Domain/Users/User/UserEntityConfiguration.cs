using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace VerticalSliceArchitectureSample.WebApi.Domain.Users.User
{

    public class UserEntityConfiguration : BaseGuidKeyEntityConfiguration<UserEntity>
    {
        public override string GetSchemaName()
        {
            return "User";
        }
        public override string GetTableName()
        {
            return "User";
        }
        public override void Map(EntityTypeBuilder<UserEntity> entity)
        {
            entity.Property(m => m.Name).HasMaxLength(32).IsRequired(false);
            entity.Property(m => m.Surname).HasMaxLength(32).IsRequired(false);
            entity.Property(m => m.Email).HasMaxLength(32).IsRequired();
            entity.Property(m => m.Password).HasMaxLength(64).IsRequired();
            entity.Property(m => m.Address).HasMaxLength(512).IsRequired(false);
            base.Map(entity);
        }
    }
}
