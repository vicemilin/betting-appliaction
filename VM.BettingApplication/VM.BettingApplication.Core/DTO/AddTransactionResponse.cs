using System;
using VM.BettingApplication.Core.Entities;

namespace VM.BettingApplication.Core.DTO
{
	public class AddTransactionResponse
	{
		public bool Success { get; set; }
		public string Message { get; set; }
		public WalletTransaction Transaction {get; set;}
	}
}

