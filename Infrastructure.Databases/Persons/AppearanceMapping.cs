using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RtlTimo.InterviewDemo.Domain.Persons;
using RtlTimo.InterviewDemo.Domain.Productions;

namespace RtlTimo.InterviewDemo.Infrastructure.Databases.Persons;

internal sealed class AppearanceMapping : IEntityTypeConfiguration<Appearance>
{
	public void Configure(EntityTypeBuilder<Appearance> builder)
	{
		builder.Property(x => x.PersonId);

		builder.Property(x => x.ShowId);

		builder.HasKey(nameof(Appearance.PersonId), nameof(Appearance.ShowId));

		builder.HasIndex(nameof(Appearance.ShowId), nameof(Appearance.PersonId));

		builder.HasOne<Person>().WithMany().HasForeignKey(x => x.PersonId);

		builder.HasOne<Show>().WithMany().HasForeignKey(x => x.ShowId);
	}
}
