using Microsoft.AspNetCore.Components;
using QMS.Services.LocalStorage;

namespace QMS.Shared.Employee
{
    public partial class SetCounter
    {
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        bool multiple = false;
        private bool isLoading = false;
        private string bearerToken;

        private int? selectedCounter;
        private string selectedTransaction;
        private string selectedTransactionThai;

        private List<int> counter = new List<int> { 1, 2, 3, 4, 5, 6 };
        private List<string> transactionsInThai = new List<string> { "ขอกู้ รับชำระ จ่ายเงินกู้", "เปิด-ปิดบัญชีฝากถอน", "สมัครสมาชิก ลาออก ซื้อ-ถอนหุ้น" , "อื่นๆ"};

        // ภาษาอังกฤษที่เก็บใน LocalStorage
        private Dictionary<string, string> transactionMappings = new Dictionary<string, string>
    {
        { "ขอกู้ รับชำระ จ่ายเงินกู้", "Loan" },
        { "เปิด-ปิดบัญชีฝากถอน", "Finance" },
        { "สมัครสมาชิก ลาออก ซื้อ-ถอนหุ้น", "Shares" },
        { "อื่นๆ" , "Other"}
    };
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var savedCounter = await LocalStorageService.GetItemAsync("selectedCounter");
                var savedTransaction = await LocalStorageService.GetItemAsync("selectedTransaction");

                if (!string.IsNullOrEmpty(savedCounter))
                {
                    selectedCounter = int.Parse(savedCounter);
                    StateHasChanged(); // เรียกใช้เพื่อบอก Blazor ว่าต้องการ re-render เนื่องจากค่าที่เปลี่ยนแปลง
                }

                if (!string.IsNullOrEmpty(savedTransaction))
                {
                    selectedTransaction = savedTransaction;
                    selectedTransactionThai = TransformTransactionToThai(savedTransaction);
                    StateHasChanged(); // เรียกใช้เพื่อบอก Blazor ว่าต้องการ re-render เนื่องจากค่าที่เปลี่ยนแปลง
                }
            }
        }


        private async Task OnCounterChanged()
        {
            await LocalStorageService.SetItemAsync("selectedCounter", selectedCounter.ToString());
            Navigation.NavigateTo(Navigation.Uri, forceLoad: true);

        }

        private async Task OnTransactionChanged()
        {
            selectedTransaction = transactionMappings[selectedTransactionThai];
            await LocalStorageService.SetItemAsync("selectedTransaction", selectedTransaction);
            Navigation.NavigateTo(Navigation.Uri, forceLoad: true);

        }
        

        // ฟังก์ชันแปลงจาก key ภาษาอังกฤษเป็นภาษาไทย
        private string TransformTransactionToThai(string transactionKey)
        {
            return transactionMappings.FirstOrDefault(x => x.Value == transactionKey).Key ?? transactionKey;
        }
    }
}