using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using static app_OlimpiadasChapinas.Models.csEstructuraDeporte;

namespace app_OlimpiadasChapinas.Controllers
{
    public class DeporteController : Controller
    {
        

        public async Task<ActionResult> Deporte(string idDeporte)
        {
            List<requestDeporte> deportes = new List<requestDeporte>();
            var url = string.IsNullOrEmpty(idDeporte) 
                ? $"http://localhost/api-OlimpiadasChapinas/rest/api/ListarDeporte" 
                : $"http://localhost/api-OlimpiadasChapinas/rest/api/listarDeportePorID?idDeporte={idDeporte}";

            using (HttpClient webClient = new HttpClient())
            {
                webClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    HttpResponseMessage response = await webClient.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        if (string.IsNullOrEmpty(responseBody))
                        {
                            deportes = JsonConvert.DeserializeObject<List<requestDeporte>>(responseBody) ?? new List<requestDeporte>();
                        }
                        else
                        {
                            var deporte = JsonConvert.DeserializeObject<requestDeporte>(responseBody);
                            if (deporte != null)
                            {
                                deportes.Add(deporte);
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "No se pudo obtener la información del API.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error al conectar con el API: {ex.Message}");
                }
            }

            return View(deportes);
        }

        public ActionResult newDeporte()
        {
            return View();
        }

        //[HttpPost]
        //public async Task<ActionResult> Guardar(requestDeporteByID model)
        //{
        //    using (HttpClient client = new HttpClient())
        //    {
        //        string json = JsonConvert.SerializeObject(model);
        //        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
        //        HttpResponseMessage response = await client.PostAsync(apiBaseUrl + "InsertarDeporte", content);
        //        return RedirectToAction(response.IsSuccessStatusCode ? "Deporte" : "newDeporte");
        //    }
        //}

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

        //public async Task<ActionResult> ActualizarDeporte(string idDeporte)
        //{
        //    requestDeporteByID deporte = await GetApiResponse<requestDeporteByID>("ListarDeportePorID?idDeporte={idDeporte}");
        //    return View(deporte);
        //}

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

        //[HttpPost]
        //public async Task<ActionResult> Actualizar(requestDeporteByID model)
        //{
        //    using (HttpClient client = new HttpClient())
        //    {
        //        string json = JsonConvert.SerializeObject(model);
        //        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
        //        HttpResponseMessage response = await client.PostAsync(apiBaseUrl + "ActualizarDeporte", content);
        //        return RedirectToAction(response.IsSuccessStatusCode ? "Deporte": "ActualizarDeporte", new { idDeporte = model.idDeporte});
        //    }
        //}

        public ActionResult actualizar(FormCollection form)
        {
            string json = "";
            string resultJson = "";
            Byte[] reqByte, resByte;
            requestDeporteByID InsertarDeporte = new requestDeporteByID();
            InsertarDeporte.idDeporte = int.Parse(form["idDeporte"]);
            InsertarDeporte.nombre = form["nombre"];
            InsertarDeporte.categoria = form["categoria"];
            InsertarDeporte.descripcion = form["descripcion"];
            InsertarDeporte.cantidadJugadores = int.Parse(form["cantidadJugadores"].ToString());

            json = JsonConvert.SerializeObject(InsertarDeporte);

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

        //public async Task<ActionResult> Eliminar(int idDeporte)
        //{
        //    using (HttpClient client = new HttpClient())
        //    {
        //        string json = JsonConvert.SerializeObject(new { idDeporte });
        //        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
        //        HttpResponseMessage response = await client.PostAsync(apiBaseUrl + "EliminarDeporte", content);
        //        return RedirectToAction("Deporte");
        //    }
        //}

        public ActionResult eliminar(string idDeporte)
        {
            string json = "";
            string resultJson = "";
            Byte[] reqByte, resByte;

            WebClient client = new WebClient();
            string url = $"http://localhost/api-OlimpiadasChapinas/rest/api/EliminarDeporte";
            var request = (HttpWebRequest)WebRequest.Create(url);

            requestEliminarDeporte eliminar = new requestEliminarDeporte();
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