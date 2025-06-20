﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Recipe.Models;
using Recipe.Data;
using MvvmHelpers;
using System.Collections.ObjectModel;

namespace Recipe.ViewModels
{
    public partial class RecipePageViewModel : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
    {
        private readonly IRecipeData recipeData;
        public RecipePageViewModel(IRecipeData recipeService)
        {
            this.recipeData = recipeService;
            Title = "Manage Recipes";
        }

        [ObservableProperty]
        private string title;

        [ObservableProperty]
        private string imageSource;

        [ObservableProperty]
        private MyRecipe recipeObject;

        partial void OnRecipeObjectChanged(MyRecipe value)
        {
            ImageSource = value.Image;
        }

        [ObservableProperty]
        private bool showPopup;

        [ObservableProperty]
        private string headerTitle;

        public ObservableRangeCollection<MyRecipe> RecipeObjects { get; set; } = new();
        public ObservableCollection<Country> OriginObjects { get; set; } = new();

        [ObservableProperty]
        private MyRecipe selectedRowData;

        partial void OnSelectedRowDataChanged(MyRecipe oldValue, MyRecipe newValue)
        {
            if (oldValue != newValue)
                ManageSelectedData(newValue);
        }

        [ObservableProperty]
        private Country selectedOrigin;
        partial void OnSelectedOriginChanged(Country oldValue, Country newValue)
        {
            try
            {
                if (newValue is null) return;
                if (newValue.Id <= 0) return;
                RecipeObject.OriginId = newValue.Id;
            }
            catch (Exception) { return; }
        }

        [ObservableProperty]
        private Country getRecipeByOrigin;

        partial void OnGetRecipeByOriginChanged(Country value)
        {
            GetAllRecipesByOriginId(value.Id);
        }

        [RelayCommand]
        private void ShowDialog()
        {
            HeaderTitle = "Add Recipe";
            RecipeObject = new MyRecipe();
            ShowPopup = true;
        }

        [RelayCommand]
        private async Task SaveObject()
        {
            if (RecipeObject.OriginId <= 0 || RecipeObject.Name is null) return;

            int result;
            if (RecipeObject.Id > 0)
                result = await recipeData.UpdateRecipeAsync(RecipeObject);
            else
                result = await recipeData.AddRecipeAsync(RecipeObject);

            if (result > 0)
            {
                await ToastModel.MakeToast("Process completed successfully");
                ShowPopup = false;
                RecipeObject = new MyRecipe();
                await LoadRecipes();
            }
        }

        [RelayCommand]
        private async Task LoadRecipes()
        {
            var results = await recipeData.GetRecipesAsync();
            if (RecipeObjects.Count > 0)
                RecipeObjects.Clear();

            if (results is not null)
            {
                if (results.Count > 0)
                    RecipeObjects.ReplaceRange(results.OrderByDescending(_ => _.Id));
            }
            // get origin as well
            var originResults = await recipeData.GetAsync();
            if (OriginObjects.Count > 0)
                OriginObjects.Clear();

            if (originResults.Count > 0)
                foreach (var originObject in originResults)
                    OriginObjects.Add(originObject);
        }

        private async Task ManageSelectedData(MyRecipe selectedRecipeModel)
        {
            if (selectedRecipeModel is null) return;
            string action = await Shell.Current.DisplayActionSheet("Action: Choose an Option", "Cancel", null, "Edit", "Delete");
            if (string.IsNullOrEmpty(action) || string.IsNullOrWhiteSpace(action)) return;

            if (action.Equals("Cancel")) return;

            if (action.Equals("Edit"))
            {
                RecipeObject = new MyRecipe();
                RecipeObject = selectedRecipeModel;
                HeaderTitle = "Update Recipe";
                ShowPopup = true;
            }

            if (action.Equals("Delete"))
            {
                bool answer = await Shell.Current.DisplayAlert("Confirm Operation", "Are you sure you wanna do this?", "Yes", "No");
                if (!answer) return;

                int result = await recipeData.DeleteRecipeAsync(selectedRecipeModel);
                if (result > 0)
                {
                    await ToastModel.MakeToast("Origin deleted successfully");
                    await LoadRecipes();
                    selectedRecipeModel = new MyRecipe();
                    return;
                }

                await Shell.Current.DisplayAlert("Alert", "Error occured", "Ok");
                return;
            }
        }

        private async void GetAllRecipesByOriginId(int originId)
        {
            var result = await recipeData.GetAllRecipesByOriginId(originId);
            if (RecipeObjects is not null)
                RecipeObjects.Clear();

            if (result is not null)
                RecipeObjects.AddRange(result);
        }


        [RelayCommand]
        private async Task BrowserImage()
        {
            var image = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Select Product Image",
                FileTypes = FilePickerFileType.Images
            });

            if (image is null)
                return;

            byte[] imageByte;
            var newFile = Path.Combine(FileSystem.CacheDirectory, image.FileName);
            var stream = await image.OpenReadAsync();
            using (MemoryStream memory = new())
            {
                stream.CopyTo(memory);
                imageByte = memory.ToArray();
            }
            //converting to base64string
            var convertedImage = Convert.ToBase64String(imageByte);
            RecipeObject.Image = convertedImage;
            ImageSource = convertedImage;
        }
    }
}