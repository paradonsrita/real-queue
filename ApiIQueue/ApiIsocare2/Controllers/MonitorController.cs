using ApiIsocare2.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiIsocare2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonitorController : ControllerBase
    {
        private readonly AppDbContext _db;
        public MonitorController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet()]
        public IActionResult DailyStatistics()
        {
            try
            {
                var bookingStatistics = _db.BookingQueues
                    .Include(q => q.QueueType)
                    .Include(q => q.QueueStatus) // Include QueueStatus if needed

                    .Where(q => q.appointment_date.Date == DateTime.Today && q.queue_status_id == 2)

                    .Select(q => new
                    {
                        QueueType = q.QueueType.type_name,
                        QueueNumber = q.queue_type_id.ToUpper() + q.queue_number.ToString("000"),
                        q.counter,
                        CallQueueTime = q.call_queue_time,
                        Source = "booking"
                    })
                    .ToList();

                var counterStatistics = _db.CounterQueues
                    .Include(q => q.QueueType)
                    .Include(q => q.QueueStatus) // Include QueueStatus if needed
                    .Where(q => q.queue_date.Date == DateTime.Today && q.queue_status_id == 2)
                    .Select(q => new
                    {
                        QueueType = q.QueueType.type_name,
                        QueueNumber = q.queue_type_id.ToUpper() + q.queue_number.ToString("000"),
                        q.counter,
                        CallQueueTime = q.call_queue_time,
                        Source = "counter"
                    })
                    .ToList();

                var totalStatistics = bookingStatistics
                    .Concat(counterStatistics)
                    .OrderBy(q => q.Source)
                    .ThenBy(q => q.CallQueueTime)
                    .ToList();


                return Ok(totalStatistics);

            }
            catch (Exception ex)
            {
                var innerExceptionMessage = ex.InnerException?.Message ?? "ไม่มีข้อผิดพลาดเพิ่มเติม";
                return StatusCode(500, $"เกิดข้อผิดพลาด : {ex.Message}, ข้อผิดพลาดเพิ่มเติม : {innerExceptionMessage}");
            }

        }
    }
}
