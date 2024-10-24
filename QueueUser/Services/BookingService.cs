using System.Globalization;
using System.Net.Http;
using System.Text.Json;
using QMS.Models;
using QMS.Pages.Booking;

namespace QMS.Services
{
    public class BookingService
    {
        private readonly HttpClient _httpClient;

        public BookingService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("Queue");
        }

        public async Task<List<CalendarBooking>> GetQueueOnDate(string transaction)
        {
            var response = await _httpClient.GetAsync($"/api/Booking/calendar/{transaction}");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<List<CalendarBooking>>();
        }

        public async Task<HttpResponseMessage> BookSlot(AddBookingRequest request)
        {
            // แสดงค่าของ request เป็น JSON
            var requestJson = JsonSerializer.Serialize(request);
            Console.WriteLine($"on BookSlot {requestJson}");
            var response = await _httpClient.PostAsJsonAsync("/api/Booking/add-queue", request);
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine(content);

            return response; // ส่งคืน HttpResponseMessage เพื่อให้ตรวจสอบสถานะ

        }
    }

}
