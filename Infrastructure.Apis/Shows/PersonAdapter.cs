using RtlTimo.InterviewDemo.Domain.Persons;
using RtlTimo.InterviewDemo.Domain.Shared;

namespace RtlTimo.InterviewDemo.Infrastructure.Apis.Shows;

internal sealed class PersonAdapter
{
	public Person FromDto(TvMazePersonDto dto)
	{
		var result = new Person(
			new ExternalId(dto.Id.ToString()),
			new ProperName(dto.Name),
			dateOfBirth: dto.Birthday,
			modificationDateTime: DateTime.UnixEpoch.AddSeconds(dto.Updated));

		return result;
	}
}
