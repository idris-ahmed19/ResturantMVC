using Microsoft.AspNetCore.Mvc;
using ResturantWebsite.Models;
using System.Net.Http.Json;

namespace ResturantWebsite.Controllers
{
    public class RestaurantController : Controller
    {
        private readonly HttpClient _httpClient;

        public RestaurantController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7075/");
        }

       

        public async Task<IActionResult> Index()

        {

            var response = await _httpClient.GetAsync("api/Menu/GetAllMenus");

            if (response.IsSuccessStatusCode)
            {
                var menuList = await response.Content.ReadFromJsonAsync<List<Menu>>();
                return View(menuList);
            }

            return View(new List<Menu>());


        }
    }
}
