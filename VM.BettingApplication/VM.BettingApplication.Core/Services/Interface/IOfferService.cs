using System;
using System.Threading.Tasks;
using VM.BettingApplication.Core.DTO;
using VM.BettingApplication.Core.Entities;


namespace VM.BettingApplication.Core.Services.Interface
{
	public interface IOfferService
	{
		Task<Event[]> GetFullOffer();

		Task AddNewEvents(IEnumerable<Event> events, IEnumerable<Pick> picks);

        Task UpdatePickStatus(Guid eventId, string pickId, TicketStatus status);

		Task<Dictionary<Sport, int>> GetNumberOfEventsForToday();

		Task<Event> AddToTopOffer(Guid eventId);

		Task<OfferValidationResult> ValidateOffer(PayinTicketBets[] payinTicketBets);
    }
}

