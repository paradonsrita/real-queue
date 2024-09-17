<script>
    window.printPdf = function (pdfData) {
        var iframe = document.createElement("iframe");
    iframe.style.width = "210mm";
    iframe.style.height = "297mm";
    iframe.style.display = "none";
    document.body.appendChild(iframe);

    var doc = iframe.contentWindow.document;
    doc.open();
    doc.write(pdfData);
    doc.close();
    console.log(doc);

    return iframe;
    };
</script>

