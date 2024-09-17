﻿using QMS.Services;
using QMS.Services.LocalStorage;

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

       
        public class QueueData
        {
            public int queue_id { get; set; }
            public DateTime queue_date { get; set; }
            public string QueueStatus { get; set; }
            public string QueueType { get; set; }
            public string QueueNumber { get; set; }
            public int counter { get; set; }
        }
    }
}