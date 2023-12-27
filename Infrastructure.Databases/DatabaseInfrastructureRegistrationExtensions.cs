using Architect.EntityFramework.DbContextManagement;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RtlTimo.InterviewDemo.Application;

namespace RtlTimo.InterviewDemo.Infrastructure.Databases;

public static class DatabaseInfrastructureRegistrationExtensions
{
	public static IServiceCollection AddDatabaseInfrastructureLayer(this IServiceCollection services, IConfiguration config)
	{
		services.AddPooledDbContextFactory<CoreDbContext>(dbContext => dbContext
			.UseSqlServer(config.GetConnectionString("CoreDatabase")!, sqlServer => sqlServer.EnableRetryOnFailure()));

		services.AddDbContextScope<ICoreDatabase, CoreDbContext>(scope => scope
			.ExecutionStrategyOptions(ExecutionStrategyOptions.RetryOnOptimisticConcurrencyFailure)
			.AvoidFailureOnCommitRetries(true));

		// Register the current project's dependencies
		services.Scan(scanner => scanner.FromAssemblies(typeof(DatabaseInfrastructureRegistrationExtensions).Assembly)
			.AddClasses(c => c.Where(type => !type.Name.Contains('<') && !type.IsNested && !type.Name.EndsWith("Interceptor")), publicOnly: false)
			.AsSelfWithInterfaces().WithSingletonLifetime());

		return services;
	}

	/// <summary>
	/// Causes all relevant database migrations to be applied on host startup, in a concurrency-safe manner.
	/// </summary>
	public static IServiceCollection AddDatabaseMigrations(this IServiceCollection services)
	{
		services.AddSingleton<MigrationAssistant<CoreDbContext>>();
		services.AddSingleton<IHostedService>(serviceProvider => serviceProvider.GetRequiredService<MigrationAssistant<CoreDbContext>>());

		return services;
	}
}
