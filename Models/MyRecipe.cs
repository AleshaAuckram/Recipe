using SQLite;
using SQLiteNetExtensions.Attributes;

namespace Recipe.Models
{
    [Table("MyRecipes")]
    public class MyRecipe
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Rank { get; set; }
        public string TimeNeeded { get; set; }
        public string Image { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; } = DateTime.Now;

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Instruction> Instructions { get; set; }

        [ManyToOne(CascadeOperations = CascadeOperation.All)]
        public Country Country { get; set; }

        [ForeignKey(typeof(Country))]
        public int OriginId { get; set; }
    }
}