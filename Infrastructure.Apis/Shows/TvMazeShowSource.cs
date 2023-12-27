using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RtlTimo.InterviewDemo.Application.Shows;
using RtlTimo.InterviewDemo.Domain.Persons;
using RtlTimo.InterviewDemo.Domain.Productions;

namespace RtlTimo.InterviewDemo.Infrastructure.Apis.Shows;

internal sealed class TvMazeShowSource(
	ILogger<TvMazeShowSource> logger,
	ShowAdapter showAdapter,
	PersonAdapter personAdapter,
	IHttpClientFactory httpClientFactory,
	IConfiguration configuration)
	: IShowSource
{
	private readonly string _baseUrl = configuration["TvMaze:BaseUrl"] ?? throw new Exception("Missing configuration.");
	private readonly string _pagedShowRoute = configuration["TvMaze:PagedShowRoute"] ?? throw new Exception("Missing configuration.");
	private readonly string _showRoute = configuration["TvMaze:ShowRoute"] ?? throw new Exception("Missing configuration.");
	private readonly string _castSubroute = configuration["TvMaze:CastSubroute"] ?? throw new Exception("Missing configuration.");
	private readonly TimeSpan _rateLimitingDelay = TimeSpan.FromMilliseconds(UInt16.Parse(configuration["TvMaze:RateLimitingDelayInMilliseconds"] ?? throw new Exception("Missing configuration.")));

	public async IAsyncEnumerable<Show> EnumerateAllShows([EnumeratorCancellation] CancellationToken cancellationToken)
	{
		var nextPageNumber = 0; // 0-based
		var expectsMoreData = true;

		while (expectsMoreData)
		{
			using var httpClient = httpClientFactory.CreateClient();

			TvMazeShowDto[] showDtos;

			logger.LogTrace("Getting all shows from TvMaze, page {PageNumber}", nextPageNumber);

			try
			{
				showDtos = await httpClient.GetFromJsonAsync<TvMazeShowDto[]>($"{this._baseUrl}/{this._pagedShowRoute}?page={nextPageNumber}", cancellationToken) ??
					throw new InvalidDataException("The API returned an unexpected null result.");
			}
			catch (HttpRequestException e) when (e.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
			{
				logger.LogTrace("Delaying due to rate limiting in TvMaze");
				await Task.Delay(this._rateLimitingDelay, cancellationToken);
				expectsMoreData = true;
				continue; // Retry
			}
			catch (HttpRequestException e) when (e.StatusCode == System.Net.HttpStatusCode.NotFound)
			{
				logger.LogTrace("Finished getting all shows from TvMaze");
				expectsMoreData = false;
				break;
			}
			catch (Exception e)
			{
				logger.LogError(e, "Error getting all shows from TvMaze");
				throw;
			}

			expectsMoreData = showDtos.Length > 0; // Loop until exhausted, without hardcoding against a particular batch size
			nextPageNumber++;

			foreach (var showDto in showDtos)
			{
				var show = showAdapter.FromDto(showDto);
				yield return show;
			}
		}
	}

	public async Task<IReadOnlyCollection<Person>> GetCastForShow(ShowId showId, CancellationToken cancellationToken)
	{
		using var httpClient = httpClientFactory.CreateClient();

		logger.LogTrace("Getting cast for show {ShowId} from TvMaze", showId);

		TvMazeCastingDto[]? castingDtos = null;

		while (castingDtos is null)
		{
			try
			{
				castingDtos = await httpClient.GetFromJsonAsync<TvMazeCastingDto[]>($"{this._baseUrl}/{this._showRoute}/{showId}/{this._castSubroute}", cancellationToken) ??
					throw new InvalidDataException("The API returned an unexpected null result.");
			}
			catch (HttpRequestException e) when (e.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
			{
				logger.LogTrace("Delaying due to rate limiting in TvMaze");
				await Task.Delay(this._rateLimitingDelay, cancellationToken);
				continue; // Retry
			}
			catch (Exception e)
			{
				logger.LogError(e, "Error getting cast for show {ShowId} from TvMaze", showId);
				throw;
			}
		}

		var results = castingDtos
			.Select(castingDto => personAdapter.FromDto(castingDto.Person))
			.ToList();

		return results;
	}
}
