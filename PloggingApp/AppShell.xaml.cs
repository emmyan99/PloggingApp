﻿using PloggingApp.Pages;

namespace PloggingApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(CheckoutImagePage), typeof(CheckoutImagePage));
            Routing.RegisterRoute(nameof(DashboardPage), typeof(DashboardPage));
            Routing.RegisterRoute(nameof(ScanQRcodePage), typeof(ScanQRcodePage));
            Routing.RegisterRoute(nameof(GenerateQRcodePage), typeof(GenerateQRcodePage));
            Routing.RegisterRoute(nameof(MyProfilePage), typeof(MyProfilePage));
        }

        private async void GoToMyProfilePage(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("MyProfilePage");

        }
    }
}
