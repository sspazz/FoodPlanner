namespace FoodPlanner.Models
{
    public class MealPlan
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public List<MealPlanRecipe> Recipes { get; set; }
    }
}
