namespace ResturantWebsite.Models
{
    public class Menu
    {
        public int MenuId { get; set; }

        public string DishName { get; set; }
        public string Description { get; set; }

        public int Price { get; set; }
        public bool Availability { get; set; }
    }
}
