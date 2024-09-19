using QMS.Services.LocalStorage;
using QMS.Models;


namespace QMS.Pages.QueueEmployees.HomeCounterQueue
{
    public partial class ListQueueOnHome
    {
        private IEnumerable<QueueData> queues;
        private bool loading = true;
        private string error;
        private string selectedTransaction;
        private int selectedCounter;

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
                var savedTransaction = await LocalStorageService.GetItemAsync("selectedTransaction");
                var savedCounter = await LocalStorageService.GetItemAsync("selectedCounter");


                if (!string.IsNullOrEmpty(savedTransaction))
                {
                    selectedTransaction = savedTransaction;
                }
                if (!string.IsNullOrEmpty(savedCounter))
                {
                    selectedCounter = int.Parse(savedCounter);
                }

                // ดึงข้อมูลจาก API
                queues = await Http.GetFromJsonAsync<IEnumerable<QueueData>>("https://localhost:44328/api/Queue");
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
                _ => queueType
            };
        }
    }
}