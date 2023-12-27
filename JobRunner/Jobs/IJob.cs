using Hangfire;

namespace RtlTimo.InterviewDemo.JobRunner.Jobs;

public interface IJob
{
	string CronSchedule { get; }

	[DisableConcurrentExecution(timeoutInSeconds: 5 * 60)]
	Task Execute(CancellationToken cancellationToken);
}
