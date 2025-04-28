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

try
{
    Console.Write("Enter a User ID to fetch: ");
    var userInput = Console.ReadLine();

    if (int.TryParse(userInput, out int userId))
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
    else
    {
        Console.WriteLine("Invalid User ID entered.");
    }

    Console.WriteLine("\nFetching users from a specific page...");

    Console.Write("Enter a Page Number to fetch users: ");
    var pageInput = Console.ReadLine();

    if (int.TryParse(pageInput, out int pageNumber) && pageNumber > 0)
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
    else
    {
        Console.WriteLine("Invalid Page Number entered.");
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

Console.WriteLine("\nProgram finished. Press any key to exit.");
Console.ReadKey();
