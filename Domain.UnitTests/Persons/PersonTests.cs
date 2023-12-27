namespace RtlTimo.InterviewDemo.Domain.UnitTests.Persons;

public sealed class PersonTests
{
	[Fact]
	public void Construct_WithNullSourceId_ShouldThrow()
	{
		var exception = Assert.Throws<NullValidationException>(() => new PersonDummyBuilder().WithSourceId(null!).Build());
		Assert.Equal("Show_SourceIdNull", exception.ErrorCode); // Note that hardcoding safeguards error code stability
	}

	[Fact]
	public void Construct_WithNullName_ShouldThrow()
	{
		var exception = Assert.Throws<NullValidationException>(() => new PersonDummyBuilder().WithName(null!).Build());
		Assert.Equal("Show_NameNull", exception.ErrorCode);
	}

	[Fact]
	public void Construct_Regularly_ShouldProduceExpectedResult()
	{
		var person = new PersonDummyBuilder()
			.WithSourceId("One")
			.WithName("Petey Piranha 💩")
			.WithDateOfBirth(new DateOnly(2000, 01, 01))
			.WithModificationDateTime(DateTime.UnixEpoch)
			.Build();

		Assert.NotEqual(default, person.Id);
		Assert.Equal("One", person.SourceId);
		Assert.Equal("Petey Piranha 💩", person.Name);
		Assert.Equal(new DateOnly(2000, 01, 01), person.DateOfBirth);
		Assert.Equal(DateTime.UnixEpoch, person.ModificationDateTime);
	}
}
