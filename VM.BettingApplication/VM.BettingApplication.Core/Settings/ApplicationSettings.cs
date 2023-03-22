using System;
namespace VM.BettingApplication.Core.Settings
{
	public class ApplicationSettings
	{
		public string DatabaseConnectionString { get; set; }
        public int NumberOfEventsPerSportPerDay { get; set; }
        public decimal TopOfferOddsBoostPercentage { get; set; }
        public decimal MinPayinAmount { get; set; }
        public int MaxEventsFromTopOffer { get; set; }
        public decimal MinimumOddsWithTopOffer { get; set; }
        public int MinimumEventsWithTopOffer { get; set; }

        public string[] SoccerTeams = new string[]
        {
            "Dinamo",
            "Hajduk",
            "Osijek",
            "Rijeka"
        };

        public string[] BasketballTeams = new string[]
        {
            "Split",
            "Zadar",
            "Cibona",
            "Cedevita"
        };

        public string[] TennisPlayers = new string[]
        {
            "Borna",
            "Marin",
            "Ivo",
            "Goran"
        };
    }
}

