using RtlTimo.InterviewDemo.Application.Shows;

namespace RtlTimo.InterviewDemo.JobRunner.Jobs;

internal sealed class ImportShowIncrementJob(
	ImportShowIncrementsUseCase useCase)
	: Job
{
	public override string CronSchedule => "30 * * * *"; // Every hour at minute 30

	public override Task Execute(CancellationToken cancellationToken)
	{
		return useCase.ImportShowIncrements(cancellationToken);
	}
}
