# InterviewDemo

This bounded context is a demo for Timo's interview at RTL.
It collects and exposes information in the domain of TV productions.

## Getting Started

Dependencies:

- SQL Server LocalDB: Should be started automatically when accessed.

To get started with the Swagger API, simply clone and run.
Next, to allow the API to provide actual results, start the JobRunner once and run its PopulateInitialAppearancesJob.
For testing purposes, feel free to interrupt the job to play with an incomplete data set.

Database migrations are performed on startup in a concurrency-safe way.
If so desired, this mechanism could easily be swapped out for something like migrations run from a pipeline.

## Summary

The Api appliction exposes information relevant to the domain.
It is obtained from a third-party source by the JobRunner application.

A manually started job obtains the initial data set once.
Periodic jobs efficiently pull incremental updates.

## Architecture

The architecture takes the form of a DDD bounded context, adhering to both Clean Architecture and Hexagonal Architecture (ports & adapters).

## Scalability

- The API can scale _out_ without issue.
- The JobRunner can scale out, but should hardly need to. Large batches use async enumeration to avoid excessive RAM usage. Scaling out is possible because Hangfire's `DisableConcurrentExecution` provides limited concurrency protection, with EF's optimistic concurrency plus retry-on-failure providing a more thorough protection.
- The database can scale _up_. The data set is more than sufficiently limited for this to suffice. The workload is read-heavy. Expected usage patterns will make good use of the database's cache.

## Resilience & Consistency

The bounded context is ready for high availability, thanks to health checks and the ability to scale out.
All required infra resources are available with an impressive number of nines at reasonable prices (99.95% for Azure App Services, 99.995% for Azure SQL Database Premium).

Even transient failures should rarely be felt, thanks to EF's `EnableRetryOnFailure` and the use of execution strategies: failures to reach the database lead to opaque retries with gradual backoff.

Concurrency conflicts are detected optimistically, with similar opaque retries to resolve them.

## Contracts

The contracts optimize for flexibility: additions to any output are free, and optional additions to any input are free.
To support breaking changes, the contracts are versioned per functional area.
A breaking change requires only a new version of that area.

## Repositories

Although some like to use Entity Framework (EF) directly from application services, EF does not provide a full repository pattern.
A clean repository pattern has functional interfaces with reusable methods.
It abstracts away the "how" (e.g. a complex EF LINQ query) from the "what", which application services should focus on.

The [Architect.EntityFramework.DbContextManagement](https://github.com/TheArchitectDev/Architect.EntityFramework.DbContextManagement) package makes it easy to manage DbContext lifespans across use cases and repositories, while facilitating resilience and concurrency protection at the same time.

## Testing

Integration tests cover uses cases, with actual database interaction and in-memory HTTP server interaction, where applicable.
This provides great coverage at minimal effort.
Such tests run in isolation, each creating and deleting their own schema on the fly.

Unit tests cover additional details.

All tests can be run both locally and in a pipeline.

## Pipelines

For lack of time and information regarding RTL's infrastructure, the implementation of pipelines has been left out-of-scope.

Here is a general outline of the intent:

- Use Terraform to manage resources, with variables handling differences between environments.
- Use yaml pipelines with Azure DevOps to deploy to Kubernetes via rolling deployments, adjusted carefully to the bounded context's needs.
- Manage secrets with Azure Key Vault or a comparable offering. Ideally, these are loaded into the application on startup with the help of Managed Identities, to avoid the many security pitfalls of moving secrets through pipelines.
- Run a verification pipeline when a PR is opened. Deploy to the various environments as early and automatically as can be afforded.

## Future Improvements

- Authentication/authorization, rate limiting, and/or a Web Application Firewall (WAF) have been left out-of-scope.
- Paging based on a page index is brittle. Data changes may cause items to be skipped or repeated when a caller is between pages. When there is more time, the API could be redesigned to circumvent this issue.
- Rate limiting when consuming the TvMaze API could be made smarter and application-wide, which is relevant once multiple jobs _might_ touch it at the same time.
