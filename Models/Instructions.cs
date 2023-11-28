using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipe.Models
{
    [Table("Instructions")]
    public class Instruction
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int Number { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string TimeNeeded { get; set; }
        [ManyToOne(CascadeOperations = CascadeOperation.All)]
        public MyRecipe MyRecipe { get; set; }
        [ForeignKey(typeof(MyRecipe))]
        public int RecipeId { get; set; }
    }
}