﻿using QMS.Services.LocalStorage;
using QMS.Models;

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
                var savedCounter = await LocalStorageService.GetItemAsync("selectedCounter");
                var savedTransaction = await LocalStorageService.GetItemAsync("selectedTransaction");

                if (!string.IsNullOrEmpty(savedCounter))
                {
                    selectedCounter = int.Parse(savedCounter);
                }

                if (!string.IsNullOrEmpty(savedTransaction))
                {
                    selectedTransaction = savedTransaction;
                }

                // ดึงข้อมูลจาก API
                queues = await Http.GetFromJsonAsync<IEnumerable<QueueData>>("https://localhost:44328/api/Booking");
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