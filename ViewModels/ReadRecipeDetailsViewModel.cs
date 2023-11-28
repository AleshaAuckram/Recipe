using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using Recipe.Models;

namespace Recipe.ViewModels
{
    [QueryProperty(nameof(TappedRecipe), "SelectedRecipe")]
    public partial class ReadRecipeDetailsPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string title;

        [ObservableProperty]
        private MyRecipe tappedRecipe;

        public ReadRecipeDetailsPageViewModel()
        {
            Title = "Recipe Details";
        }
    }
}