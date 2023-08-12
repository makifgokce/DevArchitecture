using Core.Entities;
using System;

namespace Entities.Dtos
{
    public class UpdateUserDto : IDto
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string MobilePhones { get; set; }
        public string Address { get; set; }
        public string Notes { get; set; }
        public DateTime BirthDate { get; set; }
    }
}