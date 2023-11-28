using Recipe.ViewModels;

namespace Recipe.Views;

public partial class ReadRecipeDetailsPage : ContentPage
{
    public ReadRecipeDetailsPage(ReadRecipeDetailsPageViewModel recipeDetailsPageViewModel)
    {
        InitializeComponent();
        BindingContext = recipeDetailsPageViewModel;
    }
}