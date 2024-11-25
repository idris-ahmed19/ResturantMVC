using Microsoft.AspNetCore.Mvc;
using ResturantWebsite.Models;
using System.Text.Json;

namespace ResturantWebsite.Controllers
{
    public class BookingController : Controller
    {
        private readonly HttpClient _httpClient;

        public BookingController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7075/");
        }

       

       

        [HttpGet]
        public IActionResult Index() 

        {
            return View();
        
        }

        [HttpPost]
        public async Task<IActionResult> Index(CustomerRequest customerRequest, Booking model)

        {
            var availabilityResponse = await _httpClient.GetAsync($"api/Booking/available/{model.FK_TableId}/{model.BookingDate}");
            if(!availabilityResponse.IsSuccessStatusCode)
            {
                ViewBag.Message = "Error checking table availability";
                return View();
            }

                var availabilityContent = await availabilityResponse.Content.ReadAsStringAsync();

                var availabilityJson = JsonDocument.Parse(availabilityContent);
                var root = availabilityJson.RootElement;

            bool isAvailable = root.GetProperty("isAvailable").GetBoolean();

                
            if(!isAvailable)
            {
                ViewBag.Message = "The selected table is already booked for this date.";
                return View();
            }

            var customerResponse = await _httpClient.PostAsJsonAsync("api/Customer/AddCustomer", customerRequest);
            if(!customerResponse.IsSuccessStatusCode)
            {
                ViewBag.Message = "Error creating customer";
                return View();
            }

            var customerContent = await customerResponse.Content.ReadAsStringAsync();
            
            Console.WriteLine(customerContent);
            var customer = JsonSerializer.Deserialize<CustomerRequest>(customerContent);
            Console.WriteLine(customer);
           
            


            if (customer == null || customer.customerId == 0)
            {
                ViewBag.Message = "Error retrieving customer information.";
                return View();
            }

            model.FK_CustomerId = customer.customerId;
            Console.WriteLine(model.BookingId);
            Console.WriteLine(model.FK_TableId);
            Console.WriteLine(model.FK_CustomerId);
            Console.WriteLine(model.CustomerAmount);
            Console.WriteLine(model.BookingDate);

            var bookingResponse = await _httpClient.PostAsJsonAsync("api/Booking/AddBooking", model);
            Console.WriteLine(model);
            Console.WriteLine(bookingResponse);
            if (bookingResponse.IsSuccessStatusCode)
            {
                ViewBag.Message = "Your booking has been successfully placed";
            }
            else
            {
                ViewBag.Message = "There was an error placing your booking. Please try again.";
            }
            return View();

        }
    }
}
