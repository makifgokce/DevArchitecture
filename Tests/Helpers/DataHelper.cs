using Core.Entities.Concrete;
using Core.Utilities.Security.Hashing;
using System;
using System.Collections.Generic;
using static Core.Entities.Concrete.User;

namespace Tests.Helpers
{
    public static class DataHelper
    {
        public static User GetUser(string name)
        {
            HashingHelper.CreatePasswordHash("123456", out var passwordSalt, out var passwordHash);

            return new User()
            {
                UserId = 1,
                Address = "test",
                BirthDate = new DateTime(1988, 01, 01),
                CitizenId = 12345678910,
                Account = "testacc",
                Email = "test@test.com",
                Name = name,
                Surname = name,
                Gender = 1,
                MobilePhones = "05339262726",
                Notes = "test",
                RecordDate = DateTime.Now,
                PasswordHash = passwordSalt,
                PasswordSalt = passwordHash,
                Status = UserStatus.Activated,
                AuthenticationProviderType = "Person",
                UpdateContactDate = DateTime.Now,
                Verified = true
            };
        }

        public static List<User> GetUserList()
        {
            HashingHelper.CreatePasswordHash("123456", out var passwordSalt, out var passwordHash);
            var list = new List<User>();

            for (var i = 1; i <= 5; i++)
            {
                var user = new User()
                {
                    UserId = i,
                    Address = "test" + i,
                    BirthDate = new DateTime(1988, 01, 01),
                    CitizenId = 123456789101,
                    Account = "testacc" + i,
                    Email = "test@test.com",
                    Name = "test" + i,
                    Surname = "test" + i,
                    Gender = 1,
                    MobilePhones = "123456789",
                    Notes = "test",
                    RecordDate = DateTime.Now,
                    PasswordHash = passwordSalt,
                    PasswordSalt = passwordHash,
                    Status = UserStatus.Activated,
                    AuthenticationProviderType = "User",
                    UpdateContactDate = DateTime.Now,
                    Verified = true
                };
                list.Add(user);
            }

            return list;
        }
    }
}