using RtlTimo.InterviewDemo.Application.Persons;

namespace RtlTimo.InterviewDemo.JobRunner.Jobs;

internal sealed class ImportPersonAndAppearanceIncrementJob(
	ImportPersonAndAppearanceIncrementsUseCase useCase)
	: Job
{
	public override string CronSchedule => "0 * * * *"; // Every hour at minute 0

	public override Task Execute(CancellationToken cancellationToken)
	{
		return useCase.ImportPersonAndAppearanceIncrements(cancellationToken);
	}
}
