// สร้าง PDF ด้วย iframe
window.createPdf = function (pdfData) {
    var iframe = document.createElement("iframe");
    iframe.style.width = "80mm";  // ปรับขนาดให้ตรงกับกระดาษของเครื่อง POS-80 (80mm กว้าง)
    iframe.style.height = "auto";  // ให้ความสูงปรับตามเนื้อหา
    iframe.style.display = "block";
    iframe.style.position = "absolute"; // ป้องกัน iframe จากการรบกวน layout ของหน้า
    iframe.style.top = "-9999px"; // นำ iframe ออกจากหน้าจอ
    document.body.appendChild(iframe);

    var doc = iframe.contentWindow.document;
    doc.open();
    doc.write(`<html><body style="margin: 0; padding: 0; font-size: 12px;">${pdfData}</body></html>`);
    doc.close();
    return iframe;
};

window.printPdf = function (pdfData) {
    var iframe = createPdf(pdfData);
    iframe.contentWindow.focus();

    // รอให้ iframe โหลดก่อนที่จะพิมพ์
    iframe.contentWindow.onload = function () {
        iframe.contentWindow.print();  // สั่งพิมพ์เนื้อหาของ PDF
        // ลบ iframe หลังจากพิมพ์เสร็จ
        setTimeout(() => {
            document.body.removeChild(iframe);
        }, 1000); // รอ 1 วินาทีก่อนที่จะลบ iframe
    };
};
