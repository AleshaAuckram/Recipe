using Recipe.ViewModels;
using Recipe.Views;
using Moq;
using Recipe.Data;
using Recipe.Models;
using Recipe.ImageConverter;

namespace UnitTest1
{
    public class UnitTest1
    {
        [Fact]
        public void ShowDialog_SetsHeaderTitle()
        {
            // Arrange
            var recipeDataMock = new Mock<IRecipeData>();
            var viewModel = new CountryPageViewModel(recipeDataMock.Object);

            // Act
            viewModel.ShowDialog();

            // Assert
            Assert.Equal("Add Country", viewModel.HeaderTitle);
        }


        [Fact]
        public void Base64Source_SetValue_GetValue()
        {
            // Arrange
            var instance = new ImageControl();
            string expectedValue = "Base64SourceProperty";

            // Act
            instance.Base64Source = expectedValue;
            string actualValue = instance.Base64Source;

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public async Task LoadProcedures_CallsGetProceduresAsyncAndGetRecipesAsync()
        {
            // Arrange
            var mockRecipeData = new Mock<IRecipeData>();
            var viewModel = new InstructionPageViewModel(mockRecipeData.Object);

            // Act
            await viewModel.LoadProcedures();

            // Assert
            mockRecipeData.Verify(rd => rd.GetProceduresAsync(), Times.Once);
            mockRecipeData.Verify(rd => rd.GetRecipesAsync(), Times.Once);
        }
    }
}