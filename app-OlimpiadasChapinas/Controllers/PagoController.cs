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
using static app_OlimpiadasChapinas.Models.csEstructuraPago;
using app_OlimpiadasChapinas.Models;

namespace app_OlimpiadasChapinas.Controllers
{
    public class PagoController : Controller
    {
        // GET: Pago
        public ActionResult Pago(string idPago)
        {
            DataSet data = new DataSet();
            var url = string.IsNullOrEmpty(idPago)
                ? $"http://localhost/api-OlimpiadasChapinas/rest/api/ListarPago"
                : $"http://localhost/api-OlimpiadasChapinas/rest/api/ListarPagoPorID?idPago={idPago}";

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

        public ActionResult newPago()
        {
            DataSet formaPagos = new DataSet();

            string urlFormaPagos = $"http://localhost/api-OlimpiadasChapinas/rest/api/ListarFormaPago";

            try
            {
                formaPagos = createDataSet(urlFormaPagos);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocurrió un error: {ex.Message}");
            }

            csDataSets model = new csDataSets
            {
                FormaPago = formaPagos
            };

            return View(model);
        }

        public ActionResult Guardar(FormCollection form)
        {
            string json = "";
            string resultJson = "";
            Byte[] reqByte, resByte;
            requestPago InsertarPago = new requestPago();
            InsertarPago.idFormaPago = returnID(form["idFormaPago"]);
            InsertarPago.montoPago = double.Parse(form["montoPago"]);
            InsertarPago.observaciones = form["observaciones"].Trim();

            json = JsonConvert.SerializeObject(InsertarPago);

            WebClient client = new WebClient();
            string url = $"http://localhost/api-OlimpiadasChapinas/rest/api/InsertarPago";
            var request = (HttpWebRequest)WebRequest.Create(url);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            client.Headers["content-type"] = "application/json";

            reqByte = Encoding.UTF8.GetBytes(json);
            resByte = client.UploadData(request.Address.ToString(), "post", reqByte);
            resultJson = Encoding.UTF8.GetString(resByte);

            responsePago result = new responsePago();
            result = JsonConvert.DeserializeObject<responsePago>(resultJson);
            client.Dispose();

            if (result.respuesta == 1) return RedirectToAction("Pago", "Pago");
            else return RedirectToAction("newPago", "Pago");
        }

        public ActionResult ActualizarPago(int idPago)
        {
            DataSet data = new DataSet();
            DataSet formaPagos = new DataSet();

            var url = $"http://localhost/api-OlimpiadasChapinas/rest/api/ListarPagoPorID?idPago={idPago}";
            string urlFormaPagos = $"http://localhost/api-OlimpiadasChapinas/rest/api/ListarFormaPago";

            try
            {
                data = createDataSet(url);
                formaPagos = createDataSet(urlFormaPagos);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocurrió un error: {ex.Message}");
            }

            csDataSets model = new csDataSets
            {
                Data = data,
                FormaPago = formaPagos
            };

            return View(model);
        }

        public async Task<ActionResult> Actualizar(FormCollection form)
        {
            string json = "";
            string resultJson = "";
            requestPago ActualizarPago = new requestPago();
            ActualizarPago.idPago = int.Parse(form["idPago"]);
            ActualizarPago.idFormaPago = returnID(form["idFormaPago"]);
            ActualizarPago.montoPago = double.Parse(form["montoPago"]);
            ActualizarPago.observaciones = form["observaciones"].Trim();

            json = JsonConvert.SerializeObject(ActualizarPago);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost/api-OlimpiadasChapinas/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var jsonContent = new StringContent(JsonConvert.SerializeObject(ActualizarPago), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("rest/api/ActualizarPago", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    resultJson = await response.Content.ReadAsStringAsync();
                    responsePago result = JsonConvert.DeserializeObject<responsePago>(resultJson);
                    if (result.respuesta == 1)
                        return RedirectToAction("Pago", "Pago");
                }

                return RedirectToAction("ActualizarPago", "Pago");
            }
        }

        public ActionResult Eliminar(string idPago)
        {
            string json = "";
            string resultJson = "";
            Byte[] reqByte, resByte;

            WebClient client = new WebClient();
            string url = $"http://localhost/api-OlimpiadasChapinas/rest/api/EliminarPago";
            var request = (HttpWebRequest)WebRequest.Create(url);

            requestPago eliminar = new requestPago();
            eliminar.idPago = int.Parse(idPago);

            json = JsonConvert.SerializeObject(eliminar);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            client.Headers["content-type"] = "application/json";

            reqByte = Encoding.UTF8.GetBytes(json);
            resByte = client.UploadData(request.Address.ToString(), "post", reqByte);
            resultJson = Encoding.UTF8.GetString(resByte);

            responsePago result = new responsePago();
            result = JsonConvert.DeserializeObject<responsePago>(resultJson);
            client.Dispose();

            return RedirectToAction("Pago", "Pago");
        }

        static DataSet createDataSet(string url)
        {
            DataSet data = new DataSet();
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Accept = "application/json";

            using (WebResponse response = request.GetResponse())
            {
                using (Stream strReader = response.GetResponseStream())
                {
                    using (StreamReader objReader = new StreamReader(strReader))
                    {
                        string responseBody = objReader.ReadToEnd();
                        data = JsonConvert.DeserializeObject<DataSet>(responseBody) ?? new DataSet();
                    }
                }
            }

            return data;
        }

        static int returnID(string opcion)
        {
            int id = 0;
            string[] datos = new string[2];

            datos = opcion.Split('-');

            id = int.Parse(datos[0]);

            return id;
        }
    }
}