function printBooking(queueNumber, queueType, bookingDate, appointmentDate, phoneNumber, Name, lastname) {
    const printContent = `
        <div style="margin: 0;">
            <h1 style="text-align: center;">ข้อมูลการจองคิว</h1>
            <br />
            <h3>ชื่อผู้จอง: ${Name} ${lastname}</h3>
            <h3>หมายเลขคิว: ${queueNumber}</h3>
            <h3>ประเภทคิว: ${queueType}</h3>
            <h3>วันที่จอง: ${new Date(bookingDate).toLocaleDateString()}</h3>
            <h3>วันที่นัดหมาย: ${new Date(appointmentDate).toLocaleDateString()}</h3>
            <h3>เวลา: ${new Date(appointmentDate).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}</h3>
            <h3>หมายเลขโทรศัพท์: ${phoneNumber}</h3>
            <br />
            <h4>*ขอความกรุณามาก่อนเวลานัด 30 นาที*</h4>
            <h4>*หากไม่มาติดต่อภายในเวลาที่กำหนด ถือว่าท่านสละสิทธิ์ และต้องทำการจองคิวใหม่*</h4>
        </div>
    `;

    const printWindow = window.open('', '', 'height=1122,width=794'); // A4 height and width in pixels
    printWindow.document.write(`
        <html>
        <head>
            <title>ปริ้นการจองคิว</title>
            <style>
                @media print {
                    @page { size: A4; margin: 20mm; }
                    body { font-family: sans-serif; font-size: 14px; }
                    h2 { margin-bottom: 20px; }
                    p { margin: 0 0 10px 0; }
                    div { max-width: 21cm; margin: auto; }
                }
            </style>
        </head>
        <body>
            ${printContent}
        </body>
        </html>
    `);
    printWindow.document.close();
    printWindow.print();
}