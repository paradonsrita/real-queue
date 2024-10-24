function initializeCalendar(objRef) {
    var today = new Date();
    const futureDate = new Date();
    
    futureDate.setDate(today.getDate() + 30);

    var calendarEl = document.getElementById('calendar');

    if (!calendarEl) {
        console.error("Calendar element not found!");
        return;
    }

    console.log("Creating FullCalendar...");

    var calendar = new FullCalendar.Calendar(calendarEl, {
        initialView: 'dayGridMonth',
        selectable: true,
        locale: 'th',  // กำหนดให้ใช้ภาษาไทย
        // ใช้ฟังก์ชันเพื่อตั้งค่าให้แสดงปีเป็น พ.ศ.
        titleFormat: { year: 'numeric', month: 'long' }, // รูปแบบของชื่อที่แสดงในหัวปฏิทิน

        // แก้ไขฟังก์ชันเพื่อเปลี่ยนสีของวันก่อนหน้าและวันที่ปัจจุบัน
        dayCellDidMount: function (info) {
            var cellDate = info.date;

            // เปรียบเทียบวันที่ปัจจุบันกับเซลล์ปัจจุบัน
            if (cellDate.toDateString() === today.toDateString()) {
                // เปลี่ยนสีพื้นหลังของวันที่ปัจจุบันเป็นสีเหลืองอ่อน
                info.el.style.backgroundColor = 'lightyellow';
            }
            else if (cellDate < today) {
                // เปลี่ยนสีพื้นหลังของวันที่ก่อนหน้าวันปัจจุบันเป็นสีเทา
                info.el.style.backgroundColor = 'lightgray';
            }
        },

        dateClick: function (info) {
            console.log("Date clicked:", info.dateStr);
/*
            var adYear = info.date.getFullYear(); // ปี ค.ศ.
            var buddhistYear = adYear;

            if (adYear < 2566) {
                buddhistYear = adYear + 543; // แปลงเป็นปี พ.ศ.
            }
            
            // สร้างสตริงวันที่ในรูปแบบที่ต้องการ
            var day = info.date.getDate();
            var month = info.date.getMonth() + 1; // เดือนเริ่มจาก 0

            var formattedDate = `${day}/${month}/${buddhistYear}`;
            */
            // ตรวจสอบว่ามีการจองเต็มแล้วหรือไม่
            objRef.invokeMethodAsync('CheckBookingAvailability', info.dateStr)
                .then((result) => {
                    if (result) {
                        if (info.date > futureDate) {
                            alert(`ต้องเลือกภายใน 30 วันต่อจากเวลาปัจจุบัน`)
                        }
                        else if (info.date >= today && isWeekday(info.date)) {
                            
                            objRef.invokeMethodAsync('ShowPopup', info.dateStr)
                                .then(() => {
                                    console.log("Popup should be visible now.");
                                })
                                .catch((error) => {
                                    console.error("Error calling ShowPopup:", error);
                                });
                        }
                        else {
                            alert("ไม่สามารถเลือกวันที่ผ่านมาแล้วหรือวันเสาร์อาทิตย์ได้")
                        }
                    } else {
                        console.log("Date is fully booked:", info.dateStr);
                    }
                })
                .catch((error) => {
                    console.error("Error checking booking availability:", error);
                });
        }
    });

    calendar.render();
    console.log("render complete");
}

// ฟังก์ชันเพื่อเช็คว่าวันที่เป็นวันจันทร์ถึงวันศุกร์หรือไม่
function isWeekday(date) {
    var day = date.getDay(); // 0 = Sunday, 1 = Monday, ..., 6 = Saturday
    return day >= 1 && day <= 5; // คืนค่า true หากวันคือจันทร์ถึงศุกร์
}


/*
function markFullDays(fullDays) {
    var calendarEl = document.getElementById('calendar');
    var calendar = new FullCalendar.Calendar(calendarEl);

    fullDays.forEach(function (day) {
        var date = new Date(day);
        calendar.addEvent({
            title: 'เต็มแล้ว',
            start: date,
            backgroundColor: 'red',
            borderColor: 'red',
            textColor: 'white',
        });
    });

    calendar.render();
}
*/