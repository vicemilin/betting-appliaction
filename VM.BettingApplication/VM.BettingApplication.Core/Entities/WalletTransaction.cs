using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace VM.BettingApplication.Core.Entities
{
	public class WalletTransaction
	{
		public WalletTransaction()
		{
		}

		[Column("id")]
		public Guid Id { get; set; }
	}
}

