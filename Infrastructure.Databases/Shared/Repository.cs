namespace RtlTimo.InterviewDemo.Infrastructure.Databases.Shared;

/// <summary>
/// Abstract base class for a repository.
/// </summary>
public abstract class Repository<TEntity>
	where TEntity : class
{
	/// <summary>
	/// <para>
	/// The <see cref="IQueryable{T}"/> used to query the entire aggregate, including all its default includes.
	/// </para>
	/// <para>
	/// All entity-returning methods should query based on this property, because aggregates must be loaded in their entirety.
	/// </para>
	/// </summary>
	protected abstract IQueryable<TEntity> AggregateQueryable { get; }

	protected CoreDbContext DbContext { get; }

	protected Repository(CoreDbContext dbContext)
	{
		this.DbContext = dbContext;
	}
}
