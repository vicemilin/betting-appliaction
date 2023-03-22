using Microsoft.AspNetCore.Mvc;
using VM.BettingApplication.Core.Repository;
using VM.BettingApplication.Core.Entities;
using Microsoft.EntityFrameworkCore;
using VM.BettingApplication.Core.DTO;
using VM.BettingApplication.Core.Services.Interface;
using VM.BettingApplication.Core.Helpers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VM.BettingApplication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        // GET: api/<TicketController>
        [HttpGet]
        public async Task<IEnumerable<Ticket>> Get()
        {
            using(DatabaseContext databaseContext = DatabaseContext.GenerateContext("Host=localhost;Port=5432;Password=postgres;Username=postgres;Database=postgres"))
            {
                var tickets = await databaseContext.Tickets.Include(x => x.TicketBets).ToListAsync();
                return tickets;
            }
        }

        // GET api/<TicketController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
        
        // POST api/<TicketController>
        [HttpPost("Payin")]
        public async Task<IActionResult> PayinTicket([FromBody] PayinTicketRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new PayinTicketResponse { Message = ValidationMessages.InvalidRequest });
            }
            var result = await _ticketService.Payin(request);
            return StatusCode(result.Success ? 201 : 400, result);
        }

        // PUT api/<TicketController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TicketController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
