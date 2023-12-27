using Hangfire;

namespace RtlTimo.InterviewDemo.JobRunner.Shared;

/// <summary>
/// Do not inject this type directly. Instead, inject <see cref="IJob"/>.
/// </summary>
public abstract class Job : IJob
{
	public abstract string CronSchedule { get; }

	[DisableConcurrentExecution(timeoutInSeconds: 5 * 60)]
	public abstract Task Execute(CancellationToken cancellationToken);
}
