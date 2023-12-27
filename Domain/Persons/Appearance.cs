using RtlTimo.InterviewDemo.Domain.Productions;

namespace RtlTimo.InterviewDemo.Domain.Persons;

/// <summary>
/// The occurrence of a <see cref="Person"/> appearing in the cast of a <see cref="Show"/>, irrespective of the particular role.
/// </summary>
[ValueObject]
public sealed partial class Appearance
{
	public PersonId PersonId { get; private init; }
	public ShowId ShowId { get; private init; }

	public Appearance(PersonId personId, ShowId showId)
	{
		this.PersonId = personId;
		this.ShowId = showId;
	}
}
