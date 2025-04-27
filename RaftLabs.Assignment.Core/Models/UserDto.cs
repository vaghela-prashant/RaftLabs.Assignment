﻿using Newtonsoft.Json;

namespace RaftLabs.Assignment.Core.Models
{
    public class UserDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; } = string.Empty;

        [JsonProperty("first_name")]
        public string FirstName { get; set; } = string.Empty;

        [JsonProperty("last_name")]
        public string LastName { get; set; } = string.Empty;

        [JsonProperty("avatar")]
        public string Avatar { get; set; } = string.Empty;
    }
}
