using RtlTimo.InterviewDemo.Domain.Persons;
using RtlTimo.InterviewDemo.Domain.Productions;
using RtlTimo.InterviewDemo.Domain.Shared;

namespace RtlTimo.InterviewDemo.Application.Shows;

/// <summary>
/// An external source to obtain <see cref="Show"/> data from.
/// </summary>
public interface IShowSource
{
	IAsyncEnumerable<Show> EnumerateAllShows(CancellationToken cancellationToken);

	Task<IReadOnlyCollection<Person>> GetCastForShow(ExternalId showId, CancellationToken cancellationToken);
}
