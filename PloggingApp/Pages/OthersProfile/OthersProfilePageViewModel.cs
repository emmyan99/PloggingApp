﻿using CommunityToolkit.Mvvm.Input;
using Plogging.Core.Models;
using PloggingApp.Data.Services.Interfaces;
using PloggingApp.MVVM.ViewModels;
namespace PloggingApp.Pages;

public class OthersProfilePageViewModel : Page
{
    public OthersSessionsViewModel OthersSessionsViewModel{ get; set; }
    public OthersProfilePageViewModel(IPloggingSessionService sessionService)
    {
        OthersSessionsViewModel = new OthersSessionsViewModel(sessionService);

        [RelayCommand]
        async Task GoBack()
        {
            await Shell.Current.GoToAsync($"//{nameof(RankingPage)}");
        }
    }



}
