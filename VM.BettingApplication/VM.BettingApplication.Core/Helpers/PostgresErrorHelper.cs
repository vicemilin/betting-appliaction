using System;
using Npgsql;

namespace VM.BettingApplication.Core.Helpers
{
	public static class PostgresErrorHelper
	{
        public static bool IsPostgresUniqueConstraintException(this Exception exception)
        {
            try
            {
                return exception.InnerException != null
                            && exception.InnerException is PostgresException pSqlEx
                            && pSqlEx.SqlState != null
                            && pSqlEx.SqlState.Equals("23505");
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}

