using System.Text.Json.Serialization;

namespace Core.Entities.Dtos
{
    public class UserDto : IEntity
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Account { get; set; }
        public string Email { get; set; }
        public string MobilePhones { get; set; }
        public string Address { get; set; }
        public string Notes { get; set; }
        public int Gender { get; set; }
        public string Password { get; set; }
        public int Status { get; set; }
        public bool Verified { get; set; }
        [JsonIgnore]
        public string RefreshToken { get; set; }
    }
}