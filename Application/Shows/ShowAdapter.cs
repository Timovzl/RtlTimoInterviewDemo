using RtlTimo.InterviewDemo.Contracts.Shows.V1;

namespace RtlTimo.InterviewDemo.Application.Shows;

public sealed class ShowAdapter
{
	public IReadOnlyCollection<ShowContract> ToContracts(IEnumerable<Show> shows, ILookup<ShowId, Person> personsAppearingInShows)
	{
		var result = shows
			.Select(show => new ShowContract(
				id: (uint)show.Id,
				name: show.Name,
				cast: this.ToContracts(personsAppearingInShows[show.Id])))
			.ToList();

		return result;
	}

	public IReadOnlyList<CastMemberContract> ToContracts(IEnumerable<Person> persons)
	{
		var result = persons
			.OrderBy(person => person.DateOfBirth)
			.Select(person => new CastMemberContract(
				id: (uint)person.Id,
				name: person.Name,
				birthday: person.DateOfBirth))
			.ToList();

		return result;
	}
}
