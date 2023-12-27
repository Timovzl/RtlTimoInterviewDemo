namespace RtlTimo.InterviewDemo.Domain;

/// <summary>
/// <para>
/// The stable error codes defined by this bounded context, which can be coded against, even by outer layers or third parties.
/// </para>
/// <para>
/// Names are stable. Numeric values are meaningless.
/// </para>
/// <para>
/// DO NOT DELETE OR RENAME ITEMS.
/// </para>
/// </summary>
public enum ErrorCode
{
	// DO NOT DELETE OR RENAME ITEMS

	ExternalId_ValueNull,
	ExternalId_ValueEmpty,
	ExternalId_ValueToolong,
	ExternalId_ValueInvalid,

	// DO NOT DELETE OR RENAME ITEMS
}
