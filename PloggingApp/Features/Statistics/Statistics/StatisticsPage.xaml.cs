namespace PloggingApp.Features.Statistics;

public partial class StatisticsPage : ContentPage
{
	public StatisticsPage(StatisticsPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}