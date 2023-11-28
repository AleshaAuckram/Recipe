using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipe.Models
{
    [Table("Countrys")]
    public class Country
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<MyRecipe> MyRecipes { get; set; }
    }
}