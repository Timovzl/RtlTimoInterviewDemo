using Hangfire;
using Hangfire.SqlServer;
using Microsoft.Data.SqlClient;

namespace RtlTimo.InterviewDemo.JobRunner;

public static class HangfireRegistrationExtensions
{
	/// <summary>
	/// Configures a Hangfire client, i.e. an application that can register and enqueue jobs.
	/// (Note that a Hangfire <em>server</em>, which <em>runs</em> the jobs, also acts as a client.)
	/// </summary>
	public static void ConfigureHangfire(this IGlobalConfiguration globalConfiguration, string connectionString)
	{
		globalConfiguration
			.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
			.UseSimpleAssemblyNameTypeSerializer()
			.UseRecommendedSerializerSettings()
			.UseSerilogLogProvider()
			.UseSqlServerStorage(() => new SqlConnection(connectionString), new SqlServerStorageOptions()
			{
				CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
				SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
				UseRecommendedIsolationLevel = true,
				DisableGlobalLocks = true,
				QueuePollInterval = TimeSpan.FromSeconds(10),
			});
	}
}
