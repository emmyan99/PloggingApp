﻿using CommunityToolkit.Mvvm.ComponentModel;
using Plogging.Core.Models;

namespace PloggingApp.Features.Statistics;

public partial class PloggingStatistics : ObservableObject
{
    [ObservableProperty]
    private double totalSteps;
    [ObservableProperty]
    private double totalDistance;
    [ObservableProperty]
    private double totalCO2Saved;
    [ObservableProperty]
    private double totalWeight;
    [ObservableProperty]
    private TimeSpan totalTime;

    public PloggingStatistics(IEnumerable<PlogSession> sessions)
    {
        TotalSteps = sessions.Sum(s => s.PloggingData.Steps);
        TotalDistance = Math.Round(sessions.Sum(s => s.PloggingData.Distance), 1);
        TotalCO2Saved = Math.Round(CO2SavedCalculator.CalculateCO2Saved(sessions), 2);
        TotalWeight = Math.Round(sessions.Sum(s => s.PloggingData.Litters.Sum(l => l.Weight)), 1);
        TotalTime = calculateTime(sessions);
    }
    public PloggingStatistics(PlogSession session)
    {
        TotalSteps = session.PloggingData.Steps;
        TotalDistance = Math.Round(session.PloggingData.Distance, 2);
        TotalCO2Saved = Math.Round(CO2SavedCalculator.CalculateCO2Saved(session), 2);
        TotalWeight = Math.Round(session.PloggingData.Litters.Sum(s => s.Weight), 2);
        TotalTime = session.EndDate - session.StartDate;
    }
    private TimeSpan calculateTime(IEnumerable<PlogSession> sessions)
    {
        TimeSpan totalTime = default(TimeSpan);
        foreach(var s in sessions)
        {
            totalTime += (s.EndDate - s.StartDate);
        }
        return totalTime;
    }
}
