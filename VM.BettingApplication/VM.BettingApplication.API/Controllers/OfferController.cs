using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VM.BettingApplication.Core.Entities;
using VM.BettingApplication.Core.Services.Interface;

namespace VM.BettingApplication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfferController : ControllerBase
    {
        private readonly IOfferService _offerService;

        public OfferController(
            IOfferService offerService
            )
        {
            _offerService = offerService;
        }

        [HttpGet]
        public async Task<IEnumerable<Event>> Get()
        {
            return await _offerService.GetFullOffer();
        }

        [HttpPost("AddToTopOffer/{eventId}")]
        public async Task<Event> AddToTopOffer([FromRoute] Guid eventId)
        {
            return await _offerService.AddToTopOffer(eventId);
        }
    }
}
