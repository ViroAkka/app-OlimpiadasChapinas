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
using static app_OlimpiadasChapinas.Models.csEstructuraPuestoPremiacion;

namespace app_OlimpiadasChapinas.Controllers
{
    public class PuestoPremiacionController : Controller
    {
        // GET: PuestoPremiacion
        public ActionResult PuestoPremiacion(string idPuesto)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("loginUsuario", "Usuario");
            }

            DataSet data = new DataSet();
            var url = string.IsNullOrEmpty(idPuesto)
                ? $"http://localhost/api-OlimpiadasChapinas/rest/api/ListarPuestoPremiacion"
                : $"http://localhost/api-OlimpiadasChapinas/rest/api/ListarPuestoPremiacionPorID?idPuesto={idPuesto}";

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

        public ActionResult newPuestoPremiacion()
        {
            return View();
        }

        public ActionResult Guardar(FormCollection form)
        {
            string json = "";
            string resultJson = "";
            Byte[] reqByte, resByte;
            requestPuestoPremiacion InsertarPuestoPremiacion= new requestPuestoPremiacion();
            InsertarPuestoPremiacion.idPuesto = int.Parse(form["idPuesto"]);
            InsertarPuestoPremiacion.descripcion = form["descripcion"];

            json = JsonConvert.SerializeObject(InsertarPuestoPremiacion);

            WebClient client = new WebClient();
            string url = $"http://localhost/api-OlimpiadasChapinas/rest/api/InsertarPuestoPremiacion";
            var request = (HttpWebRequest)WebRequest.Create(url);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            client.Headers["content-type"] = "application/json";

            reqByte = Encoding.UTF8.GetBytes(json);
            resByte = client.UploadData(request.Address.ToString(), "post", reqByte);
            resultJson = Encoding.UTF8.GetString(resByte);

            responsePuestoPremiacion result = new responsePuestoPremiacion();
            result = JsonConvert.DeserializeObject<responsePuestoPremiacion>(resultJson);
            client.Dispose();

            if (result.respuesta == 1) return RedirectToAction("PuestoPremiacion", "PuestoPremiacion");
            else return RedirectToAction("newPuestoPremiacion", "PuestoPremiacion");
        }

        public ActionResult ActualizarPuestoPremiacion(string idPuesto)
        {
            DataSet data = new DataSet();
            var url = $"http://localhost/api-OlimpiadasChapinas/rest/api/ListarPuestoPremiacionPorID?idPuesto={idPuesto}";

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
            requestPuestoPremiacion ActualizarPuestoPremiacion = new requestPuestoPremiacion();
            ActualizarPuestoPremiacion.idPuesto = int.Parse(form["idPuesto"]);
            ActualizarPuestoPremiacion.descripcion = form["descripcion"];

            json = JsonConvert.SerializeObject(ActualizarPuestoPremiacion);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost/api-OlimpiadasChapinas/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var jsonContent = new StringContent(JsonConvert.SerializeObject(ActualizarPuestoPremiacion), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("rest/api/ActualizarPuestoPremiacion", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    resultJson = await response.Content.ReadAsStringAsync();
                    responsePuestoPremiacion result = JsonConvert.DeserializeObject<responsePuestoPremiacion>(resultJson);
                    if (result.respuesta == 1)
                        return RedirectToAction("PuestoPremiacion", "PuestoPremiacion");
                }

                return RedirectToAction("ActualizarPuestoPremiacion", "PuestoPremiacion");
            }
        }

        public ActionResult Eliminar(string idPuesto)
        {
            string json = "";
            string resultJson = "";
            Byte[] reqByte, resByte;

            WebClient client = new WebClient();
            string url = $"http://localhost/api-OlimpiadasChapinas/rest/api/EliminarPuestoPremiacion";
            var request = (HttpWebRequest)WebRequest.Create(url);

            requestPuestoPremiacion eliminar = new requestPuestoPremiacion();
            eliminar.idPuesto = int.Parse(idPuesto);

            json = JsonConvert.SerializeObject(eliminar);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            client.Headers["content-type"] = "application/json";

            reqByte = Encoding.UTF8.GetBytes(json);
            resByte = client.UploadData(request.Address.ToString(), "post", reqByte);
            resultJson = Encoding.UTF8.GetString(resByte);

            responsePuestoPremiacion result = new responsePuestoPremiacion();
            result = JsonConvert.DeserializeObject<responsePuestoPremiacion>(resultJson);
            client.Dispose();

            return RedirectToAction("PuestoPremiacion", "PuestoPremiacion");
        }
    }
}