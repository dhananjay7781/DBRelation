using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication1.Models;

namespace WebApplication1.Configurations
{
	public class AddressConfiguration : IEntityTypeConfiguration<Address>
	{
		public void Configure(EntityTypeBuilder<Address> builder)
		{
			builder.HasKey(x => x.Id);

			builder.Property(x => x.Id)
				.HasColumnType("int")
				.IsRequired();

			builder.Property(x => x.AddressLine)
				.HasColumnType("nvarchar")
				.HasMaxLength(500)
				.IsRequired();

			builder.Property(x => x.City)
				.HasColumnType("nvarchar")
				.HasMaxLength(50)
				.IsRequired();

			builder.Property(x => x.State)
				.HasColumnType("nvarchar")
				.HasMaxLength(50)
				.IsRequired();

			builder.Property(x => x.Country)
				.HasColumnType("nvarchar")
				.HasMaxLength(50)
				.IsRequired();

			builder.Property(x => x.ZipCode)
				.HasColumnType("bigint")
				.IsRequired();
		}
	}
}
