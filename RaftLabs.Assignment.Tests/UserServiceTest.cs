using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using RaftLabs.Assignment.Infrastructure.Options;
using RaftLabs.Assignment.Infrastructure.Services;
using System.Net;

namespace RaftLabs.Assignment.Tests
{
    public class UserServiceTest
    {
        [Fact]
        public async Task GetUserByIdAsync_ShouldThrowException_WhenUserNotFound()
        {
            // Arrange
            var notFoundResponse = new HttpResponseMessage(HttpStatusCode.NotFound);
            var mockHandler = new MockHttpMessageHandler(notFoundResponse);
            var httpClient = new HttpClient(mockHandler);

            var options = Options.Create(new ExternalApiOptions { BaseUrl = "https://reqres.in/api", ApiKey = "reqres-free-v1" });
            var cache = new MemoryCache(new MemoryCacheOptions());

            var service = new UserService(httpClient, options, cache);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await service.GetUserByIdAsync(999));
        }
    }
}
