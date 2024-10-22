using QMS.Services.LocalStorage;
using QMS.Models;
using System.Net.Http;


namespace QMS.Pages.QueueEmployees.HomeBookingQueue
{
    public partial class ListQueueOnHome
    {

        private IEnumerable<QueueData> queues;
        private bool loading = true;
        private string error;
        private int? selectedCounter;
        private string selectedTransaction;

        
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                Console.WriteLine("Loaded");
                await LoadDataAsync();
            }
        }
        private async Task LoadDataAsync()
        {
            try
            {
                var savedCounter = await SessionStorageService.GetItemAsync("selectedCounter");
                var savedTransaction = await SessionStorageService.GetItemAsync("selectedTransaction");

                if (!string.IsNullOrEmpty(savedCounter))
                {
                    selectedCounter = int.Parse(savedCounter);
                }

                if (!string.IsNullOrEmpty(savedTransaction))
                {
                    selectedTransaction = savedTransaction;
                }

                var client = HttpClientFactory.CreateClient("Queue");
                // ดึงข้อมูลจาก API
                queues = await client.GetFromJsonAsync<IEnumerable<QueueData>>("/api/Booking");
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

        private string TransformQueueType(string queueType)
        {
            return queueType switch
            {
                "Finance" => "เปิด-ปิดบัญชีฝากถอน",
                "Loan" => "ขอกู้ รับชำระ จ่ายเงินกู้",
                "Shares" => "สมัครสมาชิก ลาออก ซื้อ-ถอนหุ้น",
                "Other" => "อื่นๆ",
                _ => queueType
            };
        }

    }
}