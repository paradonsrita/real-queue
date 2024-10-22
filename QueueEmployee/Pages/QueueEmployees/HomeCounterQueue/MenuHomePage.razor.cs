using System.Net.Http;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using QMS.Models;
using QMS.Services.LocalStorage;





namespace QMS.Pages.QueueEmployees.HomeCounterQueue
{
    public partial class MenuHomePage
    {
        private bool loading = true;
        private string error;
        private int? selectedCounter;
        private string selectedTransaction;


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await OnInitializedAsync();
            }
        }
        protected override async Task OnInitializedAsync()
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
                await JSRuntime.InvokeVoidAsync("console.log", $"{selectedTransaction} {selectedCounter}");

            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            finally
            {
                loading = false;
                StateHasChanged();
            }
            
        }

        private async Task callNextQueue()
        {
            Console.WriteLine("callNextQueue called");

            if (string.IsNullOrEmpty(selectedTransaction) || !selectedCounter.HasValue)
            {
                // จัดการกรณีที่ข้อมูลไม่ครบถ้วน
                Console.WriteLine("Transaction or Counter is not set.");
                return;
            }

            string transactionInitial = selectedTransaction switch
            {
                "Finance" => "F",
                "Other" => "H",
                "Loan" => "L",
                "Shares" => "S",
                _ => throw new InvalidOperationException("Invalid transaction type")
            };
            var client = HttpClientFactory.CreateClient("Queue");
            var requestUri = $"/api/Employee/callCounterQueue?transaction={Uri.EscapeDataString(transactionInitial)}&counter={selectedCounter}";

            try
            {
                var response = await client.PutAsync(requestUri, null);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Queue updated successfully!");
                }
                else
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error: {responseContent}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calling API: {ex.Message}");
            }
            Navigation.NavigateTo(Navigation.Uri, forceLoad: true);

        }
        private async Task skipQueue()
        {
            Console.WriteLine("skipQueue called");

            if (string.IsNullOrEmpty(selectedTransaction) || !selectedCounter.HasValue)
            {
                // จัดการกรณีที่ข้อมูลไม่ครบถ้วน
                Console.WriteLine("Transaction or Counter is not set.");
                return;
            }

            string transactionInitial = selectedTransaction switch
            {
                "Finance" => "F",
                "Other" => "H",
                "Loan" => "L",
                "Shares" => "S",
                _ => throw new InvalidOperationException("Invalid transaction type")
            };

            var client = HttpClientFactory.CreateClient("Queue");
            var requestUri = $"/api/Employee/skipCounterQueue?transaction={Uri.EscapeDataString(transactionInitial)}&counter={selectedCounter}";

            try
            {
                var response = await client.PutAsync(requestUri, null);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Queue updated successfully!");
                }
                else
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error: {responseContent}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calling API: {ex.Message}");
            }
            Navigation.NavigateTo(Navigation.Uri, forceLoad: true);

        }
    }
}