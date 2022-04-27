using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FoodApi.Models
{
    [Table("SideDishes")]
    public class SideDish
    {
        public int Id { get; set; }
        public int MainDishId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string ImageUrl { get; set; }

        [JsonIgnore]
        public bool IsAlsoPayingSideDise { get; set; }
    }
}
