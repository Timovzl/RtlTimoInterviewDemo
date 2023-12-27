using Architect.DomainModeling;

namespace RtlTimo.InterviewDemo.Application.Persons;

public sealed class ImportPersonAndAppearanceIncrementsUseCase
	: IApplicationService
{
	public Task ImportPersonAndAppearanceIncrements(CancellationToken _)
	{
		// TODO: The next step is to implement this method. The implemented use cases should sufficiently demonstrate the ability to do so correctly and efficiently.
		// This method uses TvMaze's /updates/shows and /updates/people endpoints to obtain recent changes to persons and their appearances in productions.
		// By checking, for example, the latest Person.ModificationDateTime (which is indexed), we can easily identify what data is new.
		// Entities can then be added or updated, and junction ValueObjects (such as Appearance) replaced atomatically per show.

		return Task.CompletedTask;
	}
}
