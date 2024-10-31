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
        public IActionResult MonitorGetQueue()
        {
            try
            {
                var bookingStatistics = _db.BookingQueues
                    .Include(q => q.QueueType)
                    .Include(q => q.QueueStatus) // Include QueueStatus if needed

                    .Where(q => q.appointment_date.Date == DateTime.Today && q.call_queue_time != null)

                    .Select(q => new
                    {
                        QueueType = q.QueueType.type_name,
                        QueueNumber = q.queue_type_id.ToUpper() + q.queue_number.ToString("000"),
                        q.counter,
                        CallQueueTime = q.call_queue_time,
                        Source = "booking"
                    })
                    .OrderByDescending(q => q.CallQueueTime)
                    .Take(3)
                    .ToList();

                var counterStatistics = _db.CounterQueues
                    .Include(q => q.QueueType)
                    .Include(q => q.QueueStatus) // Include QueueStatus if needed
                    .Where(q => q.queue_date.Date == DateTime.Today && q.call_queue_time != null)
                    .Select(q => new
                    {
                        QueueType = q.QueueType.type_name,
                        QueueNumber = q.queue_type_id.ToUpper() + q.queue_number.ToString("000"),
                        q.counter,
                        CallQueueTime = q.call_queue_time,
                        Source = "counter"
                    })
                    .OrderByDescending(q => q.CallQueueTime)
                    .Take(3)
                    .ToList();

                var totalStatistics = bookingStatistics
                    .Concat(counterStatistics)
                    .OrderBy(q => q.Source)
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
