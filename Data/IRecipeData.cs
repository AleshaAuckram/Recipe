using Recipe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipe.Data
{
    public interface IRecipeData
    {
        // Origin / Category
        Task<int> AddAsync(Country country);
        Task<int> UpdateAsync(Country country);
        Task<int> DeleteAsync(Country country);
        Task<List<Country>> GetAsync();

        // Recipe 
        Task<int> AddRecipeAsync(MyRecipe myrecipe);
        Task<int> UpdateRecipeAsync(MyRecipe myrecipe);
        Task<int> DeleteRecipeAsync(MyRecipe myrecipe);
        Task<List<MyRecipe>> GetRecipesAsync();
        Task<MyRecipe> GetRecipeAsync(int id);
        Task<List<MyRecipe>> GetAllRecipesByOriginId(int originId);


        // Procedure 
        Task<int> AddProcedureAsync(Instruction instruction);
        Task<int> UpdateProcedureAsync(Instruction instruction);
        Task<int> DeleteProcedureAsync(Instruction instruction);
        Task<List<Instruction>> GetProceduresAsync();
        Task<Instruction> GetProcedureAsync(int id);
        Task<List<Instruction>> GetAllProceduresByRecipeId(int recipeId);
    }
}