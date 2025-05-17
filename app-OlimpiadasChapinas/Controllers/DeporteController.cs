using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Newtonsoft.Json;
using static app_OlimpiadasChapinas.Models.csEstructuraDeporte;
using ClosedXML.Excel;

namespace app_OlimpiadasChapinas.Controllers
{
    public class DeporteController : Controller
    {
        public ActionResult Deporte(string idDeporte)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("loginUsuario", "Usuario");
            }

            DataSet data = new DataSet();
            var url = string.IsNullOrEmpty(idDeporte) ? $"http://localhost/api-OlimpiadasChapinas/rest/api/ListarDeporte" : $"http://localhost/api-OlimpiadasChapinas/rest/api/listarDeportePorID?idDeporte={idDeporte}";

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

            return View(data);
        }

        public ActionResult newDeporte()
        {
            return View();
        }

        public ActionResult guardar(FormCollection form)
        {
            string json = "";
            string resultJson = "";
            Byte[] reqByte, resByte;
            requestDeporte InsertarDeporte = new requestDeporte();
            InsertarDeporte.nombre = form["nombre"];
            InsertarDeporte.categoria = form["categoria"];
            InsertarDeporte.descripcion = form["descripcion"];
            InsertarDeporte.cantidadJugadores = int.Parse(form["cantidadJugadores"].ToString());

            json = JsonConvert.SerializeObject(InsertarDeporte);

            WebClient client = new WebClient();
            string url = $"http://localhost/api-OlimpiadasChapinas/rest/api/InsertarDeporte";
            var request = (HttpWebRequest)WebRequest.Create(url);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            client.Headers["content-type"] = "application/json";

            reqByte = Encoding.UTF8.GetBytes(json);
            resByte = client.UploadData(request.Address.ToString(), "post", reqByte);
            resultJson = Encoding.UTF8.GetString(resByte);

            responseDeporte result = new responseDeporte();
            result = JsonConvert.DeserializeObject<responseDeporte>(resultJson);
            client.Dispose();

            if (result.respuesta == 1) return RedirectToAction("Deporte", "Deporte");
            else return RedirectToAction("newDeporte", "Deporte");
        }

        public ActionResult ActualizarDeporte(string idDeporte)
        {
            DataSet data = new DataSet();
            var url = "";

            url = $"http://localhost/api-OlimpiadasChapinas/rest/api/listarDeportePorID?idDeporte={idDeporte}";

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

            return View(data);
        }

        public ActionResult actualizar(FormCollection form)
        {
            string json = "";
            string resultJson = "";
            Byte[] reqByte, resByte;
            requestDeporte ActualizarDeporte = new requestDeporte();
            ActualizarDeporte.idDeporte = int.Parse(form["idDeporte"]);
            ActualizarDeporte.nombre = form["nombre"];
            ActualizarDeporte.categoria = form["categoria"];
            ActualizarDeporte.descripcion = form["descripcion"];
            ActualizarDeporte.cantidadJugadores = int.Parse(form["cantidadJugadores"].ToString());

            json = JsonConvert.SerializeObject(ActualizarDeporte);

            WebClient client = new WebClient();
            string url = $"http://localhost/api-OlimpiadasChapinas/rest/api/ActualizarDeporte";
            var request = (HttpWebRequest)WebRequest.Create(url);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            client.Headers["content-type"] = "application/json";

            reqByte = Encoding.UTF8.GetBytes(json);
            resByte = client.UploadData(request.Address.ToString(), "post", reqByte);
            resultJson = Encoding.UTF8.GetString(resByte);

            responseDeporte result = new responseDeporte();
            result = JsonConvert.DeserializeObject<responseDeporte>(resultJson);
            client.Dispose();

            if (result.respuesta == 1) return RedirectToAction("Deporte", "Deporte");
            else return RedirectToAction("ActualizarDeporte", "Deporte");
        }

        public ActionResult eliminar(string idDeporte)
        {
            string json = "";
            string resultJson = "";
            Byte[] reqByte, resByte;

            WebClient client = new WebClient();
            string url = $"http://localhost/api-OlimpiadasChapinas/rest/api/EliminarDeporte";
            var request = (HttpWebRequest)WebRequest.Create(url);

            requestDeporte eliminar = new requestDeporte();
            eliminar.idDeporte = int.Parse(idDeporte);

            json = JsonConvert.SerializeObject(eliminar);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            client.Headers["content-type"] = "application/json";

            reqByte = Encoding.UTF8.GetBytes(json);
            resByte = client.UploadData(request.Address.ToString(), "post", reqByte);
            resultJson = Encoding.UTF8.GetString(resByte);

            responseDeporte result = new responseDeporte();
            result = JsonConvert.DeserializeObject<responseDeporte>(resultJson);
            client.Dispose();

            return RedirectToAction("Deporte", "Deporte");
        }

        public DataSet ObtenerDeportes()
        {
            DataSet data = new DataSet();
            var url = $"http://localhost/api-OlimpiadasChapinas/rest/api/ListarDeporte";

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

        public ActionResult ExportarDeportesPDF()
        {
            // Obtener los datos del DataSet
            DataSet ds = ObtenerDeportes();
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
                Paragraph titulo = new Paragraph("Reporte de Deporte", tituloFont)
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

                return File(stream.ToArray(), "application/pdf", "Deporte.pdf");
            }
        }

        public ActionResult ExportarDeportesExcel()
        {
            DataSet ds = ObtenerDeportes();
            if (ds == null || ds.Tables.Count == 0)
            {
                return new HttpStatusCodeResult(500, "No se encontraron datos.");
            }

            DataTable tabla = ds.Tables[0];

            using (XLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add("Deportes");

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
                                "Deportes.xlsx");
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