using Recipe.ViewModels;

namespace Recipe.Views;

public partial class HomePage : ContentPage
{
    private readonly HomePageViewModel homePageViewModel;

    public HomePage(HomePageViewModel homePageViewModel)
    {
        InitializeComponent();
        BindingContext = homePageViewModel;
        this.homePageViewModel = homePageViewModel;
    }

    protected override void OnAppearing()
    {
        homePageViewModel.LoadDataCommand.Execute(this);
    }

    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        string googleFormUrl = "https://forms.gle/coJxSk7GWokafx2j6";

        // Open the URL in the default browser
        Launcher.OpenAsync(new Uri(googleFormUrl));

    }
}