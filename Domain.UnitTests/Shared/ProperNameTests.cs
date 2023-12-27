namespace RtlTimo.InterviewDemo.Domain.UnitTests.Shared;

public sealed class ProperNameTests
{
	[Fact]
	public void Construct_WithNullOrWhitespaceValue_ShouldThrow()
	{
		var exception = Assert.Throws<NullValidationException>(() => new ProperName(null!));
		Assert.Equal("ProperName_ValueNull", exception.ErrorCode);
	}

	[Theory]
	[InlineData("")]
	[InlineData(" ")]
	[InlineData("	")]
	public void Construct_WithEmptyOrWhitespaceValue_ShouldThrow(string? value)
	{
		var exception = Assert.Throws<ValidationException>(() => new ProperName(value!));
		Assert.Equal("ProperName_ValueTooShort", exception.ErrorCode);
	}

	[Fact]
	public void Construct_WithOversizedValue_ShouldThrow()
	{
		var value = new string('A', count: 256);
		var exception = Assert.Throws<ValidationException>(() => new ProperName(value));
		Assert.Equal("ProperName_ValueTooLong", exception.ErrorCode);
	}

	[Theory]
	[InlineData("O")]
	[InlineData("Bre'ette of the ßlafe Ødger")]
	[InlineData("愛子")]
	[InlineData("123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345")]
	public void Construct_Regularly_ShouldProduceExpectedResult(string value)
	{
		var result = new ProperName(value);
		Assert.Equal(value, result.Value);
	}
}
