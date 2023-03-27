using Microsoft.AspNetCore.Mvc;
using VM.BettingApplication.Core.DTO;
using VM.BettingApplication.Core.Entities;
using VM.BettingApplication.Core.Helpers;
using VM.BettingApplication.Core.Services.Implementation;
using VM.BettingApplication.Core.Services.Interface;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VM.BettingApplication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;

        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }


        [HttpGet("GetState")]
        public async Task<WalletTransaction> GetWalletState()
        {
            return await _walletService.GetCurrentState();
        }


        [HttpPost("AddTransaction")]
        [ProducesResponseType(typeof(AddTransactionResponse), 201)]
        [ProducesResponseType(typeof(AddTransactionResponse), 400)]
        public async Task<IActionResult> AddTransaction([FromBody] AddTransactionRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AddTransactionResponse { Message = ValidationMessages.InvalidRequest });
            }
            var result = await _walletService.AddTransaction(request);
            return StatusCode(result.Success ? 201 : 400, result);
        }

    }
}
