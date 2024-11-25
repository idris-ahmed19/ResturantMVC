using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResturantWebsite.Models;

namespace ResturantWebsite.Controllers
{
	[Authorize]
	public class AdminController : Controller
	{
		private readonly HttpClient _httpClient;

		public AdminController()
		{
			_httpClient = new HttpClient();
			_httpClient.BaseAddress = new Uri("https://localhost:7075/");
		}


		public IActionResult Index()
		{
			return View();
		}

		public async Task<IActionResult> ManageMenu()
		{
			var response = await _httpClient.GetAsync("api/Menu/GetAllMenus");

			if (response.IsSuccessStatusCode)
			{
				var menu = await response.Content.ReadFromJsonAsync<List<Menu>>();
				return View(menu);
			}
			return View(new List<Menu>());

		}



		public IActionResult CreateMenu()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> CreateMenu(Menu model)
		{
			if (ModelState.IsValid)
			{
				var response = await _httpClient.PostAsJsonAsync("api/Menu/AddMenu", model);
				if (response.IsSuccessStatusCode)
				{
					return RedirectToAction(nameof(ManageMenu));
				}
			}
			return View(model);
		}

		
		public async Task<IActionResult> EditMenu(int id)
		{
			var response = await _httpClient.GetAsync($"api/Menu/GetMenuById/{id}");

			if (response.IsSuccessStatusCode)
			{
				var menu = await response.Content.ReadFromJsonAsync<Menu>();
				return View(menu);
			}

			return NotFound();
		}

		[HttpPost]
		public async Task<IActionResult> EditMenu(Menu model)
		{
			if (ModelState.IsValid)
			{
				var response = await _httpClient.PutAsJsonAsync($"api/Menu/UpdateMenu/{model.MenuId}", model);
				if (response.IsSuccessStatusCode)
				{
					return RedirectToAction(nameof(ManageMenu));
				}
			}
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> DeleteMenu(int id)
		{
			var response = await _httpClient.DeleteAsync($"api/Menu/DeleteMenu/{id}");
			return response.IsSuccessStatusCode ? RedirectToAction(nameof(ManageMenu)) : NotFound();
		}

		public async Task<IActionResult> ManageBookings()
		{
			var response = await _httpClient.GetAsync("api/Booking/GetAllBookings");
			if (response.IsSuccessStatusCode)
			{
				var bookings = await response.Content.ReadFromJsonAsync<List<Booking>>();
				return View(bookings);
			}
			return View(new List<Booking>());
		}

		public IActionResult CreateBooking()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> CreateBooking(Booking model)
		{
			if (ModelState.IsValid)
			{
				var response = await _httpClient.PostAsJsonAsync("api/Booking/AddBooking", model);
				if (response.IsSuccessStatusCode)
				{
					return RedirectToAction(nameof(ManageBookings));
				}
			}
			return View(model);
		}

		public async Task<IActionResult> EditBooking(int id)
		{
			var response = await _httpClient.GetAsync($"api/Booking/GetBookingById/{id}");
			if (response.IsSuccessStatusCode)
			{
				var bookings = await response.Content.ReadFromJsonAsync<Booking>();
				return View(bookings);
			}
			return NotFound();
		}

		[HttpPost]
		public async Task<IActionResult> EditBooking(Booking model)
		{
			if (ModelState.IsValid)
			{
				var response = await _httpClient.PutAsJsonAsync($"api/Booking/UpdateBooking/{model.BookingId}", model);
				if (response.IsSuccessStatusCode)
				{
					return RedirectToAction(nameof (ManageBookings));
				}
			}
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> DeleteBooking(int id)
		{
			var response = await _httpClient.DeleteAsync($"api/Booking/DeleteBooking/{id}");
			return response.IsSuccessStatusCode ? RedirectToAction(nameof (ManageBookings)) : NotFound();
		}

	}
}
