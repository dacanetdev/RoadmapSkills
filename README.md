# RoadmapSkills

A modular monolith application for managing developer roadmaps and skills.

## Architecture

The application follows a modular monolith architecture with the following key components:

- **Host API (`DeventSoft.RoadmapSkills.Api`)**: The main entry point of the application that:
  - Manages Swagger configuration
  - Handles authentication/authorization
  - Registers all module endpoints
  - Manages common middleware

- **Modules**: Independent business modules that:
  - Define their own domain models, services, and infrastructure
  - Use extension methods for service and endpoint registration
  - Maintain clean separation of concerns
  - Current modules:
    - Users: Handles user management and authentication

## Project Structure

```
src/
├── Host/
│   └── DeventSoft.RoadmapSkills.Api/      # Main application host
├── Modules/
│   └── Users/                             # Users module
│       ├── DeventSoft.RoadmapSkills.Users.Api/
│       ├── DeventSoft.RoadmapSkills.Users.Application/
│       ├── DeventSoft.RoadmapSkills.Users.Domain/
│       ├── DeventSoft.RoadmapSkills.Users.Infrastructure/
│       └── DeventSoft.RoadmapSkills.Users.Infrastructure.Abstractions/
└── Shared/                                # Shared components
    └── DeventSoft.RoadmapSkills.Shared.Infrastructure/

tests/
├── Modules/
│   └── Users/                             # Users module tests
│       ├── DeventSoft.RoadmapSkills.Users.Api.Tests/
│       ├── DeventSoft.RoadmapSkills.Users.Application.Tests/
│       └── DeventSoft.RoadmapSkills.Users.Infrastructure.Tests/
└── Shared/                                # Shared component tests
    └── DeventSoft.RoadmapSkills.Shared.Infrastructure.Tests/
```

## Technologies

- .NET 8.0 (LTS)
- FastEndpoints
- Entity Framework Core
- xUnit
- NSubstitute
- Radzen (Blazor UI)

## Getting Started

1. Clone the repository
2. Navigate to the project directory
3. Run `dotnet restore`
4. Run `dotnet build`
5. Run `dotnet run --project src/DeventSoft.RoadmapSkills.Api`

The API will be available at:
- HTTP: http://localhost:5000
- HTTPS: https://localhost:5001

Swagger UI is available at the root URL.

## Development

### Adding a New Module

1. Create a new directory under `src/Modules/`
2. Follow the standard module structure:
   - `.Domain`: Contains domain models and interfaces
   - `.Application`: Contains application services and DTOs
   - `.Infrastructure`: Contains implementations and data access
   - `.Infrastructure.Abstractions`: Contains interfaces for infrastructure services
3. Create corresponding test projects under `tests/Modules/`
4. Register the module in the Host API using extension methods

### Running Tests

```bash
dotnet test
```

To generate a coverage report:

```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura
dotnet reportgenerator -reports:"**/coverage.cobertura.xml" -targetdir:coveragereport -reporttypes:Html
```

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
