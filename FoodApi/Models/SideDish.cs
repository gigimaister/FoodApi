using System.ComponentModel.DataAnnotations.Schema;

namespace FoodApi.Models
{
    [Table("SideDishes")]
    public class SideDish
    {
        public int Id { get; set; }
        public int MainDishId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
    }
}
