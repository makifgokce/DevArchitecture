using Core.DataAccess.EntityFramework;
using Core.Entities.Concrete;
using Core.Entities.Dtos;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class PostRepository : EfEntityRepositoryBase<Post, ProjectDbContext>, IPostRepository
    {
        public PostRepository(ProjectDbContext context)
            : base(context)
        {
        }

        public async Task<PostDto> GetPost(int id, string slug)
        {
            var post = await Context.Posts.Where(q => q.Id == id && q.Slug == slug).Select(x => new PostDto
            {
                Id = x.Id,
                Title = x.Title,
                Body = x.Body,
                Description = x.Description,
                Slug = x.Slug,
                AuthorId = x.AuthorId,
                Keywords = x.Keywords,
                CreatedDate = x.CreatedDate,
                UpdatedDate = x.UpdatedDate,
                DeletedDate = (DateTime)x.DeletedDate,
                AuthorName = Context.Users.Where(z => z.UserId == x.AuthorId).FirstOrDefault().Account,
            }).FirstOrDefaultAsync();
            return post;
        }
    }
}
