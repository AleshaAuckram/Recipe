using Recipe.Models;
using SQLite;
using SQLiteNetExtensionsAsync.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipe.Data
{
    public class RecipeData : IRecipeData
    {
        private SQLiteAsyncConnection connection;
        public RecipeData()
        {
            SetupDatabase();
        }
        private async void SetupDatabase()
        {
            if (connection is null)
            {
                string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DemoMauiRecipe1.db3");
                connection = new SQLiteAsyncConnection(dbPath);
                // await connection.CreateTablesAsync<Procedure, Origin, Recipe>();
                await connection.CreateTableAsync<Instruction>();
                await connection.CreateTableAsync<MyRecipe>();
                await connection.CreateTableAsync<Country>();
            }
        }

        // Origin Service
        public async Task<int> AddAsync(Country country)
        {
            if (country is null)
                return (int)System.Net.HttpStatusCode.BadRequest;

            int result = await connection.InsertAsync(country);
            return result;
        }
        public async Task<int> DeleteAsync(Country country)
        {
            var result = await connection?.DeleteAsync(country);
            return result;
        }
        public async Task<List<Country>> GetAsync()
        {
            var result = await connection.Table<Country>().ToListAsync();
            return result;
        }

        public async Task<int> UpdateAsync(Country country)
        {
            var result = await connection.UpdateAsync(country);
            return result;
        }


        //Recipe
        public async Task<int> AddRecipeAsync(MyRecipe myrecipe)
        {
            if (myrecipe is null)
                return (int)System.Net.HttpStatusCode.BadRequest;

            int result = await connection.InsertAsync(myrecipe);
            return result;
        }
        public async Task<int> DeleteRecipeAsync(MyRecipe myrecipe)
        {
            var result = await connection?.DeleteAsync(myrecipe);
            return result;
        }
        public async Task<int> UpdateRecipeAsync(MyRecipe recipe)
        {
            var result = await connection.UpdateAsync(recipe);
            return result;
        }
        public async Task<List<MyRecipe>> GetRecipesAsync()
        {
            var result = await connection.GetAllWithChildrenAsync<MyRecipe>(recursive: true);
            if (result is null) return null;
            return result.ToList();
        }
        public async Task<MyRecipe> GetRecipeAsync(int id)
        {
            var result = await connection.Table<MyRecipe>().Where(_ => _.Id == id).FirstOrDefaultAsync();
            return result;
        }

        public async Task<List<MyRecipe>> GetAllRecipesByOriginId(int originId)
        {
            var result = await connection.GetAllWithChildrenAsync<MyRecipe>(_ => _.OriginId == originId, recursive: true);
            if (result.Count == 0) return null;
            return result.ToList();
        }

        // Procedure Service
        public async Task<int> AddProcedureAsync(Instruction instruction)
        {
            if (instruction is null)
                return (int)System.Net.HttpStatusCode.BadRequest;

            int result = await connection.InsertAsync(instruction);
            return result;
        }

        public async Task<int> UpdateProcedureAsync(Instruction instruction)
        {
            var result = await connection.UpdateAsync(instruction);
            return result;
        }

        public async Task<int> DeleteProcedureAsync(Instruction instruction)
        {
            var result = await connection?.DeleteAsync(instruction);
            return result;
        }

        public async Task<List<Instruction>> GetProceduresAsync()
        {
            var result = await connection.GetAllWithChildrenAsync<Instruction>(recursive: true);
            if (result.Count == 0) return null;
            return result.ToList();
        }

        public async Task<Instruction> GetProcedureAsync(int id)
        {
            var result = await connection.Table<Instruction>().Where(_ => _.Id == id).FirstOrDefaultAsync();
            return result;
        }

        public async Task<List<Instruction>> GetAllProceduresByRecipeId(int recipeId)
        {
            var result = await connection.GetAllWithChildrenAsync<Instruction>(_ => _.RecipeId == recipeId, recursive: true);
            if (result.Count == 0) return null;
            return result.ToList();
        }
    }
}