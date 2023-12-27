namespace RtlTimo.InterviewDemo.Domain.Productions;

public interface IShowRepo
{
	Task<bool> Any(CancellationToken cancellationToken);

	/// <summary>
	/// Enuemrates all objects in more or less reverse chronological order, i.e. latest first.
	/// </summary>
	IAsyncEnumerable<Show> EnumerateReverseChronologically();

	Task<IReadOnlyList<Show>> ListPaged(int pageSize, ushort pageIndex, CancellationToken cancellationToken);
}
