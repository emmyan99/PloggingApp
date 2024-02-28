﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plogging.Core.Enums;
using Plogging.Core.Models;
using PloggingApp.Data.Services;
using PloggingApp.Extensions;
using PloggingApp.Pages;
using System.Collections.ObjectModel;

namespace PloggingApp.MVVM.ViewModels;

public partial class LeaderboardViewModel : BaseViewModel, IAsyncInitialization
{
    private readonly IRankingService _rankingService;
    public ObservableCollection<UserRanking> Rankings { get; set; } = [];
    private IEnumerable<UserRanking> _allRankings = new ObservableCollection<UserRanking>();
    public SortProperty[] SortProperties { get; set; } = (SortProperty[])Enum.GetValues(typeof(SortProperty));
    public SortProperty SelectedSortProperty
    {
        get => _selectedSortProperty;
        set
        {
            _selectedSortProperty = value;
            SortUnit = value.GetUnitOfMeasurement();

            if (RecentRankingCommand != null && RecentRankingCommand.CanExecute(this))
            {
                RecentRankingCommand.Execute(this);
            }

            OnPropertyChanged(nameof(SortUnit));
            OnPropertyChanged(nameof(SelectedSortProperty));
        }
    }
    private SortProperty _selectedSortProperty;
    public string SortUnit { get; set; } = "";
    private IRelayCommand? RecentRankingCommand { get; set; }
    public Task Initialization { get; private set; }

    [ObservableProperty]
    private UserRanking userRank;

    public LeaderboardViewModel(IRankingService rankingService)
    {
        _rankingService = rankingService;

        Initialization = InitializeAsync();
    }

    private async Task InitializeAsync()
    {
        await GetYearlyRankings();
    }

    [RelayCommand]
    private async Task GetMonthlyRankings()
    {
        RecentRankingCommand = GetMonthlyRankingsCommand;
        IsBusy = true;

        _allRankings = await _rankingService.GetUserRankings(DateTime.UtcNow.FirstDateInMonth(), DateTime.UtcNow.LastDateInMonth(), SelectedSortProperty);
        Rankings.ClearAndAddRange(_allRankings);
        UserRank = GetUserRank();

        IsBusy = false;
    }

    [RelayCommand]
    private async Task GetYearlyRankings()
    {
        RecentRankingCommand = GetYearlyRankingsCommand;
        IsBusy = true;

        _allRankings = await _rankingService.GetUserRankings(DateTime.UtcNow.FirstDateInYear(), DateTime.UtcNow.LastDateInYear(), SelectedSortProperty);
        Rankings.ClearAndAddRange(_allRankings);
        UserRank = GetUserRank();

        IsBusy = false;
    }

    [RelayCommand]
    private async Task GetAllTimeRankings()
    {
        RecentRankingCommand = GetAllTimeRankingsCommand;
        IsBusy = true;

        _allRankings = await _rankingService.GetUserRankings(DateTime.MinValue, DateTime.UtcNow.LastDateInYear(), SelectedSortProperty);
        Rankings.ClearAndAddRange(_allRankings);
        UserRank = GetUserRank();

        IsBusy = false;
    }

    public UserRanking GetUserRank()
    {
        var currentUserId = "123ajsldkfjasödjfk34"; //TODO replace with actual id when user authentication is implemented

        var userRank = _allRankings.FirstOrDefault(user => user.Id.Equals(currentUserId, StringComparison.InvariantCultureIgnoreCase));

        return userRank;
    }

    [RelayCommand]
    private void SearchUsers(string userName)
    {
        if (!userName.Equals(""))
        {
            var searchResults = _allRankings.Where(x => x.DisplayName.Contains(userName, StringComparison.InvariantCultureIgnoreCase));
            Rankings.ClearAndAddRange(searchResults);
        }
        else
        {
            Rankings.ClearAndAddRange(_allRankings);
        }
    }
    [RelayCommand]
    private async Task GoToProfilePage()
    {
        await Shell.Current.GoToAsync($"//{nameof(OthersProfilePage)}");
    }

}
