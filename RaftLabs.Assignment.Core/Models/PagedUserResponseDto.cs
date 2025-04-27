namespace RaftLabs.Assignment.Core.Models
{
    public class PagedUserResponseDto
    {
        public int Page { get; set; }
        public int PerPage { get; set; }
        public int Total { get; set; }
        public int TotalPages { get; set; }
        public List<UserDto> Data { get; set; }

        public PagedUserResponseDto()
        {
            this.Data = new List<UserDto>();
        }
    }
}
