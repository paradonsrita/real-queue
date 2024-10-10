function printBooking(queueNumber, queueType, bookingDate, appointmentDate, phoneNumber) {
    const printContent = `
        <div style="margin: 0;">
            <h2>ข้อมูลการจองคิว</h2>
            <p>หมายเลขคิว: ${queueNumber}</p>
            <p>ประเภทคิว: ${queueType}</p>
            <p>วันที่จอง: ${new Date(bookingDate).toLocaleDateString()}</p>
            <p>วันที่นัดหมาย: ${new Date(appointmentDate).toLocaleDateString()}</p>
            <p>หมายเลขโทรศัพท์: ${phoneNumber}</p>
        </div>
    `;

    const printWindow = window.open('', '', 'height=600,width=800');
    printWindow.document.write('<html><head><title>ปริ้นการจองคิว</title></head><body>');
    printWindow.document.write(printContent);
    printWindow.document.write('</body></html>');
    printWindow.document.close();
    printWindow.print();
}