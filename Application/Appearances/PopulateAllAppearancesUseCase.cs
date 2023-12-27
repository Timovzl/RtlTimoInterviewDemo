using Architect.AmbientContexts;
using Architect.DomainModeling;
using Architect.EntityFramework.DbContextManagement;
using RtlTimo.InterviewDemo.Application.Shows;
using RtlTimo.InterviewDemo.Domain.Shared;

namespace RtlTimo.InterviewDemo.Application.Appearances;

/// <summary>
/// Intended to initially populate all <see cref="Appearance"/> data, along with corresponding <see cref="Show"/> and <see cref="Person"/> data.
/// </summary>
public sealed class PopulateAllAppearancesUseCase(
	ILogger<PopulateAllAppearancesUseCase> logger,
	IDbContextProvider<ICoreDatabase> dbContextProvider,
	IShowSource showSource,
	IShowRepo showRepo)
	: IApplicationService
{
	public async Task PopulateAllAppearances(CancellationToken cancellationToken)
	{
		logger.LogInformation("First time populating all shows, persons, and appearances");
		logger.LogInformation("Delete the database to be able to re-run this one-time process, as it merely exists to speed things up as compared to incremental updates");

		// Populate shows
		await this.PopulateShows(cancellationToken);

		var knownPersonIdsBySourceId = new Dictionary<ExternalId, PersonId>();

		// Per batch of shows
		await dbContextProvider.ExecuteInDbContextScopeAsync(cancellationToken, async (executionScope, cancellationToken) =>
		{
			await using var showEnumerator = showRepo.EnumerateReverseChronologically().WithCancellation(cancellationToken).GetAsyncEnumerator();
			while (await showEnumerator.MoveNextAsync())
			{
				var showBatch = new List<Show>(capacity: 100) { showEnumerator.Current };
				while (showBatch.Count < showBatch.Capacity && await showEnumerator.MoveNextAsync())
					showBatch.Add(showEnumerator.Current);

				// Populate corresponding persons and appearances
				await this.PopulatePersonsAndAppearances(knownPersonIdsBySourceId, showBatch, cancellationToken);
			}
		});
	}

	/// <summary>
	/// Populates all <see cref="Show"/> instances and returns the complete list of <see cref="Show.SourceId"/> values.
	/// </summary>
	private async Task PopulateShows(CancellationToken cancellationToken)
	{
		var cumulativeCount = 0;

		// Per batch of shows
		await using var showEnumerator = showSource.EnumerateAllShows(cancellationToken).GetAsyncEnumerator(cancellationToken);
		while (await showEnumerator.MoveNextAsync())
		{
			var showBatch = new List<Show>(capacity: 1000) { showEnumerator.Current };
			while (showBatch.Count < showBatch.Capacity && await showEnumerator.MoveNextAsync())
				showBatch.Add(showEnumerator.Current);

			// Store the batch of shows
			await dbContextProvider.ExecuteInDbContextScopeAsync(cancellationToken, async (executionScope, cancellationToken) =>
			{
				executionScope.DbContext.AddRange(showBatch);
				await executionScope.DbContext.SaveChangesAsync(cancellationToken);
			});

			cumulativeCount += showBatch.Count;

			logger.LogInformation("Imported {Count} shows ({CumulativeCount} cumulative)", showBatch.Count, cumulativeCount);
		}
	}

	private async Task PopulatePersonsAndAppearances(Dictionary<ExternalId, PersonId> knownPersonIdByExternalId, IEnumerable<Show> shows, CancellationToken cancellationToken)
	{
		var newPersons = new List<Person>(capacity: 256);
		var appearances = new HashSet<Appearance>(capacity: 256);
		var cumulativeAppearanceCount = 0;

		// Per show, get the appearances and yet-unseen persons
		foreach (var show in shows)
		{
			var importedPersons = await showSource.GetCastForShow(show.SourceId, cancellationToken);

			foreach (var importedPerson in importedPersons)
			{
				if (!knownPersonIdByExternalId.TryGetValue(importedPerson.SourceId, out var personId))
				{
					knownPersonIdByExternalId[importedPerson.SourceId] = personId = importedPerson.Id;
					newPersons.Add(importedPerson);
				};

				appearances.Add(new Appearance(personId, show.Id));
			}
		}

		// Store the appearances and newly discovered persons
		await dbContextProvider.ExecuteInDbContextScopeAsync(AmbientScopeOption.ForceCreateNew, cancellationToken, async (executionScope, cancellationToken) =>
		{
			executionScope.DbContext.AddRange(newPersons);
			executionScope.DbContext.AddRange(appearances);
			await executionScope.DbContext.SaveChangesAsync(cancellationToken);
		});

		cumulativeAppearanceCount += appearances.Count;

		logger.LogInformation("Imported {Count} persons ({CumulativeCount} cumulative)", newPersons.Count, knownPersonIdByExternalId.Count);
		logger.LogInformation("Imported {Count} appearances ({CumulativeCount} cumulative)", appearances.Count, cumulativeAppearanceCount);
	}
}
