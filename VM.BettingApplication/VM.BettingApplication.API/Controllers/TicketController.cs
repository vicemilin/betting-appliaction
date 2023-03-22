using Microsoft.AspNetCore.Mvc;
using VM.BettingApplication.Core.Repository;
using VM.BettingApplication.Core.Entities;
using Microsoft.EntityFrameworkCore;
using VM.BettingApplication.Core.DTO;
using VM.BettingApplication.Core.Services.Interface;
using VM.BettingApplication.Core.Helpers;

namespace VM.BettingApplication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private const int DEFAULT_TICKET_PAGE_SIZE = 10;

        private readonly ITicketService _ticketService;

        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpGet("GetTickets")]
        public async Task<IEnumerable<Ticket>> GetTickets([FromQuery] int? pageSize, [FromQuery] int? pageNumber)
        {
            return await _ticketService.GetTickets(
                pageSize.HasValue ? pageSize.Value : DEFAULT_TICKET_PAGE_SIZE,
                pageNumber.GetValueOrDefault()
            );
        }

        
        [HttpPost("Payin")]
        [ProducesResponseType(typeof(PayinTicketResponse), 201)]
        [ProducesResponseType(typeof(PayinTicketResponse), 400)]
        public async Task<IActionResult> PayinTicket([FromBody] PayinTicketRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new PayinTicketResponse { Message = ValidationMessages.InvalidRequest });
            }
            var result = await _ticketService.Payin(request);
            return StatusCode(result.Success ? 201 : 400, result);
        }
    }
}
