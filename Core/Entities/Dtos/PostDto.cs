using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.Entities.Dtos
{
    public class PostDto : IEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
        public string Slug { get; set; }
        public int AuthorId { get; set; }
        [NotMapped]
        public string? AuthorName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        [JsonIgnore]
        public DateTime? DeletedDate { get; set; } = null;
    }
}
