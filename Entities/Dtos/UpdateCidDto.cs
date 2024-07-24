using Core.Entities;
using System;

namespace Entities.Dtos
{
    public class UpdateCidDto : IDto
    {
        public long CitizenId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
    }
}