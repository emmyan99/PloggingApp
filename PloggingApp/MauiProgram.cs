﻿using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Maps;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using Plogging.Core.Models;
using PloggingApp.Data.Services;
using PloggingApp.Data.Services.ApiClients;
using PloggingApp.Data.Services.Interfaces;
using PloggingApp.MVVM.ViewModels;
using PloggingApp.MVVM.Views;
using PloggingApp.Pages;
using PloggingApp.Pages.Dashboard;
using PloggingApp.Services.Camera;
using PloggingApp.Services.PloggingTracking;
using RestSharp;
using System.Reflection;
using ZXing.Net.Maui.Controls;

namespace PloggingApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseMauiCommunityToolkitCore()
            .AddAppSettings()
            .UseMauiCommunityToolkitMaps("AoUR4E62oR7u3eyHLolc9rR0ofWn0p0DrczTs1d6oIQCwkUmla3SCdnzdftVvCMS") /*FÖR WINDOWS */
            .UseMauiMaps() /*android och IOS specific*/
            .UseBarcodeReader()

            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
        AddApiClients(builder);
        AddViewModels(builder);
        AddPopups(builder);
        AddPages(builder);
        AddServices(builder);

        builder.ConfigureMauiHandlers(handlers =>
        {
            handlers.AddHandler<Microsoft.Maui.Controls.Maps.Map, CustomMapHandler>();
        });

#if DEBUG
        builder.Logging.AddDebug();
#endif


        return builder.Build();
    }

    private static void AddViewModels(MauiAppBuilder builder)
    {
        //Pages ViewModels
        builder.Services.AddTransient<RankingViewmodel>();
        builder.Services.AddTransient<DashBoardViewModel>();
        builder.Services.AddTransient<MapPageViewModel>();

        builder.Services.AddScoped<removeViewmodel>();
        builder.Services.AddScoped<CheckoutImageViewModel>();

        //Views ViewModels
        builder.Services.AddTransient<LeaderboardViewModel>();
        builder.Services.AddTransient<StreakViewModel>();
        builder.Services.AddTransient<PlogTogetherViewModel>();
        builder.Services.AddTransient<GenerateQRcodeViewModel>();
    }

    private static void AddPopups(MauiAppBuilder builder)
    {
        builder.Services.AddTransientPopup<AcceptPopup, AcceptPopupViewModel>();
    }

    private static void AddPages(MauiAppBuilder builder)
    {
        builder.Services.AddTransient<RankingPage>();
        builder.Services.AddTransient<DashboardPage>();

        builder.Services.AddTransient<MapPage>();

        builder.Services.AddTransient<DashboardPage>();

        builder.Services.AddScoped<CheckoutImagePage>();
        builder.Services.AddScoped<GenerateQRcodePage>();
        builder.Services.AddScoped<ScanQRcodePage>();

    }

    private static void AddServices(MauiAppBuilder builder)
    {
        builder.Services.AddTransient<IRankingService, RankingService>();
        builder.Services.AddTransient<IStreakService, StreakService>();
        builder.Services.AddScoped<ICameraService, CameraService>();
        builder.Services.AddTransient<IPloggingSessionTracker, PloggingSessionTracker>();
        builder.Services.AddTransient<IPloggingSessionService, PloggingSessionService>();
        builder.Services.AddTransient<IPlogTogetherService, PlogTogetherService>();
    }

    private static void AddApiClients(MauiAppBuilder builder)
    {
        var apiUrl = builder.Configuration["ApiUrls:PloggingApiUrl"];
        if (apiUrl != null)
        {
            var ploggingApiClient = new RestClient(apiUrl);
            builder.RegisterPloggingApiClient<PloggingSession>(ploggingApiClient);
            builder.RegisterPloggingApiClient<UserStreak>(ploggingApiClient);
            builder.RegisterPloggingApiClient<PlogTogether>(ploggingApiClient);
        }
    }

    private static void RegisterPloggingApiClient<T>(this MauiAppBuilder builder, IRestClient restClient)
    {
        builder.Services.AddTransient<IPloggingApiClient<T>>(serviceProvider =>
        {
            return new PloggingApiClient<T>(restClient);
        });
    }

    private static MauiAppBuilder AddAppSettings(this MauiAppBuilder builder)
    {
        var environment = Environment.GetEnvironmentVariable("MAUI_ENVIRONMENT") ?? "Production";
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"PloggingApp.appsettings.{environment}.json");
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

        if (stream != null)
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                .AddJsonStream(stream)
                .Build();

            builder.Configuration.AddConfiguration(config);
        }

        return builder;
    }
}
