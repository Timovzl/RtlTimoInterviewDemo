using RtlTimo.InterviewDemo.Application;
using RtlTimo.InterviewDemo.Application.ExceptionHandlers;
using RtlTimo.InterviewDemo.Infrastructure.Databases;
using Microsoft.OpenApi.Models;
using Prometheus;
using Serilog;
using RtlTimo.InterviewDemo.Infrastructure.Apis;

namespace RtlTimo.InterviewDemo.Api;

public static class Program
{
	public static async Task Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		builder.Host.UseSerilog((context, logger) => logger.ReadFrom.Configuration(context.Configuration));

		builder.Services.AddApplicationLayer(builder.Configuration);
		builder.Services.AddApiInfrastructureLayer(builder.Configuration);
		builder.Services.AddDatabaseInfrastructureLayer(builder.Configuration);
		builder.Services.AddDatabaseMigrations();

		builder.Services.AddApplicationControllers();

		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen(swagger =>
		{
			swagger.CustomSchemaIds(type => type.FullName!["RtlTimo.InterviewDemo.Contracts.".Length..]);

			swagger.SupportNonNullableReferenceTypes();
			swagger.SwaggerDoc("v1", new OpenApiInfo()
			{
				Title = "InterviewDemo API",
				Description = """
				<p>This page documents the InterviewDemo API for Timo's interview at RTL.</p>
				""",
			});

			var apiDocumentationFilePath = Path.Combine(AppContext.BaseDirectory, $"{typeof(Program).Assembly.GetName().Name}.xml");
			swagger.IncludeXmlComments(apiDocumentationFilePath);
			var contractsDocumentationFilePath = Path.Combine(AppContext.BaseDirectory, $"{typeof(Contracts.Shows.V1.ShowContract).Assembly.GetName().Name}.xml");
			swagger.IncludeXmlComments(contractsDocumentationFilePath);
		});

		builder.Services.AddHealthChecks();

		var app = builder.Build();
		
		if (builder.Environment.IsDevelopment())
			app.UseDeveloperExceptionPage();

		app.UseHttpsRedirection();

		app.UseExceptionHandler(app => app.Run(async context =>
			await context.RequestServices.GetRequiredService<RequestExceptionHandler>().HandleExceptionAsync()));

		app.UseRouting();

		// Expose a health check endpoint
		app.UseHealthChecks("/health");

		// Expose Prometheus metrics
		app.UseMetricServer();
		app.UseHttpMetrics();

		app.UseSwagger();
		app.UseSwaggerUI();

		app.UseApplicationControllers();

		await app.RunAsync();
	}
}
