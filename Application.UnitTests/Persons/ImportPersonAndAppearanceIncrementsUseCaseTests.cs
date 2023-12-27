using Architect.EntityFramework.DbContextManagement;
using Microsoft.Extensions.Logging;
using Moq;
using RtlTimo.InterviewDemo.Application.Persons;
using RtlTimo.InterviewDemo.Application.Shows;
using RtlTimo.InterviewDemo.Domain.Productions;
using RtlTimo.InterviewDemo.Infrastructure.Databases;

namespace RtlTimo.InterviewDemo.Application.UnitTests.Persons;

public sealed class ImportPersonAndAppearanceIncrementsUseCaseTests
{
	private readonly PopulateAllAppearancesUseCase _instance;

	private readonly Mock<ILogger<PopulateAllAppearancesUseCase>> _logger;
	private readonly Mock<IShowSource> _showSource;
	private readonly MockDbContextProvider<ICoreDatabase, CoreDbContext> _contextProvider;
	private readonly Mock<IShowRepo> _showRepo;

	public ImportPersonAndAppearanceIncrementsUseCaseTests()
	{
		this._logger = new Mock<ILogger<PopulateAllAppearancesUseCase>>();
		this._showSource = new Mock<IShowSource>();
		this._contextProvider = new MockDbContextProvider<ICoreDatabase, CoreDbContext>();
		this._showRepo = new Mock<IShowRepo>();

		this._instance = new PopulateAllAppearancesUseCase(
			this._logger.Object,
			this._showSource.Object,
			this._contextProvider,
			this._showRepo.Object);
	}

	[Fact]
	public async Task PopulateAllAppearances_WithExistingShows_ShouldThrow()
	{
		this._showRepo.Setup(x => x.Any(CancellationToken.None))
			.ReturnsAsync(true);

		await Assert.ThrowsAsync<InvalidOperationException>(() => this._instance.PopulateAllAppearances(CancellationToken.None));

		this._showRepo.Verify(x => x.Any(CancellationToken.None), Times.Once);
		this._showRepo.VerifyNoOtherCalls();

		this._showSource.VerifyNoOtherCalls();
	}
}
