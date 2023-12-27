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

	public IAsyncEnumerable<Show> Enumerate()
	{
		return this.AggregateQueryable
			.OrderBy(x => x.Id)
			.AsNoTracking().AsAsyncEnumerable();
	}

	public async Task<IReadOnlyList<Show>> ListPaged(int pageSize, ushort pageIndex, CancellationToken cancellationToken)
	{
		var result = await this.AggregateQueryable
			.OrderBy(x => x.Id) // Debatable, but in accordance with example given in requirements
			.Skip(pageIndex * pageSize)
			.Take(pageSize)
			.ToListAsync(cancellationToken);

		return result;
	}
}
