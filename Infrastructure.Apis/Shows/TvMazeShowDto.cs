namespace RtlTimo.InterviewDemo.Infrastructure.Apis.Shows;

internal sealed class TvMazeShowDto
{
	public uint Id { get; set; }
	public string Name { get; set; } = null!;
	public DateOnly? Premiered { get; set; }
	public DateOnly? Ended { get; set; }
	public ulong Updated { get; set; }
}
