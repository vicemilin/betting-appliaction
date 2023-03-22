using System;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using VM.BettingApplication.Core.DTO;
using VM.BettingApplication.Core.Entities;
using VM.BettingApplication.Core.Repository;
using VM.BettingApplication.Core.Services.Interface;
using VM.BettingApplication.Core.Settings;
using VM.BettingApplication.Core.Helpers;

namespace VM.BettingApplication.Core.Services.Implementation
{
	public class TicketService : ITicketService
	{
        private readonly ApplicationSettings _appSettings;
        private readonly IOfferService _offerService;

		public TicketService(
            IOptions<ApplicationSettings> appSettings,
            IOfferService offerService
        )
		{
            _appSettings = appSettings.Value;
            _offerService = offerService;
		}

        public async Task<PayinTicketResponse> Payin(PayinTicketRequest request)
        {
            if(request.PayinAmount < _appSettings.MinPayinAmount)
            {
                return new PayinTicketResponse
                {
                    Success = false,
                    Message = ValidationMessages.InvalidPayinAmount
                };
            }

            var offerValidationResult = await _offerService.ValidateOffer(request.TicketBets);
            if (!offerValidationResult.IsValid)
                return new PayinTicketResponse
                {
                    Success = false,
                    Message = ValidationMessages.OfferValidationFailed,
                    ValidationResult = offerValidationResult
                };

            var ticket = new Ticket
            {
                Id = request.TransactionId.Value,
                Payin = request.PayinAmount.Value,
                PayinTime = DateTime.UtcNow,
                Status = TicketStatus.InProgress
            };

            var ticketBets = request.TicketBets.Select(x =>
            {
                return new TicketBet {
                    Id = Guid.NewGuid(),
                    Status = TicketStatus.InProgress,
                    Odds = x.Odds.Value,
                    TicketId = request.TransactionId.Value
                };
            }).ToArray();

            using (DatabaseContext databaseContext =
                DatabaseContext.GenerateContext(_appSettings.DatabaseConnectionString))
            {
                try
                {
                    databaseContext.Add(ticket);
                    databaseContext.AddRange(ticketBets);
                    await databaseContext.SaveChangesAsync();

                    ticket.TicketBets = ticketBets;
                    return new PayinTicketResponse { Success = true }; 
                }
                catch(Exception e)
                {
                    if (e.IsPostgresUniqueConstraintException())
                        return new PayinTicketResponse { Success = true };
                    
                    throw;
                }
            }

        }
    }
}

