using Recipe.ViewModels;

namespace Recipe.Views;

public partial class InstructionPage : ContentPage
{
    private readonly InstructionPageViewModel instructionPageViewModel;

    public InstructionPage(InstructionPageViewModel procedurePageViewModel)
    {
        InitializeComponent();
        BindingContext = instructionPageViewModel;
        this.instructionPageViewModel = instructionPageViewModel;
    }

    protected override void OnAppearing()
    {
        instructionPageViewModel.LoadProceduresCommand.Execute(this);
    }
}