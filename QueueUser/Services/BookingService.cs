using QMS.Models;
using QMS.Pages.Booking;

namespace QMS.Services
{
    public class BookingService
    {
        private readonly HttpClient _httpClient;

        public BookingService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CalendarBooking>> GetQueueOnDate(string transaction)
        {
            var response = await _httpClient.GetAsync($"https://localhost:44328/api/Booking/calendar/{transaction}");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<List<CalendarBooking>>();
        }

        public async Task BookSlot(AddBookingRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("https://localhost:44328/api/Booking/add-queue", request);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error occurred while booking the slot.");
            }
        }
    }

}
