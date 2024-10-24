using Microsoft.AspNetCore.SignalR;

namespace ApiIsocare2.Utilities.SignalR
{
    public class NotificationHub : Hub
    {
        // ฟังก์ชันนี้จะถูกเรียกจากฝั่งไคลเอนต์
        public async Task SendMessage(string message)
        {
            // ส่งข้อความไปยังไคลเอนต์ทั้งหมดที่เชื่อมต่อกับ Hub นี้
            await Clients.All.SendAsync("RefreshPage");
        }
    }
}
