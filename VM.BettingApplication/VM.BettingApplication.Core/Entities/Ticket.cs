using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace VM.BettingApplication.Core.Entities
{
	[Table("ticket")]
	public class Ticket
	{
		[Column("id")]
		public Guid Id { get; set; }

		[Column("payin")]
		public decimal Payin { get; set; }

		[Column("win")]
		public decimal? Win { get; set; }

		[Column("status")]
		public TicketStatus Status { get; set; }

		[Column("payin_time")]
		public DateTime PayinTime { get; set; }


        public virtual ICollection<TicketBet> TicketBets { get; set; }
    }

	[Table("ticket_bet")]
	public class TicketBet
	{
		[Column("id")]
		public Guid Id { get; set; }

		[Column("ticket_id")]
		[ForeignKey("fk_ticket_bet_ticket")]
		public Guid TicketId { get; set; }

		[Column("odds")]
		public decimal Odds { get; set; }

		[Column("pick_id")]
        [ForeignKey("fk_ticket_bet_pick")]
        public Guid PickId { get; set; }

		[Column("status")]
        public TicketStatus Status { get; set; }

		[System.Text.Json.Serialization.JsonIgnore]
        public virtual Ticket Ticket { get; set; }

		public virtual Pick Pick { get; set; }
    }
}

