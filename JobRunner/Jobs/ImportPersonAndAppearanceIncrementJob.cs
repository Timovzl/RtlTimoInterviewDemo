namespace RtlTimo.InterviewDemo.JobRunner.Jobs;

internal sealed class ImportPersonAndAppearanceIncrementJob()
	: Job
{
	public override string CronSchedule => "0 * * * *"; // Every hour at minute 0

	public override Task Execute(CancellationToken cancellationToken)
	{
		// #TODO
		return Task.CompletedTask;
	}
}
