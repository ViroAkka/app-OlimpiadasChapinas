using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Newtonsoft.Json;

namespace app_OlimpiadasChapinas.Controllers
{
    public class ReportesController : Controller
    {
        public DataSet ObtenerDataSet(string nombreDataSet)
        {
            DataSet data = new DataSet();
            var url = $"http://localhost/api-OlimpiadasChapinas/rest/api/Listar{nombreDataSet}";

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            string responseBody = "";

            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream strReader = response.GetResponseStream())
                    {
                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            responseBody = objReader.ReadToEnd();
                        }
                    }
                    data = JsonConvert.DeserializeObject<DataSet>(responseBody) ?? new DataSet();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocurrió un error: {ex.Message}");
            }

            return data;
        }

        public ActionResult ExportarPDF(string nombreDataSet)
        {
            // Obtener los datos del DataSet
            DataSet ds = ObtenerDataSet(nombreDataSet);
            if (ds == null || ds.Tables.Count == 0)
            {
                return new HttpStatusCodeResult(500, "No se encontraron datos.");
            }

            DataTable tabla = ds.Tables[0];

            // Crear documento PDF
            using (MemoryStream stream = new MemoryStream())
            {
                Document pdfDoc = new Document(PageSize.A4.Rotate(), 25, 25, 30, 30);
                PdfWriter.GetInstance(pdfDoc, stream);
                pdfDoc.Open();

                // Título
                Font tituloFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                Paragraph titulo = new Paragraph($"Reporte de {nombreDataSet}", tituloFont)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 15f
                };
                pdfDoc.Add(titulo);

                // Estilos de celda
                Font encabezadoFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, BaseColor.WHITE);
                Font contenidoFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);

                PdfPTable pdfTable = new PdfPTable(tabla.Columns.Count);
                pdfTable.WidthPercentage = 100;

                // Agregar encabezados
                foreach (DataColumn columna in tabla.Columns)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(columna.ColumnName, encabezadoFont))
                    {
                        BackgroundColor = BaseColor.GRAY,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        Padding = 5
                    };
                    pdfTable.AddCell(cell);
                }

                // Agregar filas
                foreach (DataRow fila in tabla.Rows)
                {
                    foreach (var item in fila.ItemArray)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(item.ToString(), contenidoFont))
                        {
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            Padding = 5
                        };
                        pdfTable.AddCell(cell);
                    }
                }

                pdfDoc.Add(pdfTable);
                pdfDoc.Close();

                return File(stream.ToArray(), "application/pdf", $"{nombreDataSet}.pdf");
            }
        }

        public ActionResult ExportarExcel(string nombreDataSet)
        {
            DataSet ds = ObtenerDataSet(nombreDataSet);
            if (ds == null || ds.Tables.Count == 0)
            {
                return new HttpStatusCodeResult(500, "No se encontraron datos.");
            }

            DataTable tabla = ds.Tables[0];

            using (XLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add($"{nombreDataSet}");

                // Cargar los datos
                ws.Cell(1, 1).InsertTable(tabla);

                // Estilo de título
                ws.Range("A1:" + GetExcelColumnName(tabla.Columns.Count) + "1")
                  .Style.Font.Bold = true;

                // Autoajustar anchos de columnas
                ws.Columns().AdjustToContents();

                // Centrar el contenido de todas las celdas
                ws.RangeUsed().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.RangeUsed().Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    return File(stream.ToArray(),
                                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                $"{nombreDataSet}.xlsx");
                }
            }
        }

        private string GetExcelColumnName(int columnNumber)
        {
            string columnName = "";

            while (columnNumber > 0)
            {
                int modulo = (columnNumber - 1) % 26;
                columnName = Convert.ToChar(65 + modulo) + columnName;
                columnNumber = (columnNumber - modulo) / 26;
            }

            return columnName;
        }
    }
}