using ApiIsocare2.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiIsocare2.Models.Queue;

namespace ApiIsocare2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QueueController : ControllerBase
    {
        private readonly AppDbContext _db;
        public QueueController(AppDbContext db)
        {
            _db = db;
        }

        // GET: api/Queue
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CounterQueue>>> GetCounterQueues()
        {
            try
            {
                var result = await (from q in _db.CounterQueues
                                    join qs in _db.QueueStatuses on q.queue_status_id equals qs.queue_status_id
                                    join qt in _db.QueueTypes on q.queue_type_id equals qt.queue_type_id
                                    select new
                                    {
                                        q.queue_id,
                                        q.queue_date,
                                        QueueStatus = qs.queue_status_name,
                                        QueueType = qt.type_name,
                                        QueueNumber = qt.queue_type_id.ToUpper() + q.queue_number.ToString("000"),
                                        q.counter,
                                        q.call_queue_time

                                    })
                                .Where(q => q.queue_date.Date == DateTime.Today)
                                .ToListAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                var innerExceptionMessage = ex.InnerException?.Message ?? "ไม่มีข้อผิดพลาดเพิ่มเติม";
                return StatusCode(500, $"เกิดข้อผิดพลาด : {ex.Message}, ข้อผิดพลาดเพิ่มเติม : {innerExceptionMessage}");
            }
        }
            

        

        // GET: api/Queue/?id
        [HttpGet("{id}")]
        public async Task<ActionResult<CounterQueue>> GetQueue(int id)
        {
            try
            {
                var result = await (from q in _db.CounterQueues
                                    join qs in _db.QueueStatuses on q.queue_status_id equals qs.queue_status_id
                                    join qt in _db.QueueTypes on q.queue_type_id equals qt.queue_type_id
                                    select new
                                    {
                                        q.queue_id,
                                        q.queue_date,
                                        QueueStatus = qs.queue_status_name,
                                        QueueType = qt.type_name,
                                        QueueNumber = qt.queue_type_id.ToUpper() + q.queue_number.ToString("000"),
                                        q.counter,
                                        q.call_queue_time
                                    })
                           .Where(q => q.queue_id == id)
                           .FirstOrDefaultAsync();

                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);

            }
            catch (Exception ex)
            {
                var innerExceptionMessage = ex.InnerException?.Message ?? "ไม่มีข้อผิดพลาดเพิ่มเติม";
                return StatusCode(500, $"เกิดข้อผิดพลาด : {ex.Message}, ข้อผิดพลาดเพิ่มเติม : {innerExceptionMessage}");
            }


        }


        // POST: api/Queue
        [HttpPost]
        public async Task<IActionResult> PostAsync(string type)
        {
            try
            {
                int? count = await _db.CounterQueues
                                    .Where(q => q.queue_date.Date == DateTime.Today && q.queue_type_id == type)
                                    .OrderByDescending(q => q.queue_number)
                                    .Select(q => (int?)q.queue_number) // cast เป็น int?
                                    .FirstOrDefaultAsync();

                count = count == null || count == 0 ? 101 : count + 1;


                var queue = new CounterQueue
                {
                    queue_number = count.Value,
                    queue_date = DateTime.Now,
                    queue_type_id = type,
                    queue_status_id = 0,
                    counter = 0,
                    call_queue_time = null
                };
                
                _db.CounterQueues.Add(queue);
                await _db.SaveChangesAsync();

                return CreatedAtAction(nameof(GetQueue), new { id = queue.queue_id }, queue);

            }
            catch (Exception ex)
            {
                var innerExceptionMessage = ex.InnerException?.Message ?? "ไม่มีข้อผิดพลาดเพิ่มเติม";
                return StatusCode(500, $"เกิดข้อผิดพลาด : {ex.Message}, ข้อผิดพลาดเพิ่มเติม : {innerExceptionMessage}");
            }


        }
        
    }
}
