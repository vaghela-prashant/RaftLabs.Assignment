using Newtonsoft.Json;

namespace RaftLabs.Assignment.Core.Models
{
    public class PagedUserResponseDto
    {
        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("per_page")]
        public int PerPage { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("total_pages")]
        public int TotalPages { get; set; }

        [JsonProperty("data")]
        public List<UserDto> Data { get; set; }

        [JsonProperty("support")]
        public SupportDto Support { get; set; }

        public PagedUserResponseDto()
        {
            this.Data = new List<UserDto>();
            this.Support = new SupportDto();
        }
    }
}
