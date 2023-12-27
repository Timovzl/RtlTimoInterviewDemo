using RtlTimo.InterviewDemo.Domain.Productions;
using RtlTimo.InterviewDemo.Domain.Shared;

namespace RtlTimo.InterviewDemo.Infrastructure.Apis.Shows;

internal sealed class ShowAdapter
{
	public Show FromDto(TvMazeShowDto dto)
	{
		var result = new Show(
			new ExternalId(dto.Id.ToString()),
			new ProperName(dto.Name),
			premierDate: dto.Premiered,
			endDate: dto.Ended,
			modificationDateTime: DateTime.UnixEpoch.AddSeconds(dto.Updated));

		return result;
	}
}