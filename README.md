# RoadmapSkills

A C# .NET project to track and manage developer roadmap skills. This application helps developers track their progress along various technology learning paths and skill development roadmaps.

## Project Structure

```
RoadmapSkills/
├── src/                            # Source code
│   ├── DeventSoft.RoadmapSkills.Api/         # Web API project
│   ├── DeventSoft.RoadmapSkills.Core/        # Core business logic
│   ├── DeventSoft.RoadmapSkills.Infrastructure/  # Infrastructure layer
│   └── DeventSoft.RoadmapSkills.Domain/      # Domain models and interfaces
└── tests/                          # Test projects
    ├── DeventSoft.RoadmapSkills.UnitTests/
    └── DeventSoft.RoadmapSkills.IntegrationTests/
```

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
- Core Layer: Contains business logic and interfaces
- Infrastructure Layer: Contains implementation of core business logic
- API Layer: Contains API controllers and models

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## License

This project is licensed under the MIT License - see the LICENSE file for details.