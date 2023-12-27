namespace RtlTimo.InterviewDemo.Domain.UnitTests.Persons;

public sealed class PersonTests
{
	[Fact]
	public void Construct_WithNullName_ShouldThrow()
	{
		var exception = Assert.Throws<NullValidationException>(() => new PersonDummyBuilder().WithName(null!).Build());
		Assert.Equal("Person_NameNull", exception.ErrorCode);
	}

	[Fact]
	public void Construct_Regularly_ShouldProduceExpectedResult()
	{
		var person = new PersonDummyBuilder()
			.WithId(2_000_000_000)
			.WithName("Petey Piranha 💩")
			.WithDateOfBirth(new DateOnly(2000, 01, 01))
			.WithModificationDateTime(DateTime.UnixEpoch)
			.Build();

		Assert.Equal(2_000_000_000U, person.Id.Value);
		Assert.Equal("Petey Piranha 💩", person.Name);
		Assert.Equal(new DateOnly(2000, 01, 01), person.DateOfBirth);
		Assert.Equal(DateTime.UnixEpoch, person.ModificationDateTime);
	}
}
