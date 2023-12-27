using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using RtlTimo.InterviewDemo.Application;
using RtlTimo.InterviewDemo.Domain.Persons;
using RtlTimo.InterviewDemo.Domain.Productions;

namespace RtlTimo.InterviewDemo.Infrastructure.Databases;

/// <summary>
/// The DbContext for the bounded context's core database.
/// </summary>
public sealed class CoreDbContext(
	DbContextOptions<CoreDbContext> options)
	: DbContext(options), ICoreDatabase
{
	// Note: UTF-8 (e.g. Latin1_General_100_BIN2_UTF8/Latin1_General_100_CI_AS_SC_UTF8) is avoided because its lengths are in bytes (easy to mismatch with validations in .NET) and it requires IsUnicode() and UseColumnType()

	/// <summary>
	/// Our preferred binary collation: a binary, case-sensitive collation that matches .NET's <see cref="StringComparison.Ordinal"/>.
	/// </summary>
	public const string BinaryCollation = "Latin1_General_100_BIN2";
	/// <summary>
	/// <para>
	/// Our preferred culture-sensitive collation: a culture-sensitive, ignore-case, accent-sensitive collation.
	/// </para>
	/// <para>
	/// Use this collation only for non-indexed (or at the very least non-FK) columns, such as titles and descriptions.
	/// </para>
	/// </summary>
	public const string CulturalCollation = "Latin1_General_100_CI_AS";
	/// <summary>
	/// Our default collation, used for textual columns that do not specify one.
	/// </summary>
	public const string DefaultCollation = BinaryCollation;

	public DbSet<Show> Shows { get; private init; }
	public DbSet<Person> Persons { get; private init; }
	public DbSet<Appearance> Appearances { get; private init; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.UseCollation(DefaultCollation);

		modelBuilder.ApplyConfigurationsFromAssembly(typeof(CoreDbContext).Assembly);

		this.PluralizeTableNames(modelBuilder);

		// #TODO: Keep or remove seed data?
		Seeder.AddSeedData(modelBuilder);
	}

	protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
	{
		base.ConfigureConventions(configurationBuilder);

		// We map things explicitly
		configurationBuilder.Conventions.Remove<ConstructorBindingConvention>();
		configurationBuilder.Conventions.Remove<RelationshipDiscoveryConvention>();
		configurationBuilder.Conventions.Remove<PropertyDiscoveryConvention>();

		configurationBuilder.ConfigureDomainModelConventions(domainModel =>
		{
			domainModel.ConfigureIdentityConventions();
			domainModel.ConfigureWrapperValueObjectConventions();
			domainModel.ConfigureEntityConventions();
			domainModel.ConfigureDomainEventConventions();
		});

		configurationBuilder.Properties<DateTime>()
			.HaveConversion<UtcDateTimeConverter>()
			.HavePrecision(3);

		// Configure default precision for decimals outside of properties (e.g. in CAST(), SUM(), AVG(), etc.)
		configurationBuilder.DefaultTypeMapping<decimal>()
			.HasPrecision(19, 9);

		// Configure default precision for decimal properties
		configurationBuilder.Properties<decimal>()
			.HavePrecision(19, 9);

		configurationBuilder.Properties<ExternalId>()
			.HaveMaxLength(ExternalId.MaxLength)
			.UseCollation(BinaryCollation);
	}

	/// <summary>
	/// Ensures that table names are in plural.
	/// Although EF does this automatically where our <see cref="DbSet{TEntity}"/>s are named this way, entities without one (i.e. non-roots) require manual intervention.
	/// </summary>
	private void PluralizeTableNames(ModelBuilder modelBuilder)
	{
		foreach (var entityType in modelBuilder.Model.GetEntityTypes().Where(entityType => !entityType.IsOwned() && entityType.ClrType is not null))
		{
			var clrTypeName = entityType.ClrType!.Name;

			entityType.SetTableName(clrTypeName.EndsWith('y')
				? $"{clrTypeName[..^1]}ies"
				: clrTypeName.EndsWith('s')
				? $"{clrTypeName}es"
				: $"{clrTypeName}s");
		}
	}
}
