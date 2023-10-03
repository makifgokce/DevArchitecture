using Core.DataAccess.EntityFramework;
using Core.Entities.Concrete;
using Core.Entities.Dtos;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using Microsoft.EntityFrameworkCore;
using ServiceStack;
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
            var post = await (from p in Context.Posts join u in Context.Users on p.AuthorId equals u.UserId where p.Id == id && p.Slug == slug select new PostDto
            {
                Id = p.Id,
                Title = p.Title,
                Body = p.Body,
                Description = p.Description,
                Slug = p.Slug,
                AuthorId = p.AuthorId,
                Keywords = p.Keywords,
                CreatedDate = p.CreatedDate,
                UpdatedDate = p.UpdatedDate,
                DeletedDate = (DateTime)p.DeletedDate,
                AuthorName = u.Account
            }).FirstOrDefaultAsync();
            return post;
        }
    }
}
