using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using VM.BettingApplication.Core.DTO;
using VM.BettingApplication.Core.Entities;
using VM.BettingApplication.Core.Helpers;
using VM.BettingApplication.Core.Repository;
using VM.BettingApplication.Core.Services.Interface;
using VM.BettingApplication.Core.Settings;

namespace VM.BettingApplication.Core.Services.Implementation
{
    public class WalletService : IWalletService
    {
        private readonly ApplicationSettings _appSettings;
        private AsyncLock _initLock = new AsyncLock();

        public WalletService(IOptions<ApplicationSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public async Task<AddTransactionResponse> AddTransaction(AddTransactionRequest request)
        {
            using (await _initLock.LockAsync())
            {
                if (request.Amount <= 0)
                    return new AddTransactionResponse { Success = false, Message = ValidationMessages.InvalidAmount };
                var lastTransaction = await GetLastTransaction();
                if (request.Type == WalletTransactionType.Debit)
                {
                    if (request.Amount > lastTransaction.CurrentState)
                    {
                        var existingTransaction = await GetTransactionById(request.TransactionId.Value);
                        if (existingTransaction != null)
                            return new AddTransactionResponse { Success = true, Transaction = existingTransaction };
                        return new AddTransactionResponse { Success = false, Message = ValidationMessages.NotEnoughMoney };
                        
                    }
                }

                var nextState = (lastTransaction?.CurrentState ?? 0)
                    + (request.Amount.Value * (int)request.Type.Value);

                var newTransaction = new WalletTransaction
                {
                    Amount = request.Amount.Value,
                    CurrentState = nextState,
                    Id = request.TransactionId.Value,
                    TransactionDateTime = DateTime.Now,
                    TransactionType = request.Type.Value
                };

                try
                {
                    await SaveTransaction(newTransaction);
                }
                catch (Exception e)
                {
                    if (e.IsPostgresUniqueConstraintException())
                    {
                        var existingTransaction = await GetTransactionById(request.TransactionId.Value);
                        if (existingTransaction != null)
                            return new AddTransactionResponse { Success = true, Transaction = existingTransaction };
                    }

                    throw;
                }

                return new AddTransactionResponse { Success = true, Transaction = newTransaction };
            }
        }

        public async Task<WalletTransaction> GetCurrentState()
        {
            return await GetLastTransaction();
        }

        private async Task<WalletTransaction> GetLastTransaction()
        {
            using (
                DatabaseContext databaseContext =
                DatabaseContext.GenerateContext(_appSettings.DatabaseConnectionString)
            )
            {
                return await databaseContext.WalletTransactions
                    .OrderByDescending(x => x.TransactionDateTime)
                    .FirstOrDefaultAsync();

            }
        }

        private async Task<WalletTransaction> GetTransactionById(Guid transactionId)
        {
            using (
                DatabaseContext databaseContext =
                DatabaseContext.GenerateContext(_appSettings.DatabaseConnectionString)
            )
            {
                return await databaseContext.WalletTransactions
                    .Where(x => x.Id == transactionId)
                    .SingleOrDefaultAsync();

            }
        }

        private async Task SaveTransaction(WalletTransaction transaction)
        {
            using (
                DatabaseContext databaseContext =
                DatabaseContext.GenerateContext(_appSettings.DatabaseConnectionString)
            )
            {
                databaseContext.WalletTransactions.Add(transaction);
                await databaseContext.SaveChangesAsync();
            }
        }
    }
}

