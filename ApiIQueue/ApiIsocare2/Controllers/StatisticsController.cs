using ApiIsocare2.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiIsocare2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public StatisticsController(AppDbContext db) 
        {
            _db = db;
        }

        

        [HttpGet("statistics")]
        public IActionResult BookingStatistics([FromQuery] int year)
        {
            try
            {
                

                var bookingQueue = _db.BookingQueues
                    .Include(q => q.QueueType)
                    .Where(q => q.appointment_date.Year == year && q.queue_status_id != -9)
                    .GroupBy(q => new 
                    { 
                        q.QueueType.type_name,
                        Source = "booking",
                        Date = q.appointment_date.Date,
                        
                    })
                    .Select(group => new
                    {
                        group.Key.Date,
                        group.Key.Source,
                        group.Key.type_name,
                        Total = group.Count()
                    })
                    .ToList();

                var counterQueue = _db.CounterQueues
                    .Include(q => q.QueueType)
                    .Where(q => q.queue_date.Year == year && q.queue_status_id != -9)
                    .GroupBy(q => new
                    {
                        q.QueueType.type_name,
                        Source = "counter",
                        Date = q.queue_date.Date,
                    })
                    .Select(group => new
                    {
                        group.Key.Date,
                        group.Key.Source,
                        group.Key.type_name,
                        Total = group.Count()
                    })
                    .ToList();

                var totalQueue = bookingQueue
                    .Concat(counterQueue)
                    .OrderBy(q => q.Date)
                    .ThenBy(q => q.Source)
                    .ThenBy (q => q.type_name)
                    .ToList();

                return Ok(totalQueue);
            }
            catch (Exception ex)
            {
                var innerExceptionMessage = ex.InnerException?.Message ?? "ไม่มีข้อผิดพลาดเพิ่มเติม";
                return StatusCode(500, $"เกิดข้อผิดพลาด : {ex.Message}, ข้อผิดพลาดเพิ่มเติม : {innerExceptionMessage}");
            }

        }
    }
}


