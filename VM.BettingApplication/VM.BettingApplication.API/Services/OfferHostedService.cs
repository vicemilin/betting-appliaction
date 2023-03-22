using System;
using Microsoft.Extensions.Options;
using VM.BettingApplication.Core.Entities;
using VM.BettingApplication.Core.Helpers;
using VM.BettingApplication.Core.Services.Interface;
using VM.BettingApplication.Core.Settings;

namespace VM.BettingApplication.API.Services
{
	public class OfferHostedService : IHostedService
	{
        private readonly ApplicationSettings _appSettings;
        private readonly IOfferService _offerService;

        private Timer _offerCheckTimer;

        public OfferHostedService(
            IOptions<ApplicationSettings> appSettings,
            IOfferService offerService
        )
		{
            _appSettings = appSettings.Value;
            _offerService = offerService;
		}

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _offerCheckTimer = new Timer(OfferCheckTimerCallback, null, TimeSpan.Zero, TimeSpan.FromMinutes(30));
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _offerCheckTimer.DisposeAsync();
        }

        private async void OfferCheckTimerCallback(object? state)
        {
            var currentEventsCount = await _offerService.GetNumberOfEventsForToday();

            var minTime = DateTime.UtcNow.Date; //start of day
            var maxTime = minTime.AddDays(1).AddTicks(-1); //end of day

            var newEvents = new List<Event>();
            var newPicks = new List<Pick>();

            foreach(var e in currentEventsCount)
            {
                if(e.Value >= _appSettings.NumberOfEventsPerSportPerDay)
                    continue;
                var teams = _appSettings.SoccerTeams;

                switch (e.Key.Name)
                {
                    case "Basketball":
                        teams = _appSettings.BasketballTeams;
                        break;
                    case "Tennis":
                        teams = _appSettings.BasketballTeams;
                        break;
                    default:
                        break;
                }
                

                var result = OfferGenerator.GenerateEvents(
                    minTime,
                    maxTime,
                    _appSettings.NumberOfEventsPerSportPerDay - e.Value,
                    teams,
                    e.Key
                );

                newEvents.AddRange(result.Select(x => x.Event));
                newPicks.AddRange(result.SelectMany(x => x.Picks));
            }

            await _offerService.AddNewEvents(newEvents, newPicks);
        }
    }
}

