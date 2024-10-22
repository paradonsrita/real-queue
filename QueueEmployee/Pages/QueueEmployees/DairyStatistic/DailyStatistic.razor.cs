using System.Globalization;
using Microsoft.AspNetCore.Http.HttpResults;
using QMS.Models;
using QMS.Pages.QueueEmployees.BookingStatistic;

namespace QMS.Pages.QueueEmployees.DairyStatistic
{
    public partial class DailyStatistic
    {

        private IEnumerable<Statistic> counter_queues;
        private IEnumerable<Statistic> booking_queues;
        private bool loading = true;
        private string error;
        bool showDataLabels = false;



        protected override async Task OnAfterRenderAsync(bool firstRender)
        {

            if (firstRender)
            {



                try
                {

                    var client = HttpClientFactory.CreateClient("Queue");
                    var response = await client.GetFromJsonAsync<List<QueueData>>("/api/Statistics/daily-statistics");

                    if (response == null)
                    {
                        throw new Exception("No response from api");
                    }

                    counter_queues = response
                        .Where(q => q.Source == "counter")
                        .GroupBy(q => q.QueueType)
                        .Select(g => new Statistic
                        {
                            type_name = g.Key,
                            Total = g.Count()
                        })
                        .ToList();

                    booking_queues = response
                        .Where(q => q.Source == "booking")
                        .GroupBy(q => q.QueueType)
                        .Select(g => new Statistic
                        {
                            type_name = g.Key,
                            Total = g.Count()
                        })
                        .ToList();
                    

                }
                catch (Exception ex)
                {
                    error = ex.Message;
                }
                finally
                {
                    loading = false;
                    StateHasChanged(); // เรียกใช้เพื่อบอก Blazor ว่าต้องการ re-render เนื่องจากค่าที่เปลี่ยนแปลง
                }
            }
        }

    }
}