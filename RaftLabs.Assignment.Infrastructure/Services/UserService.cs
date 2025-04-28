using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using RaftLabs.Assignment.Core.Models;
using RaftLabs.Assignment.Infrastructure.Options;
using System.Text.Json;

namespace RaftLabs.Assignment.Infrastructure.Services
{
    public class UserService
    {
        private readonly HttpClient _httpClient;
        private readonly ExternalApiOptions _options;
        private readonly IMemoryCache _cache;
        private readonly AsyncRetryPolicy _retryPolicy;

        public UserService(HttpClient httpClient, IOptions<ExternalApiOptions> options, IMemoryCache cache)
        {
            _httpClient = httpClient;
            _options = options.Value;
            _cache = cache;

            _retryPolicy = Policy
                .Handle<HttpRequestException>()
                .Or<TaskCanceledException>()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
            _httpClient.DefaultRequestHeaders.Add("x-api-key", _options.ApiKey);
        }

        public async Task<UserDto> GetUserByIdAsync(int userId)
        {
            string cacheKey = $"User_{userId}";

            if (_cache.TryGetValue(cacheKey, out UserDto cachedUser))
            {
                return cachedUser;
            }

            return await _retryPolicy.ExecuteAsync(async () =>
            {
                var response = await _httpClient.GetAsync($"{_options.BaseUrl}/users/{userId}");

                var jsonString = await response.Content.ReadAsStringAsync();
               
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Failed to fetch user. StatusCode: {response.StatusCode}");
                }

                var content = await response.Content.ReadAsStringAsync();
                var userResponse = JsonConvert.DeserializeObject<ApiSingleUserResponse>(content);

                if (userResponse?.Data == null)
                {
                    throw new Exception("No user data returned from API.");
                }

                _cache.Set(cacheKey, userResponse.Data, TimeSpan.FromMinutes(10));

                return userResponse.Data;
            });
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync(int pageNo)
        {
            string cacheKey = "AllUsers";

            if (_cache.TryGetValue(cacheKey, out IEnumerable<UserDto> cachedUsers))
            {
                return cachedUsers;
            }

            var allUsers = new List<UserDto>();

            while (true)
            {
                var response = await _retryPolicy.ExecuteAsync(async () =>
                    await _httpClient.GetAsync($"{_options.BaseUrl}/users?page={pageNo}")
                );

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Failed to fetch users. StatusCode: {response.StatusCode}");
                }

                var content = await response.Content.ReadAsStringAsync();
                //var pageData = System.Text.Json.JsonSerializer.Deserialize<PagedUserResponseDto>(content);
                var pageData = JsonConvert.DeserializeObject<PagedUserResponseDto>(content);

                if (pageData?.Data == null || !pageData.Data.Any())
                    break;

                allUsers.AddRange(pageData.Data);

                if (pageNo >= pageData.TotalPages)
                    break;

                pageNo++;
            }

            _cache.Set(cacheKey, allUsers, TimeSpan.FromMinutes(10));

            return allUsers;
        }

    }
}
