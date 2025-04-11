function GenerarExcel(filename, bytesBase64) {
    var link = document.createElement('a');
    link.download = filename;
    link.href = "data:application/octet-stream;base64," + bytesBase64;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}

function saveAsFile(filename, bytesBase64) {
    if (navigator.msSaveBlob) {
        var data = window.atob(bytesBase64);
        var bytes = new Unit8Array(data.length);
        for (var i = 0; i < data.length; i++) {
            bytes[i] = data.charCodeAt(i);
        }
        var blob = new Blob([bytes.buffer], { type: "application/octet-stream" });
        navigator.msSaveBlob(blob, filename);
    } else {
        var link = document.createElement("a");
        link.download = filename;
        link.href = "data:application/octet-stream;base64," + bytesBase64;
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);

    }
}

window.GenerarPDF = async (Nombre, ReposicionesPendientes) => {

    window.jsPDF = window.jspdf.jsPDF;
    window.autotable = window.jspdf.jspdf - autotable;
    var doc = new jsPDF({
        orientation: 'portrait',
        unit: 'mm',
        format: 'letter'
    });


    doc.setFontSize(12);
    doc.setFont("helvetica", "bold")
    doc.text("FULLBIKE SPORT", 80, 8);


    doc.save(`${Nombre}.pdf`);
};




