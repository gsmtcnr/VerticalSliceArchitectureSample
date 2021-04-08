using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace VerticalSliceArchitectureSample.WebApi.Domain.Users.User
{
    public static class UserDomain
    {
        #region Entity 
        public class UserEntity : BaseIntKeyEntity
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public string Password { get; set; }
            public string Email { get; set; }
            public string Address { get; set; }
        }
        public class UserEntityConfiguration : BaseIntKeyEntityConfiguration<UserEntity>
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
        #endregion
        #region Dto
        public class UserDto : BaseIntKeyDto
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public string Password { get; set; }
            public string Email { get; set; }
            public string Address { get; set; }

        }
        #endregion
        #region ViewModels
        #region Inputs
        public record RegisterInputModel(string Email, string Password);
  
        #endregion
        #endregion
    }
}
