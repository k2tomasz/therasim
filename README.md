# TheraSim

TheraSim is a web-based platform designed for therapeutic training and assessment. It utilizes AI-powered simulations to provide a realistic and interactive environment for users to practice and enhance their therapeutic skills.

## Technologies Used

- **Backend**: .NET 8, ASP.NET Core
- **Frontend**: Blazor WebAssembly
- **Database**: Entity Framework Core with SQL Server (or other configurable database)
- **Authentication**: ASP.NET Core Identity
- **AI**: Integration with large language models (e.g., OpenAI) for simulation and feedback.

## Project Structure

The solution follows the principles of Clean Architecture, separating concerns into distinct projects:

- **`Domain`**: Contains the core business logic and entities of the application. It has no dependencies on other projects in the solution.
- **`Application`**: Implements the application logic, including use cases (commands and queries), and defines interfaces for infrastructure services.
- **`Infrastructure`**: Provides implementations for the interfaces defined in the `Application` layer, such as data access (using Entity Framework Core) and integrations with external services (like AI models).
- **`Web`**: The user interface of the application, built with Blazor. It handles user interactions and presents data to the user.

## Getting Started

To get the project up and running on your local machine, follow these steps:

1. **Prerequisites**:
   - .NET 8 SDK
   - SQL Server (or another database provider)
   - An account with an AI provider (e.g., OpenAI) to obtain an API key.

2. **Configuration**:
   - Clone the repository.
   - Update the database connection string in `appsettings.json` in the `Web` project.
   - Add your AI provider API key to the configuration.

3. **Database Migrations**:
   - Open a terminal in the `Infrastructure` project directory.
   - Run `dotnet ef database update` to apply the database migrations.
   - Alternativly you can inport data/therasim-db.bacpac file to your SQL server. 

4. **Run the Application**:
   - Set the `Web` project as the startup project.
   - Run the application from your IDE or by using the `dotnet run` command in the `Web` project directory.

## Key Features

- **AI-Powered Simulations**: Engage in realistic conversations with AI-powered virtual patients.
- **Assessments**: Participate in structured assessments to evaluate and improve your therapeutic skills.
- **Feedback**: Receive detailed feedback on your performance in simulations and assessments.
- **User Profiles**: Track your progress and review your assessment history.
- **Customizable Scenarios**: Create and customize simulation scenarios to target specific skills.

## Prompts

The application uses a set of predefined prompts to guide the AI in generating responses during simulations and assessments. These prompts are designed to ensure that the AI behaves in a manner consistent with therapeutic practices.
Prompts can be found in the AssessmentTaskLanguage DB table.