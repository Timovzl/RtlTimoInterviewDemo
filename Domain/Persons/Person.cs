namespace RtlTimo.InterviewDemo.Domain.Persons;

/// <summary>
/// Any person involved in the domain, such as an actor or a crew member.
/// </summary>
[Entity]
public sealed class Person : Entity<PersonId, long>
{
	// ID is inherited

	public ProperName Name { get; }

	/// <summary>
	/// Null if unknown.
	/// </summary>
	public DateOnly? DateOfBirth { get; }

	public DateTime ModificationDateTime { get; }

	public Person(
		PersonId id,
		ProperName name,
		DateOnly? dateOfBirth,
		DateTime modificationDateTime)
		: base(id)
	{
		this.Name = name ?? throw new NullValidationException(ErrorCode.Person_NameNull, nameof(name));
		this.DateOfBirth = dateOfBirth;
		this.ModificationDateTime = modificationDateTime;
	}
}
