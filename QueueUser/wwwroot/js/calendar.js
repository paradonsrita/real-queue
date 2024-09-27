function initializeCalendar(objRef) {
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
        
        dateClick: function (info) {
            console.log("Date clicked:", info.dateStr);

            if (info.date >= today && isWeekday(info.date)) { // ตรวจสอบว่าวันที่เป็นวันจันทร์ถึงวันศุกร์

                objRef.invokeMethodAsync('ShowPopup', info.dateStr)
                    .then(() => {
                        console.log("Popup should be visible now.");
                    })
                    .catch((error) => {
                        console.error("Error calling ShowPopup:", error);
                    });
            }
            
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
