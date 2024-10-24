using System;
using System.ComponentModel.DataAnnotations;
using ApiIsocare2.Data;
using ApiIsocare2.Models.Booking;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Globalization;

namespace ApiIsocare2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        
        private readonly AppDbContext _db;
        public BookingController(AppDbContext db)
        {
            _db = db;
        }

        // api/Booking
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookingQueue>>> GetBookingQueues()
        {
            try
            {
                var result = await (from q in _db.BookingQueues
                                    join qs in _db.QueueStatuses on q.queue_status_id equals qs.queue_status_id
                                    join qt in _db.QueueTypes on q.queue_type_id equals qt.queue_type_id
                                    join qu in _db.Users on q.user_id equals qu.user_id
                                    where q.appointment_date.Date == DateTime.Today && qs.queue_status_id != -9
                                    select new
                                    {
                                        q.queue_id,
                                        q.appointment_date,
                                        QueueStatus = qs.queue_status_name,
                                        QueueType = qt.type_name,
                                        QueueNumber = q.queue_type_id.ToUpper() + q.queue_number.ToString("000"),
                                        q.counter
                                    })
                               .ToListAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                var innerExceptionMessage = ex.InnerException?.Message ?? "ไม่มีข้อผิดพลาดเพิ่มเติม";
                return StatusCode(500, $"เกิดข้อผิดพลาด : {ex.Message}, ข้อผิดพลาดเพิ่มเติม : {innerExceptionMessage}");
            }

        }

        // api/Booking/profile/{userId}
        [HttpGet("profile/{userId}")]
        public async Task<ActionResult> getQueueInThisProfile(int userId)
        {
            try
            {
                // ตรวจสอบว่า userId มีอยู่ในฐานข้อมูลหรือไม่
                var userExists = await _db.Users.AnyAsync(u => u.user_id == userId);
                if (!userExists)
                {
                    return NotFound($"ไม่มีบัญชีผู้ใช้นี้");
                }
                var result = await (from q in _db.BookingQueues
                                    join qs in _db.QueueStatuses on q.queue_status_id equals qs.queue_status_id
                                    join qt in _db.QueueTypes on q.queue_type_id equals qt.queue_type_id
                                    join qu in _db.Users on q.user_id equals qu.user_id
                                    select new
                                    {
                                        q.queue_id,
                                        q.booking_date,
                                        q.appointment_date,
                                        QueueStatus = qs.queue_status_name,
                                        QueueType = qt.type_name,
                                        QueueNumber = q.queue_type_id.ToUpper() + q.queue_number.ToString("000"),
                                        q.counter,
                                        q.user_id,
                                        Name = qu.firstname,
                                        qu.lastname,
                                        qu.phone_number,
                                        qu.citizen_id_number
                                    })
                                .Where(q => q.user_id == userId)
                                .OrderBy(q => q.queue_id)
                                .ToListAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                var innerExceptionMessage = ex.InnerException?.Message ?? "ไม่มีข้อผิดพลาดเพิ่มเติม";
                return StatusCode(500, $"เกิดข้อผิดพลาด : {ex.Message}, ข้อผิดพลาดเพิ่มเติม : {innerExceptionMessage}");
            }

        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetBookingQueue(int id)
        {
            try
            {
                var result = await (from q in _db.BookingQueues
                                    join qs in _db.QueueStatuses on q.queue_status_id equals qs.queue_status_id
                                    join qt in _db.QueueTypes on q.queue_type_id equals qt.queue_type_id
                                    join qu in _db.Users on q.user_id equals qu.user_id
                                    select new
                                    {
                                        q.queue_id,
                                        q.booking_date,
                                        q.appointment_date,
                                        QueueStatus = qs.queue_status_name,
                                        QueueType = qt.type_name,
                                        QueueNumber = q.queue_type_id.ToUpper() + q.queue_number.ToString("000"),
                                        q.counter,
                                        q.user_id,
                                        Name = qu.firstname,
                                        qu.lastname,
                                        qu.phone_number,
                                        qu.citizen_id_number
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

        // GET: api/Booking/calendar/{transaction}
        [HttpGet("calendar/{transaction}")]
        public IActionResult GetQueueOnDate(string transaction)
        {
            try
            {
                var today = DateTime.Today;
                var maxDate = today.AddDays(30);

                // 1. สร้างรายการของทุกวันและเวลาที่เป็นไปได้
                var timesOfInterest = new[] { new TimeSpan(8, 0, 0), new TimeSpan(13, 0, 0) };
                var allDatesAndTimes = Enumerable.Range(0, (maxDate - today).Days + 1)
                    .SelectMany(d => timesOfInterest.Select(t => today.AddDays(d).Date.Add(t)))
                    .ToList();

                // 2. ดึงข้อมูลจากฐานข้อมูลและจัดกลุ่มตามวันที่และเวลา
                var bookingData = _db.BookingQueues
                    .Where(q => q.appointment_date <= maxDate && q.appointment_date > today && q.queue_type_id == transaction && q.queue_status_id == 0)
                    .GroupBy(q => new
                    {
                        Date = q.appointment_date.Date,
                        Time = q.appointment_date.TimeOfDay
                    })
                    .Select(g => new
                    {
                        g.Key.Date,
                        g.Key.Time,
                        Total = g.Count()
                    })
                    .ToList();

                // 3. รวมผลลัพธ์เข้ากับรายการของทุกวันและเวลาที่เป็นไปได้
                var result = allDatesAndTimes
                    .Select(d => new
                    {
                        Date = d.Date,
                        Time = d.TimeOfDay,
                        Total = bookingData.FirstOrDefault(b => b.Date == d.Date && b.Time == d.TimeOfDay)?.Total ?? 0
                    })
                    .ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                var innerExceptionMessage = ex.InnerException?.Message ?? "ไม่มีข้อผิดพลาดเพิ่มเติม";
                return StatusCode(500, $"เกิดข้อผิดพลาด : {ex.Message}, ข้อผิดพลาดเพิ่มเติม : {innerExceptionMessage}");
            }
        }

		//api/Booking/add-queue
		[HttpPost("add-queue")]
        public async Task<IActionResult> AddBooking([FromBody] AddBookingRequest request)
        {
            try
            {
                var convertAppointmentDateToAD = request.AppointmentDate;
                if (convertAppointmentDateToAD.Year < 2020)
                {
                    convertAppointmentDateToAD = convertAppointmentDateToAD.AddYears(543); // แปลงเป็น ค.ศ. โดยลบ 543 ปี
                }
                // แปลงเวลาจาก appointmentTime เป็น TimeSpan
                if (!TimeSpan.TryParse(request.AppointmentTime, out var timeOfDay))
                {
                    return BadRequest("ตัวแปรเวลาไม่ถูกต้อง");
                }

                // ตรวจสอบและปรับเวลา appointmentDate
                if (timeOfDay != new TimeSpan(8, 0, 0) && timeOfDay != new TimeSpan(13, 0, 0))
                {
                    return BadRequest("เวลาการนัดหมายต้องเป็น 08:00 กับ 13:00 เท่านั้น");
                }
                

                if (convertAppointmentDateToAD.Date <= DateTime.Now.Date)
                {
                    return BadRequest($"ไม่สามารถจองวันที่ที่ผ่านมาแล้วได้ ({convertAppointmentDateToAD.AddYears(543).ToString("dd/MM/yyyy")}) ({timeOfDay})");
                }

                // แก้ไขค่าเวลาให้เป็น 08:00 หรือ 13:00
                var correctedAppointmentDate = DateTime.SpecifyKind(convertAppointmentDateToAD.Date.Add(timeOfDay), DateTimeKind.Local);


                // ตรวจสอบว่า user_id ได้จองคิวในเวลานี้แล้วหรือยัง
                var existingQueue = await _db.BookingQueues
                    .Where(q => q.appointment_date.Date == correctedAppointmentDate.Date
                                && q.appointment_date.TimeOfDay == timeOfDay
                                && q.user_id == request.UserId
                                && q.queue_status_id != -9)
                    .FirstOrDefaultAsync();

                if (existingQueue != null)
                {
                    return BadRequest($"คุณได้ทำการจอง ณ วันที่ {request.AppointmentDate.ToString("dd/MM/yyyy")} เวลา {request.AppointmentTime} ไปแล้วในคิว {existingQueue.queue_type_id}{existingQueue.queue_number.ToString("000")}");
                }

                var queueCount = await _db.BookingQueues
                                .Where(q => q.appointment_date.Date == correctedAppointmentDate.Date
                                            && q.appointment_date.TimeOfDay == timeOfDay
                                            && q.queue_type_id == request.Type)
                                .CountAsync();

                if (queueCount >= 10)
                {
                    return BadRequest("ไม่สามารถจองคิว 10 คิวในช่วงเวลาเดียวกันได้");
                }

                var number = await _db.BookingQueues
                                .Where(q => q.appointment_date.Date == correctedAppointmentDate.Date && q.queue_type_id == request.Type)
                                .OrderByDescending(q => q.queue_number)
                                .Select(q => q.queue_number)
                                .FirstOrDefaultAsync();

                number = number == 0 ? 1 : number + 1;
                var queue = new BookingQueue
                {
                    queue_type_id = request.Type,
                    queue_number = number,
                    queue_status_id = 0,
                    user_id = request.UserId,
                    booking_date = DateTime.Now,
                    appointment_date = correctedAppointmentDate,
                    counter = 0,
                    call_queue_time = null
                };
                _db.BookingQueues.Add(queue);
                await _db.SaveChangesAsync();


                return CreatedAtAction(nameof(GetBookingQueue), new { id = queue.queue_id }, queue);

            }
            catch (Exception ex)
            {
                var innerExceptionMessage = ex.InnerException?.Message ?? "ไม่มีข้อผิดพลาดเพิ่มเติม";
                return StatusCode(500, $"เกิดข้อผิดพลาด : {ex.Message}, ข้อผิดพลาดเพิ่มเติม : {innerExceptionMessage}");
            }

        }

        [HttpPut("cancel")]
        public async Task<IActionResult> CancelBooking(int id)
        {
            try
            {
                var queue = await _db.BookingQueues.FindAsync(id);

                if (queue == null)
                {
                    return BadRequest("ไม่มีคิวนี้ในฐานข้อมูล");
                }
                
                queue.queue_status_id = -9;
                await _db.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                var innerExceptionMessage = ex.InnerException?.Message ?? "ไม่มีข้อผิดพลาดเพิ่มเติม";
                return StatusCode(500, $"เกิดข้อผิดพลาด : {ex.Message}, ข้อผิดพลาดเพิ่มเติม : {innerExceptionMessage}");
            }

        }





        
    }
}
