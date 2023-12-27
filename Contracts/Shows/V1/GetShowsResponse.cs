namespace RtlTimo.InterviewDemo.Contracts.Shows.V1;

/// <summary>
/// <para>
/// Response to a <see cref="GetShowsRequest"/>
/// </para>
/// </summary>
public class GetShowsResponse
{
	/// <summary>
	/// The 0-based page index.
	/// </summary>
	public ushort PageIndex { get; set; }

	public IReadOnlyCollection<ShowContract> Shows { get; set; } = null!;

	public GetShowsResponse()
	{
	}

	internal GetShowsResponse(ushort pageIndex, IReadOnlyCollection<ShowContract> shows)
	{
		this.PageIndex = pageIndex;
		this.Shows = shows ?? throw new ArgumentNullException(nameof(shows));
	}
}
