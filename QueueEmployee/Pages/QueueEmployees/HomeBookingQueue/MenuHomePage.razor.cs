using System.Net.Http;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using QMS.Models;
using QMS.Services.LocalStorage;



namespace QMS.Pages.QueueEmployees.HomeBookingQueue
{
    public partial class MenuHomePage
    {
        private bool loading = true;
        private string message;
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
                message = ex.Message;
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
				message = "กรุณาเลือกช่องบริการและประเภทธุรกรรมให้ควรถ้วน";
				return;
            }

            string transactionInitial = selectedTransaction switch
            {
                "Finance" => "F",
                "Other" => "H",
                "Loan" => "L",
                "Shares" => "S",
                _ => throw new InvalidOperationException("ประเภทข้อมูลไม่ถูกต้อง")
            };
            var client = HttpClientFactory.CreateClient("Queue");
            var requestUri = $"/api/Employee/callBookingQueue?transaction={Uri.EscapeDataString(transactionInitial)}&counter={selectedCounter}";

            try
            {
                var response = await client.PutAsync(requestUri, null);
                if (response.IsSuccessStatusCode)
                {
					message = "เรียกคิวสำเร็จ";
				}
				else
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
					message = $"เกิดข้อผิดพลาด: {responseContent}";
				}
			}
            catch (Exception ex)
            {
				message = $"เกิดข้อผิดพลาดในการเรียก API: {ex.Message}";
			}
			Navigation.NavigateTo(Navigation.Uri, forceLoad: true);

        }

        private async Task RepeatQueue()
        {
            try
            {
                var client = HttpClientFactory.CreateClient("Queue");
                await client.GetAsync("/api/Employee/repeatQueue");
                // อาจต้องอัปเดต UI หรือทำการโหลดข้อมูลใหม่ที่นี่
            }
            catch (Exception ex)
            {
				message = $"เกิดข้อผิดพลาดในการเรียก API: {ex.Message}";
			}

		}
        private async Task skipQueue()
        {

            if (string.IsNullOrEmpty(selectedTransaction) || !selectedCounter.HasValue)
            {
				// จัดการกรณีที่ข้อมูลไม่ครบถ้วน
				message = "กรุณาเลือกช่องบริการและประเภทธุรกรรมให้ควรถ้วน";
				return;
            }

            string transactionInitial = selectedTransaction switch
            {
                "Finance" => "F",
                "Other" => "H",
                "Loan" => "L",
                "Shares" => "S",
                _ => throw new InvalidOperationException("ประเภทข้อมูลไม่ถูกต้อง")

			};

            var client = HttpClientFactory.CreateClient("Queue");
            var requestUri = $"/api/Employee/skipBookingQueue?transaction={Uri.EscapeDataString(transactionInitial)}&counter={selectedCounter}";

            try
            {

                var response = await client.PutAsync(requestUri, null);
                if (response.IsSuccessStatusCode)
                {
					message = "ข้ามคิวสำเร็จ";
				}
				else
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
					message = $"เกิดข้อผิดพลาด: {responseContent}";
				}
			}
            catch (Exception ex)
            {
				message = $"เกิดข้อผิดพลาดในการเรียก API: {ex.Message}";
			}
			Navigation.NavigateTo(Navigation.Uri, forceLoad: true);

        }
    }
}