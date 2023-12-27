namespace RtlTimo.InterviewDemo.Domain.Productions;

public interface IShowRepo
{
	Task<bool> Any(CancellationToken cancellationToken);

	IAsyncEnumerable<Show> Enumerate();

	Task<IReadOnlyList<Show>> ListPaged(int pageSize, ushort pageIndex, CancellationToken cancellationToken);
}
