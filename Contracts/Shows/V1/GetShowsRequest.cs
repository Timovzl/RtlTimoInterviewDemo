namespace RtlTimo.InterviewDemo.Contracts.Shows.V1;

/// <summary>
/// <para>
/// Returns all TV shows in a paginated fashion.
/// </para>
/// <para>
/// Beyond the final page, result sets are simply empty.
/// </para>
/// <para>
/// The effect of data additions modifications in-between the obtaining of pages is undefined.
/// </para>
/// </summary>
public class GetShowsRequest
{
	/// <summary>
	/// The 0-based page index to obtain.
	/// </summary>
	public ushort PageIndex { get; set; }
}
