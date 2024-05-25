using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Concrete
{
    public class User : IEntity
    {
        public User()
        {
            if (UserId == 0)
            {
                RecordDate = DateTime.Now;
            }
            UpdateContactDate = DateTime.Now;
            Status = UserStatus.NotActivated;
        }

        public int UserId { get; set; }
        public long CitizenId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Account { get; set; }
        public string Email { get; set; }
        [JsonIgnore]
        public string RefreshToken { get; set; }
        public string MobilePhones { get; set; }
        public UserStatus Status { get; set; }
        public DateTime BirthDate { get; set; }
        public int Gender { get; set; }
        public DateTime RecordDate { get; set; }
        public string Address { get; set; }
        public string Notes { get; set; }
        public DateTime UpdateContactDate { get; set; }

        /// <summary>
        /// This is required when encoding token. Not in db. The default is Person.
        /// </summary>
        [NotMapped]
        public string AuthenticationProviderType { get; set; } = "Person";

        public byte[] PasswordSalt { get; set; }
        public byte[] PasswordHash { get; set; }
        public bool Verified { get; set; }

        public bool UpdateMobilePhone(string mobilePhone)
        {
            if (mobilePhone == MobilePhones)
            {
                return false;
            }

            MobilePhones = mobilePhone;
            return true;
        }
        public enum UserStatus
        {
            NotActivated,
            Activated,
            Deleted,
            Banned
        }
    }
}
