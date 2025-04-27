using Newtonsoft.Json;

namespace RaftLabs.Assignment.Core.Models
{
    public class SupportDto
    {
        [JsonProperty("url")]
        public string Url { get; set; } = string.Empty;

        [JsonProperty("text")]
        public string Text { get; set; } = string.Empty;
    }
}
