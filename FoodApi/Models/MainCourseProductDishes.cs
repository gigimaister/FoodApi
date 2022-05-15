using System.ComponentModel.DataAnnotations.Schema;

namespace FoodApi.Models
{
    public class MainCourseProductDishes
    {
        public int Id { get; set; }
        public int MainDishesId { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }

        [ForeignKey("MainDishesId")]
        public virtual MainDishes MainDishes { get; set; }
    }
}
