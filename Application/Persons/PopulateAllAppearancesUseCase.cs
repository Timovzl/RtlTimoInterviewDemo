using Architect.AmbientContexts;
using Architect.DomainModeling;
using Architect.EntityFramework.DbContextManagement;
using Prometheus;
using RtlTimo.InterviewDemo.Application.Shows;

namespace RtlTimo.InterviewDemo.Application.Persons;

/// <summary>
/// Intended to initially populate all <see cref="Appearance"/> data, along with corresponding <see cref="Show"/> and <see cref="Person"/> data.
/// </summary>
public sealed class PopulateAllAppearancesUseCase(
	ILogger<PopulateAllAppearancesUseCase> logger,
	IShowSource showSource,
	IDbContextProvider<ICoreDatabase> dbContextProvider,
	IShowRepo showRepo)
	: IApplicationService
{
	public static readonly Counter ShowCount = Metrics.CreateCounter("ShowCount", "The number of shows in the system");
	public static readonly Counter PersonCount = Metrics.CreateCounter("PersonCount", "The number of persons in the system");

	public async Task PopulateAllAppearances(CancellationToken cancellationToken)
	{
		await this.ThrowIfDataSetNotEmpty(cancellationToken);

		logger.LogInformation("First time populating all shows, persons, and appearances");
		logger.LogInformation("Delete the database to be able to re-run this one-time process, as it merely exists to speed things up as compared to incremental updates");

		// Populate shows
		await this.PopulateShows(cancellationToken);

		await dbContextProvider.ExecuteInDbContextScopeAsync(cancellationToken, async (executionScope, cancellationToken) =>
		{
			var knownPersonIds = new HashSet<PersonId>();

			await using var showEnumerator = showRepo.Enumerate().WithCancellation(cancellationToken).GetAsyncEnumerator();
			while (await showEnumerator.MoveNextAsync())
			{
				// Per batch of shows
				var showBatch = new List<Show>(capacity: 100) { showEnumerator.Current };
				while (showBatch.Count < showBatch.Capacity && await showEnumerator.MoveNextAsync())
					showBatch.Add(showEnumerator.Current);

				// Populate corresponding persons and appearances
				await this.PopulatePersonsAndAppearances(knownPersonIds, showBatch, cancellationToken);
			}
		});
	}

	private async Task ThrowIfDataSetNotEmpty(CancellationToken cancellationToken)
	{
		await dbContextProvider.ExecuteInDbContextScopeAsync(cancellationToken, async (executionScope, cancellationToken) =>
		{
			if (await showRepo.Any(cancellationToken))
				throw new InvalidOperationException("To re-run the fast initial population, delete the database and restart the application.");
		});
	}

	/// <summary>
	/// Populates all <see cref="Show"/> instances and returns the complete list of <see cref="Show.SourceId"/> values.
	/// </summary>
	private async Task PopulateShows(CancellationToken cancellationToken)
	{
		var cumulativeCount = 0;

		await using var showEnumerator = showSource.EnumerateAllShows(cancellationToken).GetAsyncEnumerator(cancellationToken);
		while (await showEnumerator.MoveNextAsync())
		{
			// Per batch of shows
			var showBatch = new List<Show>(capacity: 1000) { showEnumerator.Current };
			while (showBatch.Count < showBatch.Capacity && await showEnumerator.MoveNextAsync())
				showBatch.Add(showEnumerator.Current);

			// Store the batch
			await dbContextProvider.ExecuteInDbContextScopeAsync(cancellationToken, async (executionScope, cancellationToken) =>
			{
				executionScope.DbContext.AddRange(showBatch);
				await executionScope.DbContext.SaveChangesAsync(cancellationToken);
			});

			cumulativeCount += showBatch.Count;

			ShowCount.IncTo(cumulativeCount);

			logger.LogInformation("Imported {Count} shows ({CumulativeCount} cumulative)", showBatch.Count, cumulativeCount);
		}
	}

	private async Task PopulatePersonsAndAppearances(HashSet<PersonId> knownPersonIds, IEnumerable<Show> shows, CancellationToken cancellationToken)
	{
		var newPersons = new List<Person>(capacity: 1024);
		var appearances = new HashSet<Appearance>(capacity: 1024);

		// Per show, get the appearances and yet-unseen persons
		foreach (var show in shows)
		{
			var persons = await showSource.GetCastForShow(show.Id, cancellationToken);

			foreach (var person in persons)
			{
				if (knownPersonIds.Add(person.Id))
					newPersons.Add(person);

				appearances.Add(new Appearance(person.Id, show.Id));
			}
		}

		// Store the appearances and newly discovered persons
		await dbContextProvider.ExecuteInDbContextScopeAsync(AmbientScopeOption.ForceCreateNew, cancellationToken, async (executionScope, cancellationToken) =>
		{
			executionScope.DbContext.AddRange(newPersons);
			executionScope.DbContext.AddRange(appearances);
			await executionScope.DbContext.SaveChangesAsync(cancellationToken);
		});

		PersonCount.IncTo(knownPersonIds.Count);

		logger.LogInformation("Imported {Count} persons ({CumulativeCount} cumulative)", newPersons.Count, knownPersonIds.Count);
		logger.LogInformation("Imported {Count} appearances", appearances.Count);
	}
}
