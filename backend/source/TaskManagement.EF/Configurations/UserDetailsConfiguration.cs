using TaskManagement.Domain.UserDetails;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TaskManagement.EF.Configurations
{
    public class UserDetailsConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("UserDetails");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Username);
            builder.Property(x => x.Email);
        }
    }
}







