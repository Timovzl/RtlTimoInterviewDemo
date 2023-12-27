using Architect.EntityFramework.DbContextManagement;
using RtlTimo.InterviewDemo.Domain.Persons;
using RtlTimo.InterviewDemo.Domain.Productions;

namespace RtlTimo.InterviewDemo.Infrastructure.Databases.Productions;

public sealed class PersonRepo(
	IDbContextAccessor<CoreDbContext> dbContextAccessor)
	: Repository<Person>(dbContextAccessor),
	IPersonRepo
{
	protected override IQueryable<Person> AggregateQueryable => this.DbContext.Persons;

	public async Task<ILookup<ShowId, Person>> ListAppearingInAny(IEnumerable<ShowId> showIds, CancellationToken cancellationToken)
	{
		var result = await this.AggregateQueryable
			.Join(this.DbContext.Appearances, person => person.Id, appearance => appearance.PersonId, (person, appearance) => new { Person = person, Appearance = appearance })
			.Where(pair => showIds.Contains(pair.Appearance.ShowId))
			.ToListAsync(cancellationToken);

		return result.ToLookup(pair => pair.Appearance.ShowId, pair => pair.Person);
	}
}
