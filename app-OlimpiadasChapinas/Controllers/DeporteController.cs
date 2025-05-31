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
            else
            {
                TempData["ErrorMensaje"] = "No se pudo registrar el usuario. Verifique los datos.";
                return RedirectToAction("newDeporte", "Deporte");
            }
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
    }
}