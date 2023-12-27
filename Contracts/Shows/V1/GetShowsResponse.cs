namespace RtlTimo.InterviewDemo.Contracts.Shows.V1;

/// <summary>
/// <para>
/// Response to a <see cref="GetShowsRequest"/>
/// </para>
/// </summary>
public class GetShowsResponse : List<ShowContract> // Would have preferred to have the list as a member, and include metadata such as the page index, but let's follow the spec
{
	public GetShowsResponse()
	{
	}

	internal GetShowsResponse(IReadOnlyCollection<ShowContract> shows)
	{
		this.AddRange(shows ?? throw new ArgumentNullException(nameof(shows)));
	}
}
