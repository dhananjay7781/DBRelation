using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication1.Models;

namespace WebApplication1.Configurations
{
	public class ProfilePhotoConfiguration : IEntityTypeConfiguration<ProfilePhoto>
	{
		public void Configure(EntityTypeBuilder<ProfilePhoto> builder)
		{
			builder.HasKey(x => x.Id);

			builder.Property(x => x.Id)
				.HasColumnType("int")
				.IsRequired();

			builder.Property(x => x.Url)
				.HasColumnType("nvarchar")
				.HasMaxLength(2000)
				.IsRequired();
		}
	}
}
