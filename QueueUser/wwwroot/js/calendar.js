﻿function initializeCalendar(objRef) {
    var today = new Date();
    var calendarEl = document.getElementById('calendar');

    if (!calendarEl) {
        console.error("Calendar element not found!");
        return;
    }

    console.log("Creating FullCalendar...");

    var calendar = new FullCalendar.Calendar(calendarEl, {
        initialView: 'dayGridMonth',
        selectable: true,

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

            // ตรวจสอบว่ามีการจองเต็มแล้วหรือไม่
            objRef.invokeMethodAsync('CheckBookingAvailability', info.dateStr)
                .then((result) => {
                    if (result) {
                        if (info.date >= today && isWeekday(info.date)) {
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