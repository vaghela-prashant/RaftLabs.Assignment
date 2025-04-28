# RaftLabs User Fetcher 🚀
A video walkthrough : https://www.loom.com/share/1a961bcd969d4d91b62d3c1e71462dfa?sid=cb166205-9240-43a9-8bbb-d6e34a56f173
This is a .NET 9 console application that interacts with an external API to fetch user data. It provides functionality to retrieve a user by ID or fetch all users by page. The application is designed with extensibility and caching in mind.

## Features

- Fetch user details by ID.
- Retrieve paginated user data.
- Caching for improved performance.
- Resilient HTTP requests with retry policies using Polly.

## Technologies Used

- **.NET 9**
- **Microsoft.Extensions.DependencyInjection** for dependency injection.
- **Microsoft.Extensions.Caching.Memory** for in-memory caching.
- **Polly** for retry policies.
- **Newtonsoft.Json** for JSON deserialization.
- **xUnit** for unit testing.

## Prerequisites

- .NET 9 SDK installed.
- Internet connection to access the external API.

## Installation

1. Clone the repository: git clone https://github.com/your-repo/raftlabs-user-fetcher.git cd raftlabs-user-fetcher

2. Restore dependencies: dotnet restore

3. Build the project: dotnet build
   
## Configuration

The application uses an `appsettings.json` file for configuration. Ensure the following section is present and updated with your API details:
{ "ExternalApi": { "BaseUrl": "https://reqres.in/api", "ApiKey": "your-api-key" } }

## Usage

1. Run the application: dotnet run --project RaftLabs.Assignment.ConsoleApp
2. Follow the on-screen instructions to:
   - Fetch a user by ID.
   - Retrieve all users by page.

## Testing

Unit tests are included in the `RaftLabs.Assignment.Tests` project. To run the tests: dotnet test


## Project Structure

- **RaftLabs.Assignment.ConsoleApp**: Entry point of the application.
- **RaftLabs.Assignment.Infrastructure**: Contains services and options for API interaction.
- **RaftLabs.Assignment.Tests**: Unit tests for the application.

## Key Classes

- **UserService**: Handles API interactions and caching.
- **ExternalApiOptions**: Configuration options for the external API.

## Error Handling

- Network errors are handled with retry policies.
- Invalid user or page inputs are validated and reported to the user.

## License

This project is licensed under the MIT License. See the `LICENSE` file for details.

---

Enjoy using the RaftLabs User Fetcher! 🚀



   
