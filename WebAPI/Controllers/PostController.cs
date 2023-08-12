using Business.Handlers.Posts.Commands;
using Business.Handlers.Posts.Queries;
using Business.Handlers.Users.Queries;
using Core.Entities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Controllers;
/// <summary>
/// If controller methods will not be Authorize, [AllowAnonymous] is used.
/// </summary>
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class PostController : BaseApiController
{
    /// <summary>
    /// Add Post.
    /// </summary>
    /// <param name="createPost"></param>
    /// <returns></returns>
    [Consumes("application/json")]
    [Produces("application/json", "text/plain")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreatePostCommand createPost)
    {
        return GetResponseOnlyResultMessage(await Mediator.Send(createPost));
    }
    /// <summary>
    /// Update Post.
    /// </summary>
    /// <param name="updatePost"></param>
    /// <returns></returns>
    [Consumes("application/json")]
    [Produces("application/json", "text/plain")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdatePostCommand updatePost)
    {
        return GetResponseOnlyResultMessage(await Mediator.Send(updatePost));
    }
    /// <summary>
    /// Delete Post.
    /// </summary>
    /// <param name="deletePost"></param>
    /// <returns></returns>
    [Consumes("application/json")]
    [Produces("application/json", "text/plain")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] DeletePostCommand deletePost)
    {
        return GetResponseOnlyResultMessage(await Mediator.Send(deletePost));
    }

    /// <summary>
    /// List Posts
    /// </summary>
    /// <remarks>bla bla bla Posts</remarks>
    /// <return>Posts List</return>
    /// <response code="200"></response>
    [AllowAnonymous]
    [Produces("application/json", "text/plain")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<PostDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [HttpGet]
    public async Task<IActionResult> GetList()
    {
        return GetResponseOnlyResultData(await Mediator.Send(new GetPostsQuery()));
    }
    /// <summary>
    /// List Posts
    /// </summary>
    /// <remarks>bla bla bla Posts</remarks>
    /// <return>Posts List</return>
    /// <response code="200"></response>
    [AllowAnonymous]
    [Produces("application/json", "text/plain")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<PostDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [HttpGet("{id}/{slug}")]
    public async Task<IActionResult> GetPost([FromRoute] int id, [FromRoute] string slug)
    {
        return GetResponseOnlyResultData(await Mediator.Send(new GetPostQuery { Id = id, Slug = slug}));
    }
}