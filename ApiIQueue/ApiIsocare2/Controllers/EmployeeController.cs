using ApiIsocare2.Data;
using ApiIsocare2.Models;
using ApiIsocare2.Utilities.SignalR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ApiIsocare2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {

        private readonly AppDbContext _db;
        private readonly IHubContext<NotificationHub> _hubContext;

        public EmployeeController(AppDbContext db, IHubContext<NotificationHub> hubContext)
        {
            _db = db;
            _hubContext = hubContext;

        }

        //  PUT: api/Employee/callCounterQueue
        [HttpPut("callCounterQueue")]
        public async Task<IActionResult> CallCounterQueue(string transaction, int counter)
        {
            try
            {
                var waitQueue = await _db.CounterQueues
                    .Where(q => q.queue_type_id == transaction && q.queue_status_id == 0 && q.queue_date.Date == DateTime.Today)
                    .OrderBy(q => q.queue_number)
                    .FirstOrDefaultAsync();
                var nowQueue = await _db.CounterQueues
                    .Where(q => q.counter == counter && q.queue_status_id == 2 && q.queue_date.Date == DateTime.Today)
                    .FirstOrDefaultAsync();

                if (nowQueue != null)
                {
                    nowQueue.queue_status_id = 1;
                }
                if (waitQueue != null)
                {
                    if (counter != 0)
                    {
                        waitQueue.queue_status_id = 2;
                        waitQueue.counter = counter;
                        waitQueue.call_queue_time = DateTime.Now;
                        
                    }
                    else
                    {
                        throw new Exception("เคาน์เตอร์ไม่สามารถเป็นเลข 0 หรือช่องว่างได้");
                    }
                }
                if (nowQueue != null || waitQueue != null)
                {
                    await _db.SaveChangesAsync();

                    await _hubContext.Clients.All.SendAsync("RefreshPage");

                    return NoContent();
                }
                throw new Exception("ไม่มีคิวทีรออยู่แล้ว");


            }
            catch (Exception ex)
            {
                var innerExceptionMessage = ex.InnerException?.Message ?? "ไม่มีข้อผิดพลาดเพิ่มเติม";
                return StatusCode(500, $"เกิดข้อผิดพลาด : {ex.Message}, ข้อผิดพลาดเพิ่มเติม : {innerExceptionMessage}");
            }
        }

        

        //  PUT: api/Employee/skipCounterQueue
        [HttpPut("skipCounterQueue")]
        public async Task<IActionResult> SkipCounterQueue(string transaction, int counter)
        {
            try
            {
                var waitQueue = await _db.CounterQueues
                .Where(q => q.queue_type_id == transaction && q.queue_status_id == 0 && q.queue_date.Date == DateTime.Today)
                .OrderBy(q => q.queue_number)
                .FirstOrDefaultAsync();
                var nowQueue = await _db.CounterQueues
                    .Where(q => q.counter == counter && q.queue_status_id == 2 && q.queue_date.Date == DateTime.Today)
                    .FirstOrDefaultAsync();

                if (nowQueue != null)
                {
                    nowQueue.queue_status_id = -9;
                    nowQueue.counter = 0;

                    if (waitQueue != null)
                    {
                        waitQueue.queue_status_id = 2; 
                        waitQueue.counter = counter;
                        waitQueue.call_queue_time = DateTime.Now;
                    }
                    await _db.SaveChangesAsync();

                    await _hubContext.Clients.All.SendAsync("RefreshPage");

                    return NoContent();
                }

                throw new Exception("ไม่มีคิวที่รออยู่แล้ว");
            }
            catch (Exception ex)
            {
                var innerExceptionMessage = ex.InnerException?.Message ?? "ไม่มีข้อผิดพลาดเพิ่มเติม";
                return StatusCode(500, $"เกิดข้อผิดพลาด : {ex.Message}, ข้อผิดพลาดเพิ่มเติม : {innerExceptionMessage}");
            }
        }



        //  PUT: api/Employee/callBookingQueue
        [HttpPut("callBookingQueue")]
        public async Task<IActionResult> CallBookingQueue(string transaction, int counter)
        {
            try
            {
                var waitQueue = await _db.BookingQueues
                    .Where(q => q.queue_type_id == transaction && q.queue_status_id == 0 && q.appointment_date.Date == DateTime.Now.Date)
                    .OrderBy(q => q.queue_number)
                    .FirstOrDefaultAsync();
                var nowQueue = await _db.BookingQueues
                    .Where(q => q.counter == counter && q.queue_status_id == 2 && q.appointment_date.Date == DateTime.Now.Date)
                    .FirstOrDefaultAsync();

                if (nowQueue != null)
                {
                    nowQueue.queue_status_id = 1;
                }
                if (waitQueue != null)
                {
                    if (counter != 0)
                    {
                        waitQueue.queue_status_id = 2;
                        waitQueue.counter = counter;
                        waitQueue.call_queue_time = DateTime.Now;

                        await _db.SaveChangesAsync();

                        await _hubContext.Clients.All.SendAsync("RefreshPage");

                    }
                    else
                    {
                        throw new Exception("เคาน์เตอร์ไม่สามารถเป็นเลข 0 หรือช่องว่างได้");
                    }
                }
                

                return NoContent();
            }
            catch (Exception ex)
            {
                var innerExceptionMessage = ex.InnerException?.Message ?? "ไม่มีข้อผิดพลาดเพิ่มเติม";
                return StatusCode(500, $"เกิดข้อผิดพลาด : {ex.Message}, ข้อผิดพลาดเพิ่มเติม : {innerExceptionMessage}");
            }
        }



        //  PUT: api/Employee/skipQueue
        [HttpPut("skipBookingQueue")]
        public async Task<IActionResult> SkipBookingQueue(string transaction, int counter)
        {
            try
            {
                var waitQueue = await _db.BookingQueues
                    .Where(q => q.queue_type_id == transaction && q.queue_status_id == 0 
                        && q.appointment_date.Date == DateTime.Now.Date)
                    .OrderBy(q => q.queue_number)
                    .FirstOrDefaultAsync();
                var nowQueue = await _db.BookingQueues
                    .Where(q => q.counter == counter && q.queue_status_id == 2 
                        && q.appointment_date.Date == DateTime.Now.Date)
                    .FirstOrDefaultAsync();

                if (nowQueue != null)
                {
                    nowQueue.queue_status_id = -9;
                    nowQueue.counter = 0;

                    if (waitQueue != null)
                    {
                        waitQueue.queue_status_id = 2;
                        waitQueue.counter = counter;
                        waitQueue.call_queue_time = DateTime.Now;

                    }
                    await _db.SaveChangesAsync();

                    await _hubContext.Clients.All.SendAsync("RefreshPage");

                    return NoContent();
                }

                throw new Exception("ไม่มีคิวที่รออยู่แล้ว");
            }
            catch (Exception ex)
            {
                var innerExceptionMessage = ex.InnerException?.Message ?? "ไม่มีข้อผิดพลาดเพิ่มเติม";
                return StatusCode(500, $"เกิดข้อผิดพลาด : {ex.Message}, ข้อผิดพลาดเพิ่มเติม : {innerExceptionMessage}");
            }
        }

    }
}
