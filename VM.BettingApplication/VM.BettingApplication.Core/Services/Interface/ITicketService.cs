using System;
using VM.BettingApplication.Core.DTO;
using VM.BettingApplication.Core.Entities;

namespace VM.BettingApplication.Core.Services.Interface
{
	public interface ITicketService
	{
		Task<PayinTicketResponse> Payin(PayinTicketRequest request);
	}
}

