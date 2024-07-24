using Core.DataAccess.EntityFramework;
using Core.Entities.Concrete;
using Core.Entities.Dtos;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using Microsoft.EntityFrameworkCore;
using ServiceStack;
using ServiceStack.Text;
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
            var post = await (from p in Context.Posts
                              join u in Context.Users on p.AuthorId equals u.UserId
                              where p.Id == id && p.Slug == slug
                              select new PostDto
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

        public async Task<List<PostDto>> GetPosts()
        {
            var post = await Context.Posts.Join(Context.Users, p => p.AuthorId, u => u.UserId, (p, u) => new { Posts = p, Users = u }).Where(p => p.Posts.PublishDate <= DateTime.Now && p.Posts.DeletedDate == null).Select(x => new PostDto
            {
                Id = x.Posts.Id,
                Title = x.Posts.Title,
                Body = x.Posts.Body,
                Description = x.Posts.Description,
                Slug = x.Posts.Slug,
                Keywords = x.Posts.Keywords,
                CreatedDate = x.Posts.CreatedDate,
                UpdatedDate = x.Posts.UpdatedDate,
                AuthorId = x.Posts.AuthorId,
                AuthorName = x.Users.Account,
                PublishDate = x.Posts.PublishDate
            }).OrderByDescending(x => x.PublishDate).ToListAsync();
            return post;
        }
    }
}