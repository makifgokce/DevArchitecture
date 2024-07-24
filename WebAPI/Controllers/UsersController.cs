using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Handlers.Users.Commands;
using Business.Handlers.Users.Queries;
using Core.Entities.Dtos;
using Core.Utilities.Results;
using Entities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    /// <summary>
    /// If controller methods will not be Authorize, [AllowAnonymous] is used.
    /// </summary>
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class UsersController : BaseApiController
    {
        /// <summary>
        /// List Users
        /// </summary>
        /// <remarks>bla bla bla Users</remarks>
        /// <return>Users List</return>
        /// <response code="200"></response>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedResult<IEnumerable<UserDto>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet()]
        public async Task<IActionResult> GetList([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return GetResponseOnlyResultData(await Mediator.Send(new GetUsersQuery() { PageSize = pageSize, PageNumber = pageNumber }));
        }

        /// <summary>
        /// List Users
        /// </summary>
        /// <remarks>bla bla bla Users</remarks>
        /// <return>Users List</return>
        /// <response code="200"></response>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UserDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet("Data")]
        public async Task<IActionResult> GetUser()
        {
            return GetResponseOnlyResultData(await Mediator.Send(new GetUserDataQuery()));
        }
        /// <summary>
        /// User Lookup
        /// </summary>
        /// <remarks>bla bla bla Users</remarks>
        /// <return>Users List</return>
        /// <response code="200"></response>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<SelectionItem>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet("lookups")]
        public async Task<IActionResult> GetUserLookup()
        {
            return GetResponseOnlyResultData(await Mediator.Send(new GetUserLookupQuery()));
        }

        /// <summary>
        /// It brings the details according to its id.
        /// </summary>
        /// <remarks>bla bla bla </remarks>
        /// <return>Users List</return>
        /// <response code="200"></response>
        [AllowAnonymous]
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet("{account}")]
        public async Task<IActionResult> GetByAccount([FromRoute] string account)
        {
            return GetResponseOnlyResultData(await Mediator.Send(new GetUserQuery { Account = account }));
        }

        /// <summary>
        /// Add User.
        /// </summary>
        /// <param name="createUser"></param>
        /// <returns></returns>
        [Consumes("application/json")]
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateUserCommand createUser)
        {
            return GetResponseOnlyResultMessage(await Mediator.Send(createUser));
        }

        /// <summary>
        /// Update User.
        /// </summary>
        /// <param name="updateUserDto"></param>
        /// <returns></returns>
        [Consumes("application/json")]
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPut("{account}")]
        public async Task<IActionResult> Update([FromRoute] string account, [FromBody] UpdateUserDto updateUserDto)
        {
            return GetResponseOnlyResultMessage(await Mediator.Send(new UpdateUserCommand { Account = account, Email = updateUserDto.Email, Name = updateUserDto.Name, Surname = updateUserDto.Surname, MobilePhones = updateUserDto.MobilePhones, Address = updateUserDto.Address, Notes = updateUserDto.Notes, BirtDate = updateUserDto.BirthDate }));
        }

        /// <summary>
        /// Update User.
        /// </summary>
        /// <param name="updateUserDto"></param>
        /// <returns></returns>
        [Consumes("application/json")]
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpPut("cid")]
        public async Task<IActionResult> UpdateCid([FromBody] UpdateCidDto updateUserDto)
        {
            return GetResponseOnlyResultMessage(await Mediator.Send(new UpdateUserCidCommand { CitizenId = updateUserDto.CitizenId, Name = updateUserDto.Name, Surname = updateUserDto.Surname, BirthDate = updateUserDto.BirthDate }));
        }
        /// <summary>
        /// Delete User.
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [Consumes("application/json")]
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpDelete("{account}")]
        public async Task<IActionResult> Delete([FromRoute] string account)
        {
            return GetResponseOnlyResultMessage(await Mediator.Send(new DeleteUserCommand { Account = account }));
        }
    }
}