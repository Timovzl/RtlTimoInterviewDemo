namespace RtlTimo.InterviewDemo.Domain.Productions;

/// <summary>
/// A TV show.
/// </summary>
[Entity]
public sealed class Show : Entity<ShowId, long>
{
	// ID is inherited

	public ProperName Name { get; }

	public DateOnly? PremierDate { get; }
	public DateOnly EndDate { get; }

	public DateTime ModificationDateTime { get; }

	public Show(
		ShowId id,
		ProperName name,
		DateOnly? premierDate,
		DateOnly? endDate,
		DateTime modificationDateTime)
		: base(id)
	{
		this.Name = name ?? throw new NullValidationException(ErrorCode.Show_NameNull, nameof(name));
		this.PremierDate = premierDate;
		this.EndDate = endDate ?? DateOnly.FromDateTime(DateTime.MaxValue);
		this.ModificationDateTime = modificationDateTime;
	}
}
