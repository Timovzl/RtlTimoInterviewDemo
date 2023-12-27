using System.Net;
using Microsoft.EntityFrameworkCore;
using RtlTimo.InterviewDemo.Application.Appearances;
using RtlTimo.InterviewDemo.Domain.Persons;
using RtlTimo.InterviewDemo.Domain.Productions;
using RtlTimo.InterviewDemo.Testing.Common.Http;

namespace RtlTimo.InterviewDemo.Application.IntegrationTests.Appearances;

public sealed class PopulateAllAppearancesUseCaseTests : IntegrationTestBase
{
	[Fact]
	public async Task PopulateAllAppearances_Regularly_ShouldImportExpectedData()
	{
		// Arrange

		var mockHttpClientFactory = HttpClientFactoryMocker.CreateMockHttpClientFactory(out var mockHttpClientHandler);

		this.ConfigureServices(services => services.AddSingleton(mockHttpClientFactory.Object));

		var invocationCountForPagedShows = 0;
		mockHttpClientHandler.SetupSendAsync(
			predicate: message =>
				invocationCountForPagedShows == 0,
			valueFunction: _ => new HttpResponseMessage(HttpStatusCode.TooManyRequests)
			{
				Content = new StringContent(invocationCountForPagedShows++.ToString()),
			});
		mockHttpClientHandler.SetupSendAsync(
			predicate: message =>
				invocationCountForPagedShows > 0 &&
				message.RequestUri!.AbsoluteUri == "https://www.example.com/shows?page=0",
			value: new HttpResponseMessage(HttpStatusCode.OK)
			{
				Content = new StringContent(ExampleShowData.FirstTwo),
			});
		mockHttpClientHandler.SetupSendAsync(
			predicate: message =>
				message.RequestUri!.AbsoluteUri == "https://www.example.com/shows?page=1",
			value: new HttpResponseMessage(HttpStatusCode.OK)
			{
				Content = new StringContent(ExampleShowData.Third),
			});
		mockHttpClientHandler.SetupSendAsync(
			predicate: message =>
				message.RequestUri!.AbsoluteUri.StartsWith("https://www.example.com/shows") &&
				!message.RequestUri!.Query.Contains("page=0") &&
				!message.RequestUri!.Query.Contains("page=1"),
			value: new HttpResponseMessage(HttpStatusCode.NotFound));

		var invocationCountForCast = 0;
		mockHttpClientHandler.SetupSendAsync(
			predicate: message =>
				invocationCountForCast == 0,
			valueFunction: _ => new HttpResponseMessage(HttpStatusCode.TooManyRequests)
			{
				Content = new StringContent(invocationCountForCast++.ToString()),
			});
		mockHttpClientHandler.SetupSendAsync(
			predicate: message =>
				message.RequestUri!.AbsoluteUri == "https://www.example.com/shows/1/cast",
			value: new HttpResponseMessage(HttpStatusCode.OK)
			{
				Content = new StringContent(ExampleCastData.First),
			});
		mockHttpClientHandler.SetupSendAsync(
			predicate: message =>
				message.RequestUri!.AbsoluteUri == "https://www.example.com/shows/2/cast",
			value: new HttpResponseMessage(HttpStatusCode.OK)
			{
				Content = new StringContent(ExampleCastData.Second),
			});
		mockHttpClientHandler.SetupSendAsync(
			predicate: message =>
				message.RequestUri!.AbsoluteUri == "https://www.example.com/shows/3/cast",
			value: new HttpResponseMessage(HttpStatusCode.OK)
			{
				Content = new StringContent(ExampleCastData.Third),
			});

		var instance = this.Host.Services.GetRequiredService<PopulateAllAppearancesUseCase>();

		// Act

		await instance.PopulateAllAppearances(CancellationToken.None);

		// Assert

		await using var dbContextScope = this.CreateDbContextScope();

		var shows = await dbContextScope.DbContext.Set<Show>().ToListAsync();
		var persons = await dbContextScope.DbContext.Set<Person>().ToListAsync();
		var appearances = await dbContextScope.DbContext.Set<Appearance>().ToListAsync();

		Assert.Equal(3, shows.Count);
		Assert.Equal(27, persons.Count);
		Assert.Equal(27, appearances.Count);

		Assert.Equal(1, shows[0].Id);
		Assert.Equal("Under the Dome", shows[0].Name);
		Assert.Equal(DateTime.UnixEpoch.AddSeconds(1631010933), shows[0].ModificationDateTime);

		Assert.Equal(1, appearances[0].PersonId);
		Assert.Equal(1, appearances[0].ShowId);

		Assert.Equal(1, persons[0].Id);
		Assert.Equal("Mike Vogel", persons[0].Name);
		Assert.Equal(new DateOnly(1979, 07, 17), persons[0].DateOfBirth);
		Assert.Equal(DateTime.UnixEpoch.AddSeconds(1700438025), persons[0].ModificationDateTime);
	}
}
