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

var user = await userService.GetUserByIdAsync(2);
Console.WriteLine($"\n{user.FirstName} {user.LastName}");

var allUsers = await userService.GetAllUsersAsync();
foreach (var user in allUsers)
{
    Console.WriteLine($"UserId: {user.Id}, UserName: {user.FirstName} {user.LastName} Email: {user.Email}");
}
