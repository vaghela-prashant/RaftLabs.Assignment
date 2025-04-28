using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using RaftLabs.Assignment.Infrastructure.Options;
using RaftLabs.Assignment.Infrastructure.Services;

var host = Host.CreateDefaultBuilder()
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    })
    .ConfigureServices((context, services) =>
    {
        services.Configure<ExternalApiOptions>(context.Configuration.GetSection("ExternalApi"));
        services.AddMemoryCache();
        services.AddHttpClient<UserService>();
    })
    .Build();

var userService = host.Services.GetRequiredService<UserService>();

Console.WriteLine("Welcome to the RaftLabs User Fetcher 🚀\n");

bool exit = false;
while (!exit)
{
    Console.WriteLine("\nPlease select an option:");
    Console.WriteLine("1. Get User by ID");
    Console.WriteLine("2. Get All Users by Page");
    Console.WriteLine("3. Exit");
    Console.Write("Enter your choice (1-3): ");
    var choiceInput = Console.ReadLine();

    switch (choiceInput)
    {
        case "1":
            await GetUserByIdAsync(userService);
            break;
        case "2":
            await GetAllUsersAsync(userService);
            break;
        case "3":
            exit = true;
            Console.WriteLine("Exiting the application. Goodbye!");
            break;
        default:
            Console.WriteLine("Invalid choice. Please select 1, 2, or 3.");
            break;
    }
}

Console.WriteLine("\nProgram finished. Press any key to exit.");
Console.ReadKey();

// Helper Methods

static async Task GetUserByIdAsync(UserService userService)
{
    Console.Write("\nEnter a User ID to fetch: ");
    var userInput = Console.ReadLine();

    if (int.TryParse(userInput, out int userId))
    {
        try
        {
            var user = await userService.GetUserByIdAsync(userId);
            if (user != null)
            {
                Console.WriteLine($"\nUser Details:");
                Console.WriteLine($"ID: {user.Id}");
                Console.WriteLine($"Name: {user.FirstName} {user.LastName}");
                Console.WriteLine($"Email: {user.Email}");
            }
            else
            {
                Console.WriteLine("User not found.");
            }
        }
        catch (HttpRequestException httpEx)
        {
            Console.WriteLine($"Network error occurred: {httpEx.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
        }
    }
    else
    {
        Console.WriteLine("Invalid User ID entered.");
    }
}

static async Task GetAllUsersAsync(UserService userService)
{
    Console.Write("\nEnter a Page Number to fetch users: ");
    var pageInput = Console.ReadLine();

    if (int.TryParse(pageInput, out int pageNumber) && pageNumber > 0)
    {
        try
        {
            var allUsers = await userService.GetAllUsersAsync(pageNumber);

            if (allUsers != null && allUsers.Any())
            {
                Console.WriteLine($"\nUsers on Page {pageNumber}:");
                foreach (var singleUser in allUsers)
                {
                    Console.WriteLine($"UserId: {singleUser.Id}, Name: {singleUser.FirstName} {singleUser.LastName}, Email: {singleUser.Email}");
                }
            }
            else
            {
                Console.WriteLine($"No users found on page {pageNumber}.");
            }
        }
        catch (HttpRequestException httpEx)
        {
            Console.WriteLine($"Network error occurred: {httpEx.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
        }
    }
    else
    {
        Console.WriteLine("Invalid Page Number entered.");
    }
}
