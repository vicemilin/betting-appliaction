using System;
using System.ComponentModel.DataAnnotations;

namespace VM.BettingApplication.Core.DTO
{
	public class AddTransactionRequest
	{
		[Required]
		public Guid? TransactionId { get; set; }

		[Required]
		public decimal? Amount { get; set; }

		[Required]
		public WalletTransactionType? Type { get; set; } 
	}
}

