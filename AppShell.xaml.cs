using Recipe.Views;
namespace Recipe
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(CountryPage), typeof(CountryPage));
            Routing.RegisterRoute(nameof(RecipePage), typeof(RecipePage));
            Routing.RegisterRoute(nameof(CountryPage), typeof(CountryPage));
            Routing.RegisterRoute(nameof(ReadRecipeDetailsPage), typeof(ReadRecipeDetailsPage));
        }
    }
}