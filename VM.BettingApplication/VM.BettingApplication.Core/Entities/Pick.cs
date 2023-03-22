using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace VM.BettingApplication.Core.Entities
{
	[Table("pick")]
	public class Pick
	{
		public Pick()
		{
		}

		[Column("id")]
		public Guid Id { get; set; }

		[Column("pick_name")]
		public string PickName { get; set; }

		[Column("event_id")]
		public Guid EventId { get; set; }

		[Column("odds")]
		public decimal Odds { get; set; }

        [Column("status")]
        public TicketStatus Status { get; set; }


        [System.Text.Json.Serialization.JsonIgnore]
        public virtual Event Event { get; set; }
    }
}

