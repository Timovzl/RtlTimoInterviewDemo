using Architect.EntityFramework.DbContextManagement;
using RtlTimo.InterviewDemo.Domain.Productions;

namespace RtlTimo.InterviewDemo.Infrastructure.Databases.Productions;

public sealed class ShowRepo(
	IDbContextAccessor<CoreDbContext> dbContextAccessor)
	: Repository<Show>(dbContextAccessor),
	IShowRepo
{
	protected override IQueryable<Show> AggregateQueryable => this.DbContext.Shows;

	public Task<bool> Any(CancellationToken cancellationToken)
	{
		return this.AggregateQueryable.AnyAsync(cancellationToken);
	}

	public IAsyncEnumerable<Show> EnumerateReverseChronologically()
	{
		return this.AggregateQueryable
			.OrderByDescending(x => x.EndDate)
			.OrderByDescending(x => x.PremierDate)
			.AsNoTracking().AsAsyncEnumerable();
	}

	public async Task<IReadOnlyList<Show>> ListPaged(int pageSize, ushort pageIndex, CancellationToken cancellationToken)
	{
		var result = await this.AggregateQueryable
			.OrderByDescending(x => x.SourceId)
			.Skip(pageIndex * pageSize)
			.Take(pageSize)
			.ToListAsync(cancellationToken);

		return result;
	}
}
