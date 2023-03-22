using System;
namespace VM.BettingApplication.Core.DTO
{
	public class PayinTicketResponse
	{
		public bool Success { get; set; }
		public string Message { get; set; }

		public OfferValidationResult ValidationResult { get; set; }
	}

    public class OfferValidationResult
    {
        public bool IsValid { get; set; }
        public string Message { get; set; }
        public OfferPickValidationResult[] PickResults { get; set; }
    }

    public class OfferPickValidationResult
    {
        public bool IsValid { get; set; }
        public string Message { get; set; }
        public Guid PickId { get; set; }
        public decimal Odds { get; set; }
    }
}

