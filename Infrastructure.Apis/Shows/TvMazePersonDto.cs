namespace RtlTimo.InterviewDemo.Infrastructure.Apis.Shows;

internal sealed class TvMazePersonDto
{
	public uint Id { get; set; }
	public string Name { get; set; } = null!;
	public DateOnly? Birthday { get; set; }
	public ulong Updated { get; set; }
}
