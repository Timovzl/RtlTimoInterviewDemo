namespace RtlTimo.InterviewDemo.Contracts.Shows.V1;

/// <summary>
/// A TV show, along with additional information.
/// </summary>
public class ShowContract
{
	public ulong Id { get; set; }
	public string Name { get; set; } = null!;
	public IReadOnlyList<CastMemberContract> Cast { get; set; } = null!;

	public ShowContract()
	{
	}

	internal ShowContract(ulong id, string name, IReadOnlyList<CastMemberContract> cast)
	{
		this.Id = id;
		this.Name = name;
		this.Cast = cast ?? throw new ArgumentNullException(nameof(cast));
	}
}
