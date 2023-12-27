using Architect.DomainModeling;
using Architect.EntityFramework.DbContextManagement;
using Prometheus;
using RtlTimo.InterviewDemo.Contracts.Shows.V1;

namespace RtlTimo.InterviewDemo.Application.Shows;

/// <summary>
/// Returns all TV shows in a paginated fashion.
/// </summary>
public sealed class GetShowsUseCase(
	ShowAdapter showAdapter,
	IDbContextProvider<ICoreDatabase> dbContextProvider,
	IShowRepo showRepo,
	IPersonRepo personRepo)
	: IApplicationService
{
	public static readonly Counter RequestCount = Metrics.CreateCounter("GetShowsRequestCount", "The number of GetShows requests");
	public static readonly Histogram RequestHistogram = Metrics.CreateHistogram("GetShowsHistogram", "A histogram of the GetShows requests",
		new HistogramConfiguration() { Buckets = Histogram.ExponentialBuckets(start: 1, factor: 2, count: 16) });

	public ushort PageSize { get; set; } = 100;

	public Task<GetShowsResponse> GetShows(GetShowsRequest request, CancellationToken cancellationToken)
	{
		var pageIndex = request?.PageIndex ?? 0;

		RequestCount.Inc();
		RequestHistogram.Observe(1 + pageIndex);

		return dbContextProvider.ExecuteInDbContextScopeAsync(cancellationToken, async (executionScope, cancellationToken) =>
		{
			var shows = await showRepo.ListPaged(pageSize: this.PageSize, pageIndex: pageIndex, cancellationToken);
			var personsAppearingInShows = await personRepo.ListAppearingInAny(shows.Select(show => show.Id), cancellationToken);

			var results = showAdapter.ToContracts(shows, personsAppearingInShows);
			return new GetShowsResponse(results);
		});
	}
}
