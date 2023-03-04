using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using WebApplication1.Models;

namespace WebApplication1.Configurations
{
	public class UserConfiguration : IEntityTypeConfiguration<User>
	{
		public void Configure(EntityTypeBuilder<User> builder)
		{
			builder.HasKey(x => x.Id);

			builder.Property(x => x.Id)
				.HasColumnType("int")
				.IsRequired();

			builder.Property(x => x.UserName)
				.HasColumnType("nvarchar")
				.HasMaxLength(50)
				.IsRequired();

			builder.Property(x => x.Email)
				.HasColumnType("nvarchar")
				.HasMaxLength(200)
				.IsRequired();

			builder.Property(x => x.FirstName)
				.HasColumnType("nvarchar")
				.HasMaxLength(50)
				.IsRequired();

            builder.Property(x => x.MiddleName)
                .HasColumnType("nvarchar")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.LastName)
				.HasColumnType("nvarchar")
				.HasMaxLength(50)
				.IsRequired();

			builder.Property(x => x.BirthDate)
				.HasColumnType("datetime2")
				.HasMaxLength(7)
				.IsRequired(false);

			builder.Property(x => x.Address)
				.HasColumnType("nvarchar")
				.HasMaxLength(500)
				.IsRequired(false);

			builder.Property(x => x.Password)
				.HasColumnType("nvarchar")
				.HasMaxLength(1000)
				.IsRequired();

			// Relationships
			// One-to-one
			// User-ProfilePhoto
			builder.HasOne(x => x.ProfilePhoto)
				.WithOne(x => x.User)
				.HasForeignKey<ProfilePhoto>(x => x.UserId)
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();

			// One-to-Many
			// User-Address
			builder.HasMany(x => x.Addresses)
				.WithOne(x => x.User)
				.HasForeignKey(x => x.UserId)
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired();

            //Many-to-Many
            //User-Roles

           


        }
	}
}
