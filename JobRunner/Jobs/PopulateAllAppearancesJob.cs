using RtlTimo.InterviewDemo.Application.Appearances;

namespace RtlTimo.InterviewDemo.JobRunner.Jobs;

internal sealed class PopulateAllAppearancesJob(
	PopulateAllAppearancesUseCase useCase)
	: Job
{
	public override string CronSchedule => "0 0 31 2 *"; // Never (Feb 31), i.e. manually only

	public override Task Execute(CancellationToken cancellationToken)
	{
		return useCase.PopulateAllAppearances(cancellationToken);
	}
}
