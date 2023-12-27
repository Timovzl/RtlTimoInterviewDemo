# InterviewDemo

#TODO: Use stable package versions only.

This bounded context is a demo for Timo's interview at RTL.
It collects and exposes information in the domain of TV productions.

## Getting Started

Dependencies:

- SQL Server LocalDB: Should be started automatically when accessed.

To get started with the Swagger API, simply clone and run.
To allow the API to provide actual results, start the JobRunner once and run its PopulateInitialAppearancesJob.
The job prioritizes information that can be obtained from a cache, to provide as much data as quickly as possible.
For testing purposes, feel free to interrupt the job to work with an incomplete data set.

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

TODO

## Resilience & Consistency

TODO

## Testing

Integration tests cover uses cases, with actual database interaction and in-memory HTTP server interaction, where applicable.
This provides great coverage at minimal effort.

Unit tests cover additional details.

All tests can be run both locally and in a pipeline.
