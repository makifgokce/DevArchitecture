using Business.Handlers.Message.Queries;
using Core.Entities.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class MessagesController : BaseApiController
    {
        /// <summary>
        /// List Private Messages
        /// </summary>
        /// <remarks>bla bla bla Users</remarks>
        /// <return>Users List</return>
        /// <response code="200"></response>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<MessageDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet()]
        public async Task<IActionResult> GetMessages()
        {
            return GetResponseOnlyResultData(await Mediator.Send(new GetMessageListQuery()));
        }
        /// <summary>
        /// List Private Messages
        /// </summary>
        /// <remarks>bla bla bla Users</remarks>
        /// <return>Users List</return>
        /// <response code="200"></response>
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<MessageDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [HttpGet("{account}")]
        public async Task<IActionResult> GetMessages(string account, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return GetResponseOnlyResultData(await Mediator.Send(new GetMessageQuery() { Account = account, PageNumber = pageNumber, PageSize = pageSize}));
        }
    }
}