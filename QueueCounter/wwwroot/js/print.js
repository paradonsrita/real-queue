﻿// สร้าง PDF ด้วย iframe
window.createPdf = function (pdfData) {
    var iframe = document.createElement("iframe");
    iframe.style.width = "80mm";  // ปรับขนาดให้ตรงกับกระดาษของเครื่อง POS-80 (80mm กว้าง)
    iframe.style.height = "auto";  // ให้ความสูงปรับตามเนื้อหา
    iframe.style.display = "block";
    document.body.appendChild(iframe);

    var doc = iframe.contentWindow.document;
    doc.open();
    doc.write(`<html><body style="margin: 0; padding: 0;">${pdfData}</body></html>`);
    doc.close();
    return iframe;
};

window.printPdf = function (pdfData) {
    var iframe = createPdf(pdfData);
    iframe.contentWindow.focus();
    iframe.contentWindow.print();  // สั่งพิมพ์เนื้อหาของ PDF
};