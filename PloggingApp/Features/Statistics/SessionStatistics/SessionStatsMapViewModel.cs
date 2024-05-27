﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using PloggingApp.Features.Map.Components;
using PloggingApp.Features.PloggingSession;
using PlogPal.Domain.Models;
using System.Collections.ObjectModel;

namespace PloggingApp.Features.Statistics;

public partial class SessionStatsMapViewModel : ObservableObject
{ 
    public ObservableCollection<LocationPin> TrashPins { get; set; } = [];
    public ObservableCollection<MapPoint> LocationPins { get; set; } = [];



    [ObservableProperty]
    private PlogSession ploggingSession;

    public SessionStatsMapViewModel()
    {

    }
    public void Initialize(PlogSession ploggingSession)
     {
        PloggingSession = ploggingSession;
        PlaceTrashPins();
        LocationPins = new ObservableCollection<MapPoint>(ploggingSession.PloggingRoute);
        WeakReferenceMessenger.Default.Send(new PloggingSessionMessage(true, []));
    }

    private void PlaceTrashPins()
    {
        foreach(var location in PloggingSession.PloggingData.Litters) 
        {
            TrashPins.Add(new CollectedLitterPin()
            {
                Label = "Litter",
                Location = new Location()
                {
                    Latitude = location.LitterLocation.Latitude,
                    Longitude = location.LitterLocation.Longitude
                }
            });
        }
    }

}
