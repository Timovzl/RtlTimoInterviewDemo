using Microsoft.AspNetCore.Mvc;
using RtlTimo.InterviewDemo.Application.Shows;
using RtlTimo.InterviewDemo.Contracts.Shows.V1;

namespace RtlTimo.InterviewDemo.Api.Shows;

[ApiController]
[Route("Shows/V1/[action]")]
public sealed class ShowController : ControllerBase
{
	[HttpGet]
	public Task<GetShowsResponse> GetShows([FromQuery] GetShowsRequest request, GetShowsUseCase useCase, CancellationToken cancellationToken)
	{
		return useCase.GetShows(request, cancellationToken);
	}
}
