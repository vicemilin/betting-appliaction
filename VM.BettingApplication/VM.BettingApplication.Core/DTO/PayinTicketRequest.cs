using System;
using System.ComponentModel.DataAnnotations;

namespace VM.BettingApplication.Core.DTO
{
	public class PayinTicketRequest
	{
		[Required]
		public Guid? TransactionId { get; set; }
        [Required]
        public decimal? PayinAmount { get; set; }
        [Required, MinLength(1)]
        public PayinTicketBets[] TicketBets { get; set; }
	}

	public class PayinTicketBets
	{
		[Required]
		public Guid? PickId { get; set; }
        [Required]
        public decimal? Odds { get; set; }
	}
}

