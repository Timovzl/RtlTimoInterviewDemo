using RtlTimo.InterviewDemo.Application.Shows;
using RtlTimo.InterviewDemo.Contracts.Shows.V1;
using RtlTimo.InterviewDemo.Domain.Persons;

namespace RtlTimo.InterviewDemo.Application.IntegrationTests.Shows;

public sealed class GetShowsUseCaseTests : IntegrationTestBase
{
	[Fact]
	public async Task GetShows_Regularly_ShouldProduceExpectedResult()
	{
		// Arrange

		var oldActor = new PersonDummyBuilder()
			.WithId(10)
			.WithName("Old Actor")
			.WithDateOfBirth("1900-01-01")
			.Build();
		var unknownAgeActor = new PersonDummyBuilder()
			.WithId(15)
			.WithName("Unknown Age Actor 💩")
			.WithDateOfBirth(null)
			.Build();
		var youngActor = new PersonDummyBuilder()
			.WithId(20)
			.WithName("Young Actor")
			.WithDateOfBirth("2000-01-01")
			.Build();

		var show1 = new ShowDummyBuilder()
			.WithId(1)
			.WithName("The First")
			.Build();
		var show2 = new ShowDummyBuilder()
			.WithId(2)
			.WithName("The Second")
			.Build();
		var show3 = new ShowDummyBuilder()
			.WithId(3)
			.WithName("The Third")
			.Build();
		var show4 = new ShowDummyBuilder()
			.WithId(4)
			.WithName("The Fourth")
			.Build();
		var show5 = new ShowDummyBuilder()
			.WithId(5)
			.WithName("The Fifth")
			.Build();

		var appearances = new[]
		{
			new Appearance(oldActor.Id, show4.Id),
			new Appearance(unknownAgeActor.Id, show4.Id),
			new Appearance(youngActor.Id, show4.Id),
		};

		await using (var arrangementDbContextScope = this.CreateDbContextScope())
		{
			arrangementDbContextScope.DbContext.AddRange([oldActor, unknownAgeActor, youngActor,]);
			arrangementDbContextScope.DbContext.AddRange([show1, show2, show3, show4, show5,]);
			arrangementDbContextScope.DbContext.AddRange(appearances);
			await arrangementDbContextScope.DbContext.SaveChangesAsync();
		}

		var instance = this.Host.Services.GetRequiredService<GetShowsUseCase>();
		instance.PageSize = 2;

		// Act

		var requestBeyondTheEnd = new GetShowsRequest()
		{
			PageIndex = 3, // Beyond the last page
		};
		var requestInTheMiddle = new GetShowsRequest()
		{
			PageIndex = 1, // The middle page
		};

		// Go through the (in-memory) HTTP server
		var resultAtTheEnd = await this.GetApiResponse<GetShowsRequest, GetShowsResponse>(HttpMethod.Get, "/Shows/V1/GetShows", requestBeyondTheEnd);
		var resultInTheMiddle = await this.GetApiResponse<GetShowsRequest, GetShowsResponse>(HttpMethod.Get, "/Shows/V1/GetShows", requestInTheMiddle);

		// Assert

		Assert.NotNull(resultAtTheEnd);
		Assert.NotNull(resultAtTheEnd.Shows);
		Assert.Empty(resultAtTheEnd.Shows);

		Assert.NotNull(resultInTheMiddle);
		Assert.Equal(1, resultInTheMiddle.PageIndex);
		Assert.NotNull(resultInTheMiddle.Shows);
		Assert.Equal(2, resultInTheMiddle.Shows.Count);

		var firstShow = resultInTheMiddle.Shows.First();
		Assert.Equal(show3.Id, firstShow.Id);
		Assert.Equal(show3.Name, firstShow.Name);
		Assert.Empty(firstShow.Cast);

		var lastShow = resultInTheMiddle.Shows.Last();
		Assert.Equal(show4.Id, lastShow.Id);
		Assert.Equal(show4.Name, lastShow.Name);
		Assert.Equal(3, lastShow.Cast.Count);

		// Greatest birthday first
		Assert.Equal(youngActor.Id, lastShow.Cast[0].Id);
		Assert.Equal(youngActor.Name, lastShow.Cast[0].Name);
		Assert.Equal(youngActor.DateOfBirth, lastShow.Cast[0].Birthday);

		// Smaller birthday next
		Assert.Equal(oldActor.Id, lastShow.Cast[1].Id);
		Assert.Equal(oldActor.Name, lastShow.Cast[1].Name);
		Assert.Equal(oldActor.DateOfBirth, lastShow.Cast[1].Birthday);

		// Null birthday last
		Assert.Equal(unknownAgeActor.Id, lastShow.Cast[2].Id);
		Assert.Equal(unknownAgeActor.Name, lastShow.Cast[2].Name);
		Assert.Equal(unknownAgeActor.DateOfBirth, lastShow.Cast[2].Birthday);
	}
}
