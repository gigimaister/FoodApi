using System.ComponentModel.DataAnnotations.Schema;

namespace FoodApi.Models
{
    [Table("MainCourseToProduct")]
    public class MainCourseToProduct
    {

        public int Id { get; set; }
        public int ProductId { get; set; }
        public double Price { get; set; }
        public int MainCourseProductDishesId { get; set; }       
        public virtual MainCourseProductDishes MainCourseProductDishes { get; set; }

    }
}
