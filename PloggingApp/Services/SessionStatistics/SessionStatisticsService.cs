using Plogging.Core.Models;
using PloggingApp.Services.SessionStatistics;
using PloggingApp.Features.Statistics;

namespace PloggingApp.Services.Authentication;

public class SessionStatisticsService : ISessionStatisticsService
{
    public async Task GoToSessionStats(PlogSession session)
    {
        if (session is null)
            return;
        await Shell.Current.GoToAsync($"{nameof(SessionStatisticsPage)}", true,
            new Dictionary<string, object>
            {
                {nameof(PlogSession), session}
            });
    }
}
