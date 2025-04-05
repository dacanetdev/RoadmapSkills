# RoadmapSkills

A .NET 8.0 Clean Architecture project implementing a modular monolith design for managing user roadmap skills.

## Project Structure

The solution follows Clean Architecture principles with a modular monolith approach:

```
src/
├── Modules/
│   └── Users/
│       ├── DeventSoft.RoadmapSkills.Users.Domain
│       ├── DeventSoft.RoadmapSkills.Users.Application
│       ├── DeventSoft.RoadmapSkills.Users.Infrastructure
│       └── DeventSoft.RoadmapSkills.Users.Infrastructure.Abstractions
└── Shared/
    ├── DeventSoft.RoadmapSkills.Shared.Domain
    └── DeventSoft.RoadmapSkills.Shared.Infrastructure

tests/
└── Modules/
    └── Users/
        ├── DeventSoft.RoadmapSkills.Users.UnitTests
        └── DeventSoft.RoadmapSkills.Users.IntegrationTests
```

## Latest Changes

### Users Module Implementation (2024-03-21)
- Implemented User entity and repository
- Added UserService with CRUD operations
- Created unit tests for UserService with 100% coverage
- Added test coverage configuration using coverlet
- All tests passing successfully

## Test Coverage

Latest test run results:
- Total Tests: 8
- Passed: 8
- Failed: 0
- Skipped: 0
- Duration: 2.0s

## Development Setup

1. Clone the repository
2. Ensure you have .NET 8.0 SDK installed
3. Run `dotnet restore` to restore dependencies
4. Run `dotnet build` to build the solution
5. Run `dotnet test` to execute the tests with coverage

## Technologies Used

- .NET 8.0 (LTS)
- Entity Framework Core
- xUnit for testing
- NSubstitute for mocking
- Coverlet for code coverage 