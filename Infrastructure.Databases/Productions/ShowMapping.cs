using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RtlTimo.InterviewDemo.Domain.Productions;

namespace RtlTimo.InterviewDemo.Infrastructure.Databases.Productions;

internal sealed class ShowMapping : IEntityTypeConfiguration<Show>
{
	public void Configure(EntityTypeBuilder<Show> builder)
	{
		builder.Property(x => x.Id);

		builder.Property(x => x.SourceId);

		builder.Property(x => x.Name)
			.HasMaxLength(ProperName.MaxLength)
			.UseCollation(CoreDbContext.CulturalCollation);

		builder.Property(x => x.PremierDate);

		builder.Property(x => x.EndDate);

		builder.Property(x => x.ModificationDateTime);

		builder.Property<byte[]>("RowVersion")
			.IsRequired()
			.IsRowVersion();

		builder.HasKey(x => x.Id);

		builder.HasAlternateKey(x => x.SourceId);

		builder.HasIndex(nameof(Show.EndDate), nameof(Show.PremierDate));

		builder.HasIndex(x => x.ModificationDateTime);
	}
}
