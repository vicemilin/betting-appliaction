using System;
using VM.BettingApplication.Core.Entities;

namespace VM.BettingApplication.Core.Helpers
{
	public static class OfferGenerator
	{
        private static Random _rng = new Random();

        public static EventWithPicks[] GenerateEvents(
            DateTime startTime,
            DateTime endTime,
            int number,
            string[] teams,
            Sport sport)
        {
            if(endTime < startTime)
                throw new ArgumentException("Invalid timespan");

            var timeDiff = (int)endTime.Subtract(startTime).TotalMinutes;

            var result = new EventWithPicks[number];

            for(int i = 0; i < number; i++)
            {
                var eventStartTime = startTime.AddMinutes(_rng.Next(0, timeDiff));
                var homeTeam = teams[_rng.Next(0, teams.Length)];
                var awayTeam = teams
                    .Where(x => x != homeTeam)
                    .ElementAt(_rng.Next(0, teams.Length - 1));

                var newEvent = new Event
                {
                    HomeTeam = homeTeam,
                    AwayTeam = awayTeam,
                    Id = Guid.NewGuid(),
                    StartTime = eventStartTime,
                    SportId = sport.Id 
                };

                var newPicks = sport.AvailablePicks.Select(x =>
                {
                    return new Pick()
                    {
                        Id = Guid.NewGuid(),
                        Status = TicketStatus.InProgress,
                        EventId = newEvent.Id,
                        Odds = (decimal)_rng.Next(101, 500) / (decimal)100,
                        PickName = x
                    };
                }).ToArray();

                result[i] = new EventWithPicks { Event = newEvent, Picks = newPicks };
            }

            return result;
        }
    }


    public class EventWithPicks
    {
        public Event Event { get; set; }
        public Pick[] Picks { get; set; }
    }
}

