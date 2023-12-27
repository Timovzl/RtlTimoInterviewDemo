namespace RtlTimo.InterviewDemo.Application.Shows;

/// <summary>
/// An external source to obtain <see cref="Show"/> data from.
/// </summary>
public interface IShowSource
{
	IAsyncEnumerable<Show> EnumerateAllShows(CancellationToken cancellationToken);

	Task<IReadOnlyCollection<Person>> GetCastForShow(ShowId showId, CancellationToken cancellationToken);
}
