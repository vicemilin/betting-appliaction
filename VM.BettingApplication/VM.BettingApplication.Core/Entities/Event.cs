using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace VM.BettingApplication.Core.Entities
{
	[Table("event")]
	public class Event
	{
        [Column("id")]
        public Guid Id { get; set; }

		[Column("home_team")]
		public string HomeTeam { get; set; }

		[Column("away_team")]
		public string AwayTeam { get; set; }

		[Column("start_time")]
		public DateTime StartTime { get; set; }

		[Column("sport_id")]
		public Guid SportId { get; set; }

		[Column("is_top_offer")]
		public bool IsTopOffer { get; set; }

		[Column("linked_id")]
		public Guid? LinkedId { get; set; }

        public virtual ICollection<Pick> Picks { get; set; }
		public virtual Sport Sport { get; set; }
    }
}

