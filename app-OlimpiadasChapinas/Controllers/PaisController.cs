using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using static app_OlimpiadasChapinas.Models.csEstructuraPais;

namespace app_OlimpiadasChapinas.Controllers
{
    public class PaisController : Controller
    {
        // GET: Pais
        public ActionResult Pais(string idPais)
        {
            DataSet data = new DataSet();
            var url = string.IsNullOrEmpty(idPais)
                ? $"http://localhost/api-OlimpiadasChapinas/rest/api/ListarPais"
                : $"http://localhost/api-OlimpiadasChapinas/rest/api/ListarPaisPorID?idPais={idPais}";

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

        public ActionResult newPais()
        {
            return View();
        }

        public ActionResult Guardar(FormCollection form)
        {
            string json = "";
            string resultJson = "";
            Byte[] reqByte, resByte;
            requestPais InsertarPais = new requestPais();
            InsertarPais.idPais = form["idPais"];
            InsertarPais.nombre = form["nombre"];

            json = JsonConvert.SerializeObject(InsertarPais);

            WebClient client = new WebClient();
            string url = $"http://localhost/api-OlimpiadasChapinas/rest/api/InsertarPais";
            var request = (HttpWebRequest)WebRequest.Create(url);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            client.Headers["content-type"] = "application/json";

            reqByte = Encoding.UTF8.GetBytes(json);
            resByte = client.UploadData(request.Address.ToString(), "post", reqByte);
            resultJson = Encoding.UTF8.GetString(resByte);

            responsePais result = new responsePais();
            result = JsonConvert.DeserializeObject<responsePais>(resultJson);
            client.Dispose();

            if (result.respuesta == 1) return RedirectToAction("Pais", "Pais");
            else return RedirectToAction("newPais", "Pais");
        }

        public ActionResult ActualizarPais(string idPais)
        {
            DataSet data = new DataSet();
            var url = $"http://localhost/api-OlimpiadasChapinas/rest/api/ListarPaisPorID?idPais={idPais}";

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

        public async Task<ActionResult> Actualizar(FormCollection form)
        {
            string json = "";
            string resultJson = "";
            requestPais ActualizarPais = new requestPais();
            ActualizarPais.idPais= (form["idPais"]);
            ActualizarPais.nombre= form["nombre"];

            json = JsonConvert.SerializeObject(ActualizarPais);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost/api-OlimpiadasChapinas/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var jsonContent = new StringContent(JsonConvert.SerializeObject(ActualizarPais), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("rest/api/ActualizarPais", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    resultJson = await response.Content.ReadAsStringAsync();
                    responsePais result = JsonConvert.DeserializeObject<responsePais>(resultJson);
                    if (result.respuesta == 1)
                        return RedirectToAction("Pais", "Pais");
                }

                return RedirectToAction("ActualizarPais", "Pais");
            }
        }

        public ActionResult Eliminar(string idPais)
        {
            string json = "";
            string resultJson = "";
            Byte[] reqByte, resByte;

            WebClient client = new WebClient();
            string url = $"http://localhost/api-OlimpiadasChapinas/rest/api/EliminarPais";
            var request = (HttpWebRequest)WebRequest.Create(url);

            requestPais eliminar = new requestPais();
            eliminar.idPais = idPais;

            json = JsonConvert.SerializeObject(eliminar);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            client.Headers["content-type"] = "application/json";

            reqByte = Encoding.UTF8.GetBytes(json);
            resByte = client.UploadData(request.Address.ToString(), "post", reqByte);
            resultJson = Encoding.UTF8.GetString(resByte);

            responsePais result = new responsePais();
            result = JsonConvert.DeserializeObject<responsePais>(resultJson);
            client.Dispose();

            return RedirectToAction("Pais", "Pais");
        }
    }
}