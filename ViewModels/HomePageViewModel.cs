using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Recipe.Models;
using Recipe.Data;
using Recipe.Views;
using MvvmHelpers;

namespace Recipe.ViewModels
{
    public partial class HomePageViewModel : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
    {
        private readonly IRecipeData recipeData;
        public HomePageViewModel(IRecipeData recipeData)
        {
            this.recipeData = recipeData;
            Title = "Recipes Home";
        }

        [ObservableProperty]
        private string title;

        public ObservableRangeCollection<MyRecipe> RecipeObjects { get; set; } = new();

        [RelayCommand]
        private async Task LoadData()
        {
            var result = await recipeData.GetRecipesAsync();
            RecipeObjects?.Clear();

            if (result is not null)
                RecipeObjects.AddRange(result);
        }

        [RelayCommand]
        private async void GotoDetails(MyRecipe recipeDTO)
        {
            var navigationParameter = new Dictionary<string, object>
            {
                { "SelectedRecipe",  recipeDTO}
            };

            // Navigate to the Recipe Route with the Navigation Parameter
            await Shell.Current.GoToAsync(nameof(ReadRecipeDetailsPage), navigationParameter);
        }

    }
}