using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Runtime;
using Newtonsoft.Json;
using VM.BettingApplication.Core.Entities;

namespace VM.BettingApplication.Core.Repository
{
    public partial class DatabaseContext : DbContext
    {
        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<TicketBet> TicketBets { get; set; }
        public virtual DbSet<Sport> Sports { get; set; }
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<Pick> Picks { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema("sport");

            #region PKs

            builder.Entity<Ticket>().HasKey(x => x.Id);
            builder.Entity<TicketBet>().HasKey(x => x.Id);
            builder.Entity<Sport>().HasKey(x => x.Id);
            builder.Entity<Event>().HasKey(x => x.Id);
            builder.Entity<Pick>().HasKey(x => x.Id);

            #endregion

            #region Relations

            builder.Entity<Ticket>()
                .HasMany<TicketBet>(x => x.TicketBets)
                .WithOne(x => x.Ticket)
                .HasForeignKey(x => x.TicketId);

            builder.Entity<Event>()
                .HasMany<Pick>(x => x.Picks)
                .WithOne(x => x.Event)
                .HasForeignKey(x => x.EventId);

            builder.Entity<Event>()
                .HasOne<Sport>(x => x.Sport);

            builder.Entity<TicketBet>()
                .HasOne<Pick>(x => x.Pick);

            #endregion

            #region Conversions

            builder.Entity<Ticket>()
                .Property(x => x.Status)
                .HasConversion(
                    x => (short)x,
                    x => (TicketStatus)x);

            builder.Entity<TicketBet>()
                .Property(x => x.Status)
                .HasConversion(
                    x => (short)x, 
                    x => (TicketStatus)x);

            builder.Entity<Sport>()
                .Property(x => x.AvailablePicks)
                .HasConversion(
                    x =>  JsonConvert.SerializeObject(x),
                    x => JsonConvert.DeserializeObject<string[]>(x)
                );

            #endregion
        }

        public static DatabaseContext GenerateContext(string connectionString)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            var builder = new DbContextOptionsBuilder<DatabaseContext>();
            builder.UseNpgsql(connectionString);
            return new DatabaseContext(builder.Options);
        }
    }
}
