using Architect.DomainModeling;

namespace RtlTimo.InterviewDemo.Application.Shows;

public sealed class ImportShowIncrementsUseCase
	: IApplicationService
{
	public Task ImportShowIncrements(CancellationToken _)
	{
		// TODO: The next step is to implement this method. The implemented use cases should sufficiently demonstrate the ability to do so correctly and efficiently.
		// This method uses TvMaze's /updates/shows endpoint to obtain recent changes.
		// By checking, for example, the latest Show.ModificationDateTime (which is indexed), we can easily identify what data is new.
		// Entities can then be added or updated.

		return Task.CompletedTask;
	}
}
