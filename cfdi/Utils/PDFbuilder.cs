using cfdi.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace cfdi.Utils
{
    public class PDFbuilder
    {

        public void PDFgenerate(CFDi cfdi)
        {
            FileStream fs = new FileStream("C://TOMZA.SYS/cfdi/pdf/reporte.pdf", FileMode.Create);
            Document doc = new Document(iTextSharp.text.PageSize.LETTER);
            doc.SetMargins(10f, 10f, 10f, 10f);
            PdfWriter pw = PdfWriter.GetInstance(doc, fs);
           
            doc.Open();

            //----FUENTES----//
            BaseFont _titulo = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1252, true);
            iTextSharp.text.Font titulo = new iTextSharp.text.Font(_titulo, 14f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(0, 0, 0));

            BaseFont _subtitulo = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, true);
            iTextSharp.text.Font subtitulo = new iTextSharp.text.Font(_subtitulo, 12f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(0, 0, 0));

            BaseFont _parrafo = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, true);
            iTextSharp.text.Font parrafo = new iTextSharp.text.Font(_parrafo, 7f, iTextSharp.text.Font.NORMAL, new iTextSharp.text.BaseColor(0, 0, 0));

            BaseFont _totales = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, true);
            iTextSharp.text.Font totales = new iTextSharp.text.Font(_parrafo, 7f, iTextSharp.text.Font.BOLD, new iTextSharp.text.BaseColor(0, 0, 0));

            BaseFont _titulo_blanco = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, true);
            iTextSharp.text.Font titulo_blanco = new iTextSharp.text.Font(_titulo_blanco, 7f, iTextSharp.text.Font.NORMAL, new iTextSharp.text.BaseColor(255, 255, 255));

            //-------------//

            iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance("C:/TOMZA.SYS/CERTIFICADOS_ERP/GASOMATICO S.A. DE C.V/logo_GAS710629HU3.jpg");
            //iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(cfdi.emisor.certificado.rutaCert + "\\logo_" + cfdi.emisor.rfcSucursal + ".jpg");
            logo.ScaleAbsolute(90, 45);

            var tb1 = new PdfPTable(new float[] { 30f, 70f }) { WidthPercentage = 100f };
            tb1.AddCell(new PdfPCell(logo) { Padding = 1.5f, Border = 0, Rowspan = 3, HorizontalAlignment = Element.ALIGN_LEFT });
            tb1.AddCell(new PdfPCell(new Phrase(cfdi.emisor.sucursal, titulo)) { Border = 0 });
            tb1.AddCell(new PdfPCell(new Phrase("RFC: " + cfdi.emisor.rfcSucursal, parrafo)) { Border = 0 });
            tb1.AddCell(new PdfPCell(new Phrase("Regimen Fiscal: General de Ley Personas Morales", parrafo)) { Border = 0 });

            tb1.AddCell(new PdfPCell(new Phrase("CLIENTE", titulo_blanco)) { Padding = 3f, Border = 0, BackgroundColor = new BaseColor(47, 54, 64), HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE });
            tb1.AddCell(new PdfPCell(new Phrase("LUGAR EXPEDICION: " + cfdi.emisor.codigoPostal, titulo_blanco)) { Padding = 3f, BorderColorLeft = new BaseColor(47, 54, 64), BorderColorBottom = new BaseColor(47, 54, 64), BorderWidthTop = 0, BorderWidthLeft = 100, BorderColorRight = new BaseColor(255, 255, 255), BackgroundColor = new BaseColor(47, 54, 64), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
            tb1.AddCell(new PdfPCell(new Phrase("GAS COMERCIAL DE VILLA AHUMADA", parrafo)) { Border = 0, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE });
            tb1.AddCell(new PdfPCell(new Phrase("RFC: " + cfdi.receptor.rfcReceptor, parrafo)) { Border = 0, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

            tb1.AddCell(new PdfPCell(new Phrase("")) { Border = 0, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE });
            tb1.AddCell(new PdfPCell(new Phrase("")) { Border = 0, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

            var tb2 = new PdfPTable(new float[] { 50f, 50f }) { WidthPercentage = 100f };
            tb2.AddCell(new PdfPCell(new Phrase("FOLIO FISCAL", titulo_blanco)) { Padding = 3f, Border = 0, BackgroundColor = new BaseColor(47, 54, 64), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
            tb2.AddCell(new PdfPCell(new Phrase("FACTURA", titulo_blanco)) { Padding = 3f, Border = 0, BackgroundColor = new BaseColor(47, 54, 64), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
            tb2.AddCell(new PdfPCell(new Phrase(cfdi.folioFiscal, parrafo)) { Border = 0, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
            tb2.AddCell(new PdfPCell(new Phrase(cfdi.serie + " " + cfdi.folio, parrafo)) { Border = 0, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

            tb2.AddCell(new PdfPCell(new Phrase("FECHA DE CERTIFICACION", titulo_blanco)) { Padding= 3f, Border = 0, BackgroundColor = new BaseColor(47, 54, 64), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
            tb2.AddCell(new PdfPCell(new Phrase("FECHA EMISION", titulo_blanco)) { Padding = 3f, Border = 0, BackgroundColor = new BaseColor(47, 54, 64), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
            tb2.AddCell(new PdfPCell(new Phrase(cfdi.fechaCert.ToString(), parrafo)) { Border = 0, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
            tb2.AddCell(new PdfPCell(new Phrase(cfdi.fecha.ToString(), parrafo)) { Border = 0, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

            tb2.AddCell(new PdfPCell(new Phrase("No. CERTIFICADO SAT", titulo_blanco)) { Padding = 3f, Border = 0, BackgroundColor = new BaseColor(47, 54, 64), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
            tb2.AddCell(new PdfPCell(new Phrase("FORMA DE PAGO", titulo_blanco)) { Padding = 3f, Border = 0, BackgroundColor = new BaseColor(47, 54, 64), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
            tb2.AddCell(new PdfPCell(new Phrase("Por Defirnir", parrafo)) { Border = 0, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
            tb2.AddCell(new PdfPCell(new Phrase("Por Defirnir", parrafo)) { Border = 0, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

            tb2.AddCell(new PdfPCell(new Phrase("No. DE CERTIFICADO EMISOR", titulo_blanco)) { Padding = 3f, Border = 0, BackgroundColor = new BaseColor(47, 54, 64), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
            tb2.AddCell(new PdfPCell(new Phrase("METODO DE PAGO", titulo_blanco)) { Padding = 3f, Border = 0, BackgroundColor = new BaseColor(47, 54, 64), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
            tb2.AddCell(new PdfPCell(new Phrase(cfdi.NoCertificadoEmisor, parrafo)) { Border = 0, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
            tb2.AddCell(new PdfPCell(new Phrase(cfdi.mPago, parrafo)) { Border = 0, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

            var tb_principal = new PdfPTable(new float[] { 60f, 40f }) { WidthPercentage = 100f };
            tb_principal.AddCell(new PdfPCell(tb1) { Border = 0, HorizontalAlignment = Element.ALIGN_LEFT });
            tb_principal.AddCell(new PdfPCell(tb2) { Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
            tb_principal.AddCell(new PdfPCell(new Phrase("AAAAAAAAAAAAAAAA", titulo_blanco)) { Border = 0 });
            tb_principal.AddCell(new PdfPCell(new Phrase("BBBBBBBBBBBBBBBB", titulo_blanco)) { Border = 0 });

            tb_principal.DefaultCell.Border = 0;
            tb_principal.DefaultCell.Padding = 10f;

            doc.Add(tb_principal);

            //ENCABEZADO//
            tb1 = new PdfPTable(new float[] { 15f, 15f, 40f, 15f, 15f }) { WidthPercentage = 100f, HorizontalAlignment = Element.ALIGN_LEFT };
            tb1.AddCell(new PdfPCell(new Phrase("CANTIDAD", titulo_blanco)) { Padding = 3f, Border = 0, BackgroundColor = new BaseColor(47, 54, 64), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
            tb1.AddCell(new PdfPCell(new Phrase("U.MEDIDA", titulo_blanco)) { Padding = 3f, Border = 0, BackgroundColor = new BaseColor(47, 54, 64), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
            tb1.AddCell(new PdfPCell(new Phrase("DESCRIPCION DEL PRODUCTO", titulo_blanco)) { Padding = 3f, Border = 0, BackgroundColor = new BaseColor(47, 54, 64), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
            tb1.AddCell(new PdfPCell(new Phrase("P.UNITARIO", titulo_blanco)) { Padding = 3f, Border = 0, BackgroundColor = new BaseColor(47, 54, 64), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
            tb1.AddCell(new PdfPCell(new Phrase("IMPORTE", titulo_blanco)) { Padding = 3f, Border = 0, BackgroundColor = new BaseColor(47, 54, 64), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

            int i = 0;
            foreach (var order in cfdi.conceptos)
            {
                if (i % 2 == 0)
                {
                    tb1.AddCell(new PdfPCell(new Phrase(order.cantidad.ToString("f2"), parrafo)) { BackgroundColor = new BaseColor(255, 255, 255), Border = 0, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    tb1.AddCell(new PdfPCell(new Phrase(order.unidad, parrafo)) { BackgroundColor = new BaseColor(255, 255, 255), Border = 0, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    tb1.AddCell(new PdfPCell(new Phrase(order.descripcion, parrafo)) { BackgroundColor = new BaseColor(255, 255, 255), Border = 0, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    tb1.AddCell(new PdfPCell(new Phrase(order.valorUnitario.ToString("f2"), parrafo)) { BackgroundColor = new BaseColor(255, 255, 255), Border = 0, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    tb1.AddCell(new PdfPCell(new Phrase("$" + order.importe.ToString("f2"), parrafo)) { BackgroundColor = new BaseColor(255, 255, 255), Border = 0, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                }
                else
                {
                    tb1.AddCell(new PdfPCell(new Phrase(order.cantidad.ToString("f2"), parrafo)) { BackgroundColor = new BaseColor(204, 204, 204), Border = 0, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    tb1.AddCell(new PdfPCell(new Phrase(order.unidad, parrafo)) { BackgroundColor = new BaseColor(204, 204, 204), Border = 0, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    tb1.AddCell(new PdfPCell(new Phrase(order.descripcion, parrafo)) { BackgroundColor = new BaseColor(204, 204, 204), Border = 0, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    tb1.AddCell(new PdfPCell(new Phrase(order.valorUnitario.ToString("f2"), parrafo)) { BackgroundColor = new BaseColor(204, 204, 204), Border = 0, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    tb1.AddCell(new PdfPCell(new Phrase("$" + order.importe.ToString("f2"), parrafo)) { BackgroundColor = new BaseColor(204, 204, 204), Border = 0, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                }
                i++;
                
            }
            

            doc.Add(tb1);

            var tb_totales = new PdfPTable(new float[] { 50f }) { WidthPercentage = 50f, HorizontalAlignment = Element.ALIGN_RIGHT };
            tb_totales.AddCell(new PdfPCell(new Phrase("AAAAAAAAAAAAAAAA", titulo_blanco)) { Border = 0 });
            tb_totales.AddCell(new PdfPCell(new Phrase("AAAAAAAAAAAAAAAA", titulo_blanco)) { Border = 0 });
            tb_totales.AddCell(new PdfPCell(new Phrase("TOTALES", titulo_blanco)) { Padding = 3f, Border = 0, BackgroundColor = new BaseColor(47, 54, 64), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

            doc.Add(tb_totales);

            tb_totales = new PdfPTable(new float[] { 50f, 50f }) { WidthPercentage = 50f, HorizontalAlignment = Element.ALIGN_RIGHT };
            tb_totales.AddCell(new PdfPCell(new Phrase("SUBTOTAL", totales)) { Padding = 3f, Border = 0, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE });
            tb_totales.AddCell(new PdfPCell(new Phrase("$" + cfdi.subtotal.ToString("f2"), parrafo)) { Padding = 3f, Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_MIDDLE });
            tb_totales.AddCell(new PdfPCell(new Phrase("I.V.A", totales)) { BackgroundColor = new BaseColor(204, 204, 204), Padding = 3f, Border = 0, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE });
            tb_totales.AddCell(new PdfPCell(new Phrase("$" + cfdi.totalImp.ToString("f2"), parrafo)) { BackgroundColor = new BaseColor(204, 204, 204), Padding = 3f, Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_MIDDLE });
            tb_totales.AddCell(new PdfPCell(new Phrase("TOTAL", totales)) { Padding = 3f, Border = 0, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE });
            tb_totales.AddCell(new PdfPCell(new Phrase("$" + cfdi.total.ToString("f2"), parrafo)) { Padding = 3f, Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_MIDDLE });
            
            doc.Add(tb_totales);

            tb1 = new PdfPTable(new float[] { 100f }) { WidthPercentage = 100f };

            tb1.AddCell(new PdfPCell(new Phrase("IMPORTE CON LETRA", titulo_blanco)) { Padding = 3f, Border = 0, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
            tb1.AddCell(new PdfPCell(new Phrase("IMPORTE CON LETRA", titulo_blanco)) { Padding = 3f, Border = 0, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

            tb1.AddCell(new PdfPCell(new Phrase("IMPORTE CON LETRA", titulo_blanco)) { Padding = 3f, Border = 0, BackgroundColor = new BaseColor(47, 54, 64), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
            tb1.AddCell(new PdfPCell(new Phrase(cfdi.importeLetra, parrafo)) { Border = 0, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
            tb1.AddCell(new PdfPCell(new Phrase("CADENA ORIGINAL DEL COMPLEMENTO DE CERTIFICACION DIGITAL DEL SAT ", titulo_blanco)) { Padding = 3f, Border = 0, BackgroundColor = new BaseColor(47, 54, 64), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
            tb1.AddCell(new PdfPCell(new Phrase(cfdi.cadenaCertificadoSat, parrafo)) { Border = 0, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
            tb1.AddCell(new PdfPCell(new Phrase("SELLO DIGITAL DEL EMISOR", titulo_blanco)) { Padding = 3f, Border = 0, BackgroundColor = new BaseColor(47, 54, 64), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
            tb1.AddCell(new PdfPCell(new Phrase(cfdi.selloEmisor, parrafo)) { Border = 0, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
            tb1.AddCell(new PdfPCell(new Phrase("SELLO DIGITAL DEL SAT", titulo_blanco)) { Padding = 3f, Border = 0, BackgroundColor = new BaseColor(47, 54, 64), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
            tb1.AddCell(new PdfPCell(new Phrase(cfdi.selloSat, parrafo)) { Border = 0, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
         
            doc.Add(tb1);

            tb1 = new PdfPTable(new float[] { 100f }) { WidthPercentage = 100f };

            tb1.AddCell(new PdfPCell(new Phrase("IMPORTE CON LETRA", titulo_blanco)) { Padding = 3f, Border = 0, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
            tb1.AddCell(new PdfPCell(new Phrase("IMPORTE CON LETRA", titulo_blanco)) { Padding = 3f, Border = 0, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
            tb1.AddCell(new PdfPCell(new Phrase("Conforme al artículo 364 del Código de Comercio, se reserva expesamente el derecho a los intereses moratorios pactados y las entregas a cuenta se imputaran en primer termino al pago de intereses moratorios si existen y después al capital debido. ", parrafo)) { Border = 0, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

            doc.Add(tb1);

            //CODIGO QR//
            tb1 = new PdfPTable(new float[] { 80f,20f }) { WidthPercentage = 50f, HorizontalAlignment = Element.ALIGN_RIGHT };
            BarcodeQRCode barcodeQRCode = new BarcodeQRCode("https://verificacfdi.facturaelectronica.sat.gob.mx/default.aspx?re=" + cfdi.emisor.rfcSucursal + "&rr=" + cfdi.receptor.rfcReceptor + "&tt=" + cfdi.total + "&id=" + cfdi.folioFiscal + "&fe=" + cfdi.selloEmisor.Substring(cfdi.selloEmisor.Length -9, 8), 1000, 1000, null);
            Image codeQRImage = barcodeQRCode.GetImage();
            codeQRImage.ScaleAbsolute(60, 60);

            tb1.AddCell(new PdfPCell(new Phrase("CONDICIONES DE PAGO: \n" + cfdi.tipoVenta, titulo)) { Padding = 3f, Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_MIDDLE });
            tb1.AddCell(new PdfPCell(new Phrase("IMPORTE CON LETRA", titulo_blanco)) { Padding = 3f, Border = 0, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE });
            tb1.AddCell(new PdfPCell(new Phrase("ESTE RECIBO UNICAMENTE SERA\n VALIDO COMO PAGO SI PRESENTA\n EL COMPROBANTE QUE AMPARE\n EL IMPORTE DEL MISMO\n EFECTOS FISCALES DE PAGO.", parrafo)) { Padding = 3f, Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_MIDDLE });
            tb1.AddCell(new PdfPCell(codeQRImage) { Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_MIDDLE });

            doc.Add(tb1);

            tb1 = new PdfPTable(new float[] { 100f }) { WidthPercentage = 100f, HorizontalAlignment = Element.ALIGN_LEFT };
            tb1.AddCell(new PdfPCell(new Phrase("ESTE DOCUMENTO ES UNA REPRESENTACION IMPRESA DE UN CFDi V3.3", titulo_blanco)) { Padding = 3f, Border = 0, BackgroundColor = new BaseColor(47, 54, 64), HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

            doc.Add(tb1);

            doc.Close();
            pw.Close();
        }
    }
}
