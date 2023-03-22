using System;
using VM.BettingApplication.Core.Entities;

namespace VM.BettingApplication.Core.Services.Interface
{
	public interface ISportService
	{
        Task<Sport[]> GetAllSports();
	}
}

