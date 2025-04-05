# RoadmapSkills

A .NET 8.0 Clean Architecture project implementing a modular monolith design for managing developer roadmap skills. This application helps developers track their progress along various technology learning paths and skill development roadmaps.

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

## Prerequisites

- .NET 8.0 SDK or later
- Visual Studio 2022 or VS Code
- SQL Server (LocalDB or higher) for development

## Getting Started

1. Clone the repository
```bash
git clone https://github.com/dacanetdev/RoadmapSkills.git
```

2. Navigate to the project directory
```bash
cd RoadmapSkills
```

3. Build the solution
```bash
dotnet build
```

4. Run the tests
```bash
dotnet test
```

5. Run the API project
```bash
cd src/DeventSoft.RoadmapSkills.Api
dotnet run
```

## Features

- Track progress on different technology roadmaps
- Set learning goals and milestones
- Record completed skills and certifications
- Generate progress reports
- Share progress with mentors or team leads

## Architecture

This project follows Clean Architecture principles:

- Domain Layer: Contains enterprise logic and types
- Application Layer: Contains business logic and interfaces
- Infrastructure Layer: Contains implementation details and external concerns
- API Layer: Contains API controllers and models

## Technologies Used

- .NET 8.0 (LTS)
- Entity Framework Core
- xUnit for testing
- NSubstitute for mocking
- Coverlet for code coverage

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## License

This project is licensed under the MIT License - see the LICENSE file for details.
