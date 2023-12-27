namespace RtlTimo.InterviewDemo.Domain.Productions;

/// <summary>
/// A TV show.
/// </summary>
[Entity]
public sealed class Show : Entity<ShowId, Guid>
{
	// ID is inherited

	public ExternalId SourceId { get; }

	public ProperName Name { get; }

	public DateTime ModificationDateTime { get; }

	public Show(
		ExternalId sourceId,
		ProperName name,
		DateTime modificationDateTime)
		: base(id: Guid.NewGuid())
	{
		this.SourceId = sourceId ?? throw new NullValidationException(ErrorCode.Show_SourceIdNull, nameof(sourceId));
		this.Name = name ?? throw new NullValidationException(ErrorCode.Show_NameNull, nameof(name));
		this.ModificationDateTime = modificationDateTime;
	}
}
