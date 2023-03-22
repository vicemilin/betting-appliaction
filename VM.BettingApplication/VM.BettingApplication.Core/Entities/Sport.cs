using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace VM.BettingApplication.Core.Entities
{
	[Table("sport")]
	public class Sport
	{ 
		[Column("id")]
		public Guid Id { get; set; }

		[Column("name")]
		public string Name { get; set; }

		[Column("available_picks", TypeName = "jsonb")]
		public string[] AvailablePicks { get; set; }
	}
}

