using FoodApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace FoodApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SideDishesController : ControllerBase
    {
        private FoodDbContext _dbContext;
        public SideDishesController(FoodDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        // GET: api/SideDishes
        [HttpGet]
        // Main Dishes
        public IActionResult Get()
        {
            var mainDishes = from c in _dbContext.MainDishess
                             select new
                             {
                                 Id = c.Id,
                                 MainDishName = c.MainDishName                                
                             };

            return Ok(mainDishes);
        }

        // GET: api/SideDishes/SideDishesByMainDishId/1
        [HttpGet("[action]/{mainDishId}")]
        // Get Side Dishes
        public IActionResult SideDishesByMainDishId(int mainDishId)
        {
            var sideDishes = _dbContext.SideDishes.Where(sideDish => sideDish.MainDishId == mainDishId);
            return Ok(sideDishes);         
        }

    }
}
