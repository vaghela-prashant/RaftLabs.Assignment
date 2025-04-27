using Newtonsoft.Json;

namespace RaftLabs.Assignment.Core.Models
{
    public class ApiSingleUserResponse
    {
        [JsonProperty("data")]
        public UserDto Data { get; set; }

        [JsonProperty("support")]
        public SupportDto Support { get; set; }
    }
}
