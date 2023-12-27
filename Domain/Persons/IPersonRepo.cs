using RtlTimo.InterviewDemo.Domain.Productions;

namespace RtlTimo.InterviewDemo.Domain.Persons;

/// <summary>
/// Stores <see cref="Person"/> objects.
/// </summary>
public interface IPersonRepo
{
	Task<ILookup<ShowId, Person>> ListAppearingInAny(IEnumerable<ShowId> showIds, CancellationToken cancellationToken);
}
