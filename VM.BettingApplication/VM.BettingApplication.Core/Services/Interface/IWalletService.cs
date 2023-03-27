using System;
using VM.BettingApplication.Core.DTO;
using VM.BettingApplication.Core.Entities;

namespace VM.BettingApplication.Core.Services.Interface
{
	public interface IWalletService
	{
		Task<WalletTransaction> GetCurrentState();

		Task<AddTransactionResponse> AddTransaction(AddTransactionRequest request);
	}
}

