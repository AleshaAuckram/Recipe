using CommunityToolkit.Maui;
using Recipe.Data;
using Recipe.ViewModels;
using Recipe.Views;
using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Core.Hosting;

namespace Recipe
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureSyncfusionCore()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            builder.Services.AddSingleton<IRecipeData, RecipeData>();
            builder.Services.AddSingleton<CountryPageViewModel>();
            builder.Services.AddSingleton<CountryPage>();
            builder.Services.AddSingleton<RecipePageViewModel>();
            builder.Services.AddSingleton<RecipePage>();
            builder.Services.AddSingleton<InstructionPageViewModel>();
            builder.Services.AddSingleton<InstructionPage>();
            builder.Services.AddSingleton<HomePageViewModel>();
            builder.Services.AddSingleton<HomePage>();
            builder.Services.AddSingleton<ReadRecipeDetailsPageViewModel>();
            builder.Services.AddSingleton<ReadRecipeDetailsPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}