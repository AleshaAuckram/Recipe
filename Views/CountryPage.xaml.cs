using Recipe.ViewModels;

namespace Recipe.Views;

public partial class CountryPage : ContentPage
{
    private readonly CountryPageViewModel originPageViewModel;

    public CountryPage(CountryPageViewModel originPageViewModel)
    {
        InitializeComponent();
        BindingContext = originPageViewModel;
        this.originPageViewModel = originPageViewModel;
    }

    protected override void OnAppearing()
    {
        originPageViewModel.LoadOriginsCommand.Execute(this);
    }
}