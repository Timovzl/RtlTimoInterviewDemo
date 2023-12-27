using Architect.DomainModeling;
using Architect.EntityFramework.DbContextManagement;
using RtlTimo.InterviewDemo.Contracts.Shows.V1;

namespace RtlTimo.InterviewDemo.Application.Shows;

/// <summary>
/// Returns all TV shows in a paginated fashion.
/// </summary>
public sealed class GetShowsUseCase(
	IDbContextProvider<ICoreDatabase> dbContextProvider,
	IShowRepo showRepo,
	IPersonRepo personRepo,
	ShowAdapter showAdapter)
	: IApplicationService
{
	public ushort PageSize { get; set; } = 1000;

	public Task<GetShowsResponse> GetShows(GetShowsRequest request, CancellationToken cancellationToken)
	{
		return dbContextProvider.ExecuteInDbContextScopeAsync(cancellationToken, async (executionScope, cancellationToken) =>
		{
			var pageIndex = request?.PageIndex ?? 0;

			var shows = await showRepo.ListPaged(pageSize: this.PageSize, pageIndex: pageIndex, cancellationToken);
			var personsAppearingInShows = await personRepo.ListAppearingInAny(shows.Select(show => show.Id), cancellationToken);

			var results = showAdapter.ToContracts(shows, personsAppearingInShows);
			return new GetShowsResponse(pageIndex: pageIndex, results);
		});
	}
}
