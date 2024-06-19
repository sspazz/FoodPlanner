namespace FoodPlanner.Models
{
    public class MealPlanRecipe
    {
        public int MealPlanId { get; set; }
        public int RecipeId { get; set; }
        public string RecipeName { get; set; }
        public string RecipeDescription { get; set; }
    }
}
