namespace RtlTimo.InterviewDemo.Domain.Shared;

/// <summary>
/// A name used for an individual person, place, organization, or the like.
/// </summary>
[WrapperValueObject<string>]
public sealed partial class ProperName : IComparable<ProperName>
{
	protected override StringComparison StringComparison => StringComparison.OrdinalIgnoreCase;

	public const ushort MaxLength = 255;

	public string Value { get; private init; }

	public ProperName(string value)
	{
		this.Value = value ?? throw new NullValidationException(ErrorCode.ProperName_ValueNull, nameof(value));

		if (String.IsNullOrWhiteSpace(this.Value))
			throw new ValidationException(ErrorCode.ProperName_ValueTooShort, $"A {nameof(ProperName)} must not be empty.");
		if (this.Value.Length > MaxLength)
			throw new ValidationException(ErrorCode.ProperName_ValueTooLong, $"A {nameof(ProperName)} must not be over {MaxLength} characters long.");
		if (ContainsNonPrintableCharacters(this.Value, flagNewLinesAndTabs: true))
			throw new ValidationException(ErrorCode.ProperName_ValueInvalid, $"A {nameof(ProperName)} must contain only printable characters.");
	}
}
