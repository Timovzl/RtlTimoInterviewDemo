namespace RtlTimo.InterviewDemo.Domain.Persons;

/// <summary>
/// Any person involved in the domain, such as an actor or a crew member.
/// </summary>
[Entity]
public sealed class Person : Entity<PersonId, Guid>
{
	// ID is inherited

	public ExternalId SourceId { get; }

	public ProperName Name { get; }

	/// <summary>
	/// Null if unknown.
	/// </summary>
	public DateOnly? DateOfBirth { get; }

	public DateTime ModificationDateTime { get; }

	public Person(
		ExternalId sourceId,
		ProperName name,
		DateOnly? dateOfBirth,
		DateTime modificationDateTime)
		: base(id: Guid.NewGuid())
	{
		this.SourceId = sourceId ?? throw new NullValidationException(ErrorCode.Show_SourceIdNull, nameof(sourceId));
		this.Name = name ?? throw new NullValidationException(ErrorCode.Show_NameNull, nameof(name));
		this.DateOfBirth = dateOfBirth;
		this.ModificationDateTime = modificationDateTime;
	}
}
