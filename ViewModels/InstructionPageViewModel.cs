using Recipe.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmHelpers;
using System.Collections.ObjectModel;
using Recipe.Models;

namespace Recipe.ViewModels
{
    public partial class InstructionPageViewModel : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
    {
        private readonly IRecipeData recipeData;
        public InstructionPageViewModel(IRecipeData recipeData)
        {
            this.recipeData = recipeData;
            Title = "Manage Procedures";
        }

        [ObservableProperty]
        private string title;

        [ObservableProperty]
        private Instruction procedureObject;

        [ObservableProperty]
        private bool showPopup;

        [ObservableProperty]
        private string headerTitle;

        public ObservableCollection<MyRecipe> RecipeObjects { get; set; } = new();
        public ObservableRangeCollection<Instruction> ProcedureObjects { get; set; } = new();


        [ObservableProperty]
        private Instruction selectedRowData;
        partial void OnSelectedRowDataChanged(Instruction oldValue, Instruction newValue)
        {
            if (oldValue != newValue)
                ManageSelectedData(newValue);
        }

        [ObservableProperty]
        private MyRecipe selectedRecipe;
        partial void OnSelectedRecipeChanged(MyRecipe oldValue, MyRecipe newValue)
        {
            try
            {
                if (newValue is null) return;
                if (newValue.Id <= 0) return;
                ProcedureObject.RecipeId = newValue.Id;
            }
            catch (Exception) { return; }
        }

        [ObservableProperty]
        private MyRecipe getProceduresByRecipe;

        partial void OnGetProceduresByRecipeChanged(MyRecipe value)
        {
            CallGetProceduresByRecipeMethod(value.Id);
        }

        [RelayCommand]
        private void ShowDialog()
        {
            HeaderTitle = "Add Procedure";
            ProcedureObject = new Instruction();
            ShowPopup = true;
        }


        [RelayCommand]
        private async Task SaveObject()
        {
            if (ProcedureObject.RecipeId <= 0 || ProcedureObject.Title is null) return;

            int result;
            if (ProcedureObject.Id > 0)
                result = await recipeData.UpdateProcedureAsync(ProcedureObject);
            else
                result = await recipeData.AddProcedureAsync(ProcedureObject);

            if (result > 0)
            {
                await ToastModel.MakeToast("Process completed successfully");
                ShowPopup = false;
                ProcedureObject = new Instruction();
                await LoadProcedures();
            }
        }

        [RelayCommand]
        private async Task LoadProcedures()
        {
            var results = await recipeData.GetProceduresAsync();
            if (ProcedureObjects.Count > 0)
                ProcedureObjects.Clear();

            if (results is not null)
            {
                if (results.Count > 0)
                    ProcedureObjects.AddRange(results.OrderByDescending(_ => _.Id));
            }


            // get origin as well
            var recipeResults = await recipeData.GetRecipesAsync();
            if (RecipeObjects.Count > 0)
                RecipeObjects.Clear();
            if (recipeResults is not null)
            {
                foreach (var recipeObject in recipeResults)
                    RecipeObjects.Add(recipeObject);
            }
        }

        private async Task ManageSelectedData(Instruction selectedProcedureModel)
        {
            if (selectedProcedureModel is null) return;
            string action = await Shell.Current.DisplayActionSheet("Action: Choose an Option", "Cancel", null, "Edit", "Delete");
            if (string.IsNullOrEmpty(action) || string.IsNullOrWhiteSpace(action)) return;

            if (action.Equals("Cancel")) return;

            if (action.Equals("Edit"))
            {
                ProcedureObject = new Instruction();
                ProcedureObject = selectedProcedureModel;
                HeaderTitle = "Update Procedure";
                ShowPopup = true;
            }

            if (action.Equals("Delete"))
            {
                bool answer = await Shell.Current.DisplayAlert("Confirm Operation", "Are you sure you wanna do this?", "Yes", "No");
                if (!answer) return;

                int result = await recipeData.DeleteProcedureAsync(selectedProcedureModel);
                if (result > 0)
                {
                    await ToastModel.MakeToast("Origin deleted successfully");
                    await LoadProcedures();
                    selectedProcedureModel = new Instruction();
                    return;
                }

                await Shell.Current.DisplayAlert("Alert", "Error occured", "Ok");
                return;
            }
        }


        private async void CallGetProceduresByRecipeMethod(int recipeId)
        {

            var result = await recipeData.GetAllProceduresByRecipeId(recipeId);
            if (ProcedureObjects is not null)
                ProcedureObjects.Clear();

            if (result is not null)
                ProcedureObjects.AddRange(result);
        }
    }
}