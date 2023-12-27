namespace RtlTimo.InterviewDemo.Domain.Productions;

/// <summary>
/// A TV show.
/// </summary>
[Entity]
public sealed class Show : Entity<ShowId, long>
{
	// ID is inherited

	public ProperName Name { get; }

	public DateTime ModificationDateTime { get; }

	public Show(
		ShowId id,
		ProperName name,
		DateTime modificationDateTime)
		: base(id)
	{
		this.Name = name ?? throw new NullValidationException(ErrorCode.Show_NameNull, nameof(name));
		this.ModificationDateTime = modificationDateTime;
	}
}
