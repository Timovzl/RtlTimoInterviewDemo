using RtlTimo.InterviewDemo.Domain.Productions;

namespace RtlTimo.InterviewDemo.Domain.Persons;

public interface IPersonRepo
{
	Task<ILookup<ShowId, Person>> ListAppearingInAny(IEnumerable<ShowId> showIds, CancellationToken cancellationToken);
}
