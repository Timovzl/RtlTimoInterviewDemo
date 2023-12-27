using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RtlTimo.InterviewDemo.Domain.Persons;

namespace RtlTimo.InterviewDemo.Infrastructure.Databases.Persons;

internal sealed class PersonMapping : IEntityTypeConfiguration<Person>
{
	public void Configure(EntityTypeBuilder<Person> builder)
	{
		builder.Property(x => x.Id);

		builder.Property(x => x.Name)
			.HasMaxLength(ProperName.MaxLength)
			.UseCollation(CoreDbContext.CulturalCollation);

		builder.Property(x => x.DateOfBirth);

		builder.Property(x => x.ModificationDateTime);

		builder.Property<byte[]>("RowVersion")
			.IsRequired()
			.IsRowVersion();

		builder.HasKey(x => x.Id);

		builder.HasIndex(x => x.ModificationDateTime);
	}
}
