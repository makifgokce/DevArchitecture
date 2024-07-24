using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Concrete
{
    public class Post : IEntity
    {
        public Post()
        {
            if (Id == 0)
            {
                CreatedDate = DateTime.Now;
                PublishDate = DateTime.Now;
            }
            UpdatedDate = DateTime.Now;
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
        public int AuthorId { get; set; }
        public string Slug { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; } = null;
        public DateTime PublishDate { get; set; }

    }
}