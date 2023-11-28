using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmHelpers;
using Recipe.Data;
using Recipe.Models;


namespace Recipe.ViewModels
{
    public partial class CountryPageViewModel : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
    {

        private readonly IRecipeData recipeData;
        public CountryPageViewModel(IRecipeData recipeData)
        {
            this.recipeData = recipeData;
            ShowPopup = false;
        }

        [ObservableProperty]
        private string title;

        [ObservableProperty]
        private Country originObject;

        [ObservableProperty]
        private bool showPopup;

        [ObservableProperty]
        private string headerTitle;

        public ObservableRangeCollection<Country> OriginObjects { get; set; } = new();

        [ObservableProperty]
        private Country selectedRowData;

        partial void OnSelectedRowDataChanged(Country oldValue, Country newValue)
        {
            if (oldValue != newValue)
                ManageSelectedData(newValue);
        }

        [RelayCommand]
        private void ShowDialog()
        {
            HeaderTitle = "Add Origin";
            OriginObject = new Country();
            ShowPopup = true;
        }

        [RelayCommand]
        private async Task SaveObject()
        {
            if (OriginObject == null) return;

            int result;
            if (OriginObject.Id > 0)
                result = await recipeData.UpdateAsync(OriginObject);
            else
                result = await recipeData.AddAsync(OriginObject);

            if (result > 0)
            {
                await ToastModel.MakeToast("Process completed successfully");
                ShowPopup = false;
                OriginObject = new Country();
                await LoadOrigins();
            }

        }

        [RelayCommand]
        private async Task LoadOrigins()
        {
            var results = await recipeData.GetAsync();
            if (OriginObjects.Count > 0)
                OriginObjects.Clear();

            if (results.Count > 0)
                OriginObjects.ReplaceRange(results.OrderByDescending(_ => _.Id));
        }

        private async Task ManageSelectedData(Country selectedOriginModel)
        {
            if (selectedOriginModel is null) return;
            string action = await Shell.Current.DisplayActionSheet("Action: Choose an Option", "Cancel", null, "Edit", "Delete");
            if (string.IsNullOrEmpty(action) || string.IsNullOrWhiteSpace(action)) return;

            if (action.Equals("Cancel")) return;

            if (action.Equals("Edit"))
            {
                OriginObject = new Country()
                {
                    Id = selectedOriginModel.Id,
                    Name = selectedOriginModel.Name
                };
                HeaderTitle = "Update Origin";
                ShowPopup = true;
            }

            if (action.Equals("Delete"))
            {
                bool answer = await Shell.Current.DisplayAlert("Confirm Operation", "Are you sure you wanna do this?", "Yes", "No");
                if (!answer) return;

                int result = await recipeData.DeleteAsync(selectedOriginModel);
                if (result > 0)
                {
                    await ToastModel.MakeToast("Origin deleted successfully");
                    await LoadOrigins();
                    selectedOriginModel = new Country();
                    return;
                }

                await Shell.Current.DisplayAlert("Alert", "Error occured", "Ok");
                return;

            }

        }
    }
}