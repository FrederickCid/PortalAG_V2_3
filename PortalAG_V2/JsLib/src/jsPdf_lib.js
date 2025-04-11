import jsPDF from "jspdf";
import QRious from "qrious";
import autoTable from "jspdf-autotable";

export async function generarPDF(
  nombreDelArchivo,
  datosCliente = [],
  Elements,
  cabecera,
  selectedDireccionFact = {}
) {
  var doc = new jsPDF({
    orientation: "portrait",
    unit: "mm",
    format: "letter",
  });
  const columns = [
    "Código",
    "Descripción",
    "Cant.",
    "P. Venta",
    "Sub Total",
    "Descuento",
    "Total",
  ];
  const rows = Elements?.map((e) => [
    e.codigo,
    e.nombre,
    e.cantidad,
    `$ ${e.precio}`,
    `$ ${e.cantidad * e.precio}`,
    `$ ${e.descuento * e.cantidad}`,
    `$ ${e.total}`,
  ]);
  const options = {
    head: [columns],
    body: rows,
    startY: 80,
    theme: "plain", // plain djiseño minimalista
    tableWidth: "auto",
    styles: {
      fillColor: [255, 255, 255],
      textColor: [0, 0, 0],
      lineColor: [0, 0, 0],
      cellWidth: "auto",
      fontSize: 8,
    }, // Colores de la tabla seleccionando el componente a editar
    //columnStyles:
    //    0: { cellWidth: 'auto' }, //para odernar el texto y no se sobre salga de los bordes
    //    1: { cellWidth: 'auto' }, //ajusta el tamaño de las columnas de forma automatica (*ver las demas opciones)
    //    6: { cellWidth: 'auto' }, //ajusta el tamaño de las columnas de forma automatica (*ver las demas opciones)
    //},
  };
  var date = new Date();
  var qrCodeData = `C${date.getFullYear().toString()}00${datosCliente[3]}001`;
  var qr = new QRious({
    value: qrCodeData,
    size: 128,
    level: "L",
  });
  // Convertir el código QR a una imagen en formato Data URL
  var qrCodeImage = qr.toDataURL("image/png");
  doc.addImage(qrCodeImage, "PNG", 10, 5, 25, 25);
  doc.setFontSize(12);
  doc.setFont("helvetica", "bold");
  doc.text("FULLBIKE SPORT", 80, 8);
  doc.setFontSize(7);
  doc.setFont("times", "normal");
  doc.text("COMERCIAL FULL BIKE CHILE S.A", 45, 11);
  doc.text(
    "COMERCIALIZACIÓN DE ARTICULOS DEPORTIVOS BICICLETAS Y RESPUESTOS",
    45,
    14
  );
  doc.text(
    "CASA MATRIZ: Av. Las Condes 13451 - Loc. 118, Mall Sport, Las Condes - Tel: 223710011",
    45,
    17
  );
  doc.text(
    "SUCURSALES:  Av. La Dehesa 1766, Lo Barnechea - Tel: 222495156",
    45,
    20
  );
  doc.text(
    "\t\tAv. Paseo Colina Sur 14.500 - Loc 125, Mall Piedra Roja - Tel: 229464721",
    45,
    23
  );
  doc.text("\t\tPadre Hurtado 1233, Vitacura - Tel: 223789811", 45, 26);
  doc.text("PAGINA WEB:  www.fullbike.cl", 45, 29);
  doc.setDrawColor(255, 0, 0);
  doc.setLineWidth(1);
  doc.rect(145, 4, 60, 30);
  doc.setFont("times", "bold");
  doc.setFontSize(15);
  doc.setTextColor(255, 0, 0);
  doc.text("R.U.T: 76.809.030-0", 153, 10);
  doc.text("Cotizacion", 163, 20);
  doc.text(`Nº${datosCliente[8]}`, 170, 30);
  doc.setDrawColor(128, 128, 128);
  doc.setLineWidth(0.8);
  doc.line(140, 40, 205, 40);
  doc.line(140, 65, 205, 65);
  doc.setFont("times", "bold");
  doc.setFontSize(12);
  doc.setTextColor(0, 0, 0);
  doc.text("Valido por 5 dias", 158, 50);
  doc.setFont("times", "normal");
  doc.text("Emision:", 165, 55);
  doc.text(
    `${date.getDate()}/${date.getMonth() + 1}/${date.getFullYear()}`,
    165,
    60
  );
  doc.setFontSize(10);
  doc.text("Razon Social", 10, 45);
  doc.text(`: ${datosCliente[1]}`, 40, 45);
  doc.text("Giro", 10, 50);
  doc.text(`: ${datosCliente[2]}`, 40, 50);
  doc.text("RUT", 10, 55);
  doc.text(`: ${datosCliente[0]}`, 40, 55);
  doc.setFont("times", "normal");
  doc.text("Direccion", 10, 60);
  doc.text(
    `: ${
      datosCliente[4] == "" || datosCliente[4] == null
        ? selectedDireccionFact?.direccion
        : datosCliente[4]
    } ${
      datosCliente[4] == "" || datosCliente[4] == null
        ? selectedDireccionFact?.nroDireccion
        : ""
    }`,
    40,
    60
  );
  doc.text("Comuna", 80, 55);
  doc.text(
    `: ${
      datosCliente[5] == "" || datosCliente[5] == null
        ? selectedDireccionFact?.comuna
        : datosCliente[5]
    }`,
    110,
    55
  );
  doc.text("Ciudad", 80, 60);
  doc.text(
    `: ${
      datosCliente[6] == "" || datosCliente[6] == null
        ? selectedDireccionFact?.ciudad
        : datosCliente[6]
    }`,
    110,
    60
  );
  doc.text("Atendido Por", 10, 65);
  doc.text(`: ${datosCliente[7]}`, 40, 65);
  autoTable(doc, options);
  doc.setDrawColor(0, 0, 0);
  doc.setLineWidth(0.5);
  doc.rect(14, 80, 188, 6);
  doc.rect(14, 86, 188, 136);
  doc.rect(14, 227, 120, 40);
  doc.rect(140, 227, 62, 40);
  doc.setFont("times", "bold");
  doc.setFontSize(12);
  doc.text("Nota:", 24, 232);
  doc.setFont("times", "normal");
  doc.text(`${datosCliente[9]}`, 20, 238);
  doc.setFont("times", "bold");
  doc.text("Subtotal", 145, 232);
  doc.text(
    `% Dcto ${
      cabecera?.porcentajeDescuento == undefined
        ? 0
        : cabecera?.porcentajeDescuento
    }`,
    145,
    240
  );
  doc.text("Neto", 145, 248);
  doc.text("Iva 19%", 145, 256);
  doc.text("Total", 145, 264);
  doc.setFont("times", "normal");
  doc.setFontSize(10);
  doc.text(
    `: $${new Intl.NumberFormat("cl-CL").format(cabecera?.totalVenta)}`,
    170,
    232
  );
  doc.text(
    `: $${
      cabecera?.porcentajeDescuento == undefined
        ? 0
        : new Intl.NumberFormat("cl-CL").format(
            Math.round(
              (cabecera?.totalVenta * cabecera?.porcentajeDescuento) / 100
            )
          )
    }`,
    170,
    240
  );
  doc.text(
    `: $${new Intl.NumberFormat("cl-CL").format(cabecera?.neto)}`,
    170,
    248
  );
  doc.text(
    `: $${new Intl.NumberFormat("cl-CL").format(cabecera?.iva)}`,
    170,
    256
  );
  doc.setFont("times", "bold");
  doc.text(
    `: $${new Intl.NumberFormat("cl-CL").format(cabecera?.total)}`,
    170,
    264
  );

  doc.save(`${nombreDelArchivo}.pdf`);
}

export async function generarPDFCheques(Cheques) {
  var doc = new jsPDF({
    orientation: "portrait",
    unit: "mm",
    format: "letter",
  });
  const columns = [
    "Rut",
    "Razon Social",
    "Banco",
    "Numero Serie",
    "Nro Comprobante",
    "Fecha Ingreso",
    "Fecha Vencimiento",
    "Monto",
  ];
  const rows = Cheques   ?.map((e) => [
    e.idCliente,
    e.razonSocial,
    e.banco,
    `${e.numeroSerie}`,
    `${e.nroComprobante}`,
    `${e.fechaCancelacion}`,
    `${e.fechaVencimiento}`,
    `$${e.monto}`,
  ]);
  const options = {
    head: [columns],
    body: rows,
    startY: 20,
    theme: "striped", // plain djiseño minimalista
    tableWidth: "auto",
    styles: {
      fillColor: [255, 255, 255],
      textColor: [0, 0, 0],
      lineColor: [0, 0, 0],
      cellWidth: "auto",
      fontSize: 8,
    },
  };
  var date = new Date();

  doc.setFontSize(12);
  doc.setFont("helvetica", "bold");
  doc.text("CHEQUES INGRESADOS", 80, 8);
  doc.setFontSize(10);
  doc.setFont("times", "normal");
  doc.text(`Fecha impreion: ${date.getDate()}/${date.getMonth() + 1}/${date.getFullYear()}` , 150, 8);
  // doc.text(
  //   "COMERCIALIZACIÓN DE ARTICULOS DEPORTIVOS BICICLETAS Y RESPUESTOS",
  //   45,
  //   14
  // );
  autoTable(doc, options);
  doc.save(`Chques ${date.getDate()}/${date.getMonth() + 1}/${date.getFullYear()}.pdf`);

}
