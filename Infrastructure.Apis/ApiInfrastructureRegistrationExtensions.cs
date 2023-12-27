using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RtlTimo.InterviewDemo.Infrastructure.Apis;

public static class ApiInfrastructureRegistrationExtensions
{
	public static IServiceCollection AddApiInfrastructureLayer(this IServiceCollection services, IConfiguration _)
	{
		// Register the current project's dependencies
		services.Scan(scanner => scanner.FromAssemblies(typeof(ApiInfrastructureRegistrationExtensions).Assembly)
			.AddClasses(c => c.Where(type => type.Name.EndsWith("er") || type.Name.EndsWith("or") || type.Name.EndsWith("Source") || type.Name.EndsWith("Client")), publicOnly: false) // Services only
			.AsSelfWithInterfaces().WithSingletonLifetime());

		services.AddHttpClient();

		return services;
	}
}
