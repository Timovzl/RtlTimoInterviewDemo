namespace RtlTimo.InterviewDemo.JobRunner.Jobs;

internal sealed class ImportShowIncrementJob()
	: Job
{
	public override string CronSchedule => "30 * * * *"; // Every hour at minute 30

	public override Task Execute(CancellationToken cancellationToken)
	{
		// #TODO
		return Task.CompletedTask;
	}
}
