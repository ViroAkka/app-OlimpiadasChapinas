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
using static app_OlimpiadasChapinas.Models.csEstructuraFormaPago;

namespace app_OlimpiadasChapinas.Controllers
{
    public class FormaPagoController : Controller
    {
        // GET: FormaPago
        public ActionResult FormaPago(string idFormaPago)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("loginUsuario", "Usuario");
            }

            DataSet data = new DataSet();
            var url = string.IsNullOrEmpty(idFormaPago)
                ? $"http://localhost/api-OlimpiadasChapinas/rest/api/ListarFormaPago"
                : $"http://localhost/api-OlimpiadasChapinas/rest/api/ListarFormaPagoPorID?idFormaPago={idFormaPago}";

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

        public ActionResult newFormaPago()
        {
            return View();
        }

        public ActionResult Guardar(FormCollection form)
        {
            string json = "";
            string resultJson = "";
            Byte[] reqByte, resByte;
            requestFormaPago InsertarFormaPago = new requestFormaPago();
            InsertarFormaPago.descripcion = form["descripcion"];
            
            json = JsonConvert.SerializeObject(InsertarFormaPago);

            WebClient client = new WebClient();
            string url = $"http://localhost/api-OlimpiadasChapinas/rest/api/InsertarFormaPago";
            var request = (HttpWebRequest)WebRequest.Create(url);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            client.Headers["content-type"] = "application/json";

            reqByte = Encoding.UTF8.GetBytes(json);
            resByte = client.UploadData(request.Address.ToString(), "post", reqByte);
            resultJson = Encoding.UTF8.GetString(resByte);

            responseFormaPago result = new responseFormaPago();
            result = JsonConvert.DeserializeObject<responseFormaPago>(resultJson);
            client.Dispose();

            if (result.respuesta == 1) return RedirectToAction("FormaPago", "FormaPago");
            else return RedirectToAction("newFormaPago", "FormaPago");
        }

        public ActionResult ActualizarFormaPago(int idFormaPago)
        {
            DataSet data = new DataSet();
            var url = $"http://localhost/api-OlimpiadasChapinas/rest/api/ListarFormaPagoPorID?idFormaPago={idFormaPago}";

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
            requestFormaPago ActualizarFormaPago = new requestFormaPago();
            ActualizarFormaPago.idFormaPago = int.Parse(form["idFormaPago"]);
            ActualizarFormaPago.descripcion = form["descripcion"];

            json = JsonConvert.SerializeObject(ActualizarFormaPago);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost/api-OlimpiadasChapinas/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var jsonContent = new StringContent(JsonConvert.SerializeObject(ActualizarFormaPago), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("rest/api/ActualizarFormaPago", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    resultJson = await response.Content.ReadAsStringAsync();
                    responseFormaPago result = JsonConvert.DeserializeObject<responseFormaPago>(resultJson);
                    if (result.respuesta == 1)
                        return RedirectToAction("FormaPago", "FormaPago");
                }

                return RedirectToAction("ActualizarFormaPago", "FormaPago");
            }
        }

        public ActionResult Eliminar(string idFormaPago)
        {
            string json = "";
            string resultJson = "";
            Byte[] reqByte, resByte;

            WebClient client = new WebClient();
            string url = $"http://localhost/api-OlimpiadasChapinas/rest/api/EliminarFormaPago";
            var request = (HttpWebRequest)WebRequest.Create(url);

            requestFormaPago eliminar = new requestFormaPago();
            eliminar.idFormaPago = int.Parse(idFormaPago);

            json = JsonConvert.SerializeObject(eliminar);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            client.Headers["content-type"] = "application/json";

            reqByte = Encoding.UTF8.GetBytes(json);
            resByte = client.UploadData(request.Address.ToString(), "post", reqByte);
            resultJson = Encoding.UTF8.GetString(resByte);

            responseFormaPago result = new responseFormaPago();
            result = JsonConvert.DeserializeObject<responseFormaPago>(resultJson);
            client.Dispose();

            return RedirectToAction("FormaPago", "FormaPago");
        }
    }
}
