using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace VM.BettingApplication.Core.Entities
{
	[Table("wallet_transaction")]
	public class WalletTransaction
	{
		public WalletTransaction()
		{
		}

		[Column("id")]
		public Guid Id { get; set; }

		[Column("type")]
		public WalletTransactionType TransactionType { get; set; }

		[Column("amount")]
		public decimal Amount { get; set; }

		[Column("transaction_date_time")]
		public DateTime TransactionDateTime { get; set; }

		[Column("current_state")]
		public decimal CurrentState { get; set; }
	}
}

