namespace RtlTimo.InterviewDemo.Contracts.Shows.V1;

/// <summary>
/// A member of the cast of a production.
/// </summary>
public class CastMemberContract
{
	public ulong Id { get; set; }
	public string Name { get; set; } = null!;
	public DateOnly? Birthday { get; set; }

	public CastMemberContract()
	{
	}

	internal CastMemberContract(ulong id, string name, DateOnly? birthday)
	{
		this.Id = id;
		this.Name = name ?? throw new ArgumentNullException(nameof(name));
		this.Birthday = birthday;
	}
}
