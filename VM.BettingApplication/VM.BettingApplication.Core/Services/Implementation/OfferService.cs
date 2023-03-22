using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using VM.BettingApplication.Core.DTO;
using VM.BettingApplication.Core.Entities;
using VM.BettingApplication.Core.Helpers;
using VM.BettingApplication.Core.Repository;
using VM.BettingApplication.Core.Services.Interface;
using VM.BettingApplication.Core.Settings;

namespace VM.BettingApplication.Core.Services.Implementation
{
	public class OfferService : IOfferService
	{
        private readonly ApplicationSettings _appSettings;
        private readonly ISportService _sportService;

		public OfferService(
            IOptions<ApplicationSettings> appSettings,
            ISportService sportService
        )
		{
            _appSettings = appSettings.Value;
            _sportService = sportService;
		}

        public async Task<Event[]> GetFullOffer()
        {
            using(DatabaseContext databaseContext = DatabaseContext.GenerateContext(_appSettings.DatabaseConnectionString))
            {
                var timeLimit = DateTime.UtcNow;

                return await databaseContext.Events
                    .Where(x => x.StartTime > timeLimit)
                    .Include(x => x.Picks)
                    .Include(x => x.Sport)
                    .ToArrayAsync();
            }
        }


        public async Task AddNewEvents(IEnumerable<Event> events, IEnumerable<Pick> picks)
        {
            using (DatabaseContext databaseContext = DatabaseContext.GenerateContext(_appSettings.DatabaseConnectionString))
            {
                databaseContext.AddRange(events);
                databaseContext.AddRange(picks);
                await databaseContext.SaveChangesAsync();
            }
        }

        public async Task UpdatePickStatus(Guid eventId, string pickId, TicketStatus status)
        {
            using (DatabaseContext databaseContext = DatabaseContext.GenerateContext(_appSettings.DatabaseConnectionString))
            {
                var pick = await databaseContext.Picks.SingleAsync(x =>
                    x.EventId == eventId &&
                    x.PickName == pickId
                );

                pick.Status = status;
                await databaseContext.SaveChangesAsync();
            }
        }

        public async Task<Dictionary<Sport, int>> GetNumberOfEventsForToday()
        {
            var allSports = await _sportService.GetAllSports();
            var minTime = DateTime.UtcNow.Date; //start of day
            var maxTime = minTime.AddDays(1).AddTicks(-1); //end of day

            using (DatabaseContext databaseContext =
                DatabaseContext.GenerateContext(_appSettings.DatabaseConnectionString)) {
                var eventsForToday = await databaseContext.Events.Where(x =>
                    x.StartTime >= minTime &&
                    x.StartTime <= maxTime
                ).ToArrayAsync();

                return allSports.Select(x =>
                    KeyValuePair.Create(x, eventsForToday.Where(y => y.SportId == x.Id).Count())
                ).ToDictionary(x => x.Key, x => x.Value);
            }
        }

        public async Task<Event> AddToTopOffer(Guid eventId)
        {
            using (DatabaseContext databaseContext =
                DatabaseContext.GenerateContext(_appSettings.DatabaseConnectionString))
            {
                var currentEvent = await databaseContext.Events
                    .Include(x => x.Picks)
                    .SingleAsync(x => x.Id == eventId);

                if (currentEvent.IsTopOffer)
                    throw new ArgumentException("already top offer");

                var newEvent = new Event
                {
                    Id = Guid.NewGuid(),
                    SportId = currentEvent.SportId,
                    StartTime = currentEvent.StartTime,
                    AwayTeam = currentEvent.AwayTeam,
                    HomeTeam = currentEvent.HomeTeam,
                    IsTopOffer = true,
                    LinkedId = currentEvent.Id
                };

                var newPicks = currentEvent.Picks.Select(x =>
                {
                    return new Pick()
                    {
                        Id = Guid.NewGuid(),
                        Status = x.Status,
                        PickName = x.PickName,
                        EventId = x.EventId,
                        Odds =
                        Math.Round((1 + (_appSettings.TopOfferOddsBoostPercentage / 100)) * x.Odds, 2)
                    };
                }).ToArray();

                databaseContext.Add(newEvent);
                databaseContext.AddRange(newPicks);
                await databaseContext.SaveChangesAsync();

                newEvent.Picks = newPicks;
                return newEvent;
            }
        }

        public async Task<OfferValidationResult> ValidateOffer(PayinTicketBets[] payinTicketBets)
        {
            using (DatabaseContext databaseContext =
                DatabaseContext.GenerateContext(_appSettings.DatabaseConnectionString))
            {
                var result = new OfferValidationResult
                {
                };

                var now = DateTime.Now;
                var pickIds = payinTicketBets.Select(x => x.PickId).ToArray();

                var picks = await databaseContext.Picks
                    .Where(x => pickIds.Contains(x.Id))
                    .Include(x => x.Event)
                    .ToArrayAsync();

                var picksValidation = payinTicketBets.Select(x =>
                {
                    var offerPick = picks.SingleOrDefault(y => y.Id == x.PickId);

                    if(offerPick == null)
                    {
                        return new OfferPickValidationResult
                        {
                            IsValid = false,
                            Message = ValidationMessages.InvalidPick
                        };
                    }

                    var pickResult = new OfferPickValidationResult
                    {
                        PickId = offerPick.Id,
                        Odds = offerPick.Odds
                    };

                    if(x.Odds != offerPick.Odds)
                    {
                        pickResult.Message = ValidationMessages.InvalidOdds;
                        return pickResult;
                    }

                    if(offerPick.Event.StartTime < now)
                    {
                        pickResult.Message = ValidationMessages.EventAlreadyStarted;
                        return pickResult;
                    }

                    pickResult.IsValid = true;
                    return pickResult;

                }).ToArray();

                if(
                    picks.Count(x => x.Event.IsTopOffer) > _appSettings.MaxEventsFromTopOffer ||
                    picks.Any(x => picks.Any(y => y.Event.IsTopOffer && y.Event.LinkedId == x.EventId)))
                {
                    result.IsValid = false;
                    result.Message = ValidationMessages.CombinationNotAllowed;
                    return result;
                }

                if(picks.Any(x => x.Event.IsTopOffer))
                {
                    var nonTopOfferPicks = picks.Where(x => !x.Event.IsTopOffer);
                    if(nonTopOfferPicks.Count() < _appSettings.MinimumEventsWithTopOffer)
                    {
                        result.IsValid = false;
                        result.Message = ValidationMessages.CombinationNotAllowed;
                        return result;
                    }

                    if(nonTopOfferPicks.Any(x => x.Odds < _appSettings.MinimumOddsWithTopOffer))
                    {
                        result.IsValid = false;
                        result.Message = ValidationMessages.CombinationNotAllowed;
                        return result;
                    }
                }

                result.IsValid = true;
                return result;
            }
        }
    }
}

