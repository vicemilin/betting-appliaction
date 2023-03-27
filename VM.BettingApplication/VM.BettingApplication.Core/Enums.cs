﻿using System;
namespace VM.BettingApplication.Core
{

    public enum TicketStatus
    {
        Lost = -1,
        Refund = 0,
        Won = 1,
        InProgress = 2
    }

    public enum WalletTransactionType
    {
        Debit = -1,
        Credit = 1
    }
}

