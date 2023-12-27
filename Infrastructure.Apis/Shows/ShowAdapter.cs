using RtlTimo.InterviewDemo.Domain.Productions;
using RtlTimo.InterviewDemo.Domain.Shared;

namespace RtlTimo.InterviewDemo.Infrastructure.Apis.Shows;

internal sealed class ShowAdapter
{
	public Show FromDto(TvMazeShowDto dto)
	{
		var result = new Show(
			dto.Id,
			new ProperName(dto.Name),
			modificationDateTime: DateTime.UnixEpoch.AddSeconds(dto.Updated));

		return result;
	}
}
