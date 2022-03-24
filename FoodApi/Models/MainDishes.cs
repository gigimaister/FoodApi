using System.ComponentModel.DataAnnotations.Schema;

namespace FoodApi.Models
{
    [Table("MainDishes")]
    public class MainDishes
    {
        public int Id { get; set; }
        public string MainDishName { get; set; }
    }
}
