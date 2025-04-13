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
using static app_OlimpiadasChapinas.Models.csEstructuraInscripcion;

namespace app_OlimpiadasChapinas.Controllers
{
    public class InscripcionController : Controller
    {
        // GET: Inscripcion
        public ActionResult Inscripcion(string idEvento, string idParticipante, string idPago)
        {
            DataSet data = new DataSet();
            var url = "";

            if (string.IsNullOrEmpty(idEvento) & string.IsNullOrEmpty(idParticipante) & string.IsNullOrEmpty(idPago))
            {
                url = $"http://localhost/api-OlimpiadasChapinas/rest/api/ListarInscripcion";
            }
            else if (string.IsNullOrEmpty(idParticipante) & string.IsNullOrEmpty(idPago))
            {
                url = $"http://localhost/api-OlimpiadasChapinas/rest/api/ListarInscripcionPorEvento?idEvento={idEvento}";
            }
            else if (string.IsNullOrEmpty(idEvento) & string.IsNullOrEmpty(idPago))
            {
                url = $"http://localhost/api-OlimpiadasChapinas/rest/api/ListarInscripcionPorParticipante?idParticipante={idParticipante}";
            }
            else if (string.IsNullOrEmpty(idEvento) & string.IsNullOrEmpty(idParticipante))
            {
                url = $"http://localhost/api-OlimpiadasChapinas/rest/api/ListarInscripcionPorPago?idPago={idPago}";
            }
            else if (string.IsNullOrEmpty(idPago))
            {
                url = $"http://localhost/api-OlimpiadasChapinas/rest/api/ListarInscripcionPorEventoParticipante?idEvento={idEvento}&idParticipante={idParticipante}";
            }
            else if (string.IsNullOrEmpty(idParticipante))
            {
                url = $"http://localhost/api-OlimpiadasChapinas/rest/api/ListarInscripcionPorEventoPago?idEvento={idEvento}&idPago={idPago}";
            }
            else if (string.IsNullOrEmpty(idEvento))
            {
                url = $"http://localhost/api-OlimpiadasChapinas/rest/api/ListarInscripcionPorParticipantePago?idParticipante={idParticipante}&idPago={idPago}";
            }
            else
            {
                url = $"http://localhost/api-OlimpiadasChapinas/rest/api/ListarInscripcionPorID?idEvento={idEvento}&idParticipante={idParticipante}&idPago={idPago}";
            }

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

        public ActionResult newInscripcion()
        {
            return View();
        }

        public ActionResult Guardar(FormCollection form)
        {
            string json = "";
            string resultJson = "";
            Byte[] reqByte, resByte;
            requestInscripcion InsertarInscripcion = new requestInscripcion();
            InsertarInscripcion.idEvento = int.Parse(form["idEvento"]);
            InsertarInscripcion.idParticipante = int.Parse(form["idParticipante"]);
            InsertarInscripcion.idPago = int.Parse(form["idPago"]);
            InsertarInscripcion.fuentePublicidad = form["fuentePublicidad"];

            json = JsonConvert.SerializeObject(InsertarInscripcion);

            WebClient client = new WebClient();
            string url = $"http://localhost/api-OlimpiadasChapinas/rest/api/InsertarInscripcion";
            var request = (HttpWebRequest)WebRequest.Create(url);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            client.Headers["content-type"] = "application/json";

            reqByte = Encoding.UTF8.GetBytes(json);
            resByte = client.UploadData(request.Address.ToString(), "post", reqByte);
            resultJson = Encoding.UTF8.GetString(resByte);

            responseInscripcion result = new responseInscripcion();
            result = JsonConvert.DeserializeObject<responseInscripcion>(resultJson);
            client.Dispose();

            if (result.respuesta == 1) return RedirectToAction("Inscripcion", "Inscripcion");
            else return RedirectToAction("newInscripcion", "Inscripcion");
        }

        public ActionResult ActualizarInscripcion(int idEvento, int idParticipante, int idPago)
        {
            DataSet data = new DataSet();
            var url = $"http://localhost/api-OlimpiadasChapinas/rest/api/ListarInscripcionPorID?idEvento={idEvento}&idParticipante={idParticipante}&idPago={idPago}";

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

            requestInscripcion ActualizarInscripcion= new requestInscripcion();
            ActualizarInscripcion.idEvento = int.Parse(form["idEvento"]);
            ActualizarInscripcion.idParticipante = int.Parse(form["idParticipante"]);
            ActualizarInscripcion.idPago = int.Parse(form["idPago"]);
            ActualizarInscripcion.idEventoActualizado = int.Parse(form["idEventoActualizado"]);
            ActualizarInscripcion.idParticipanteActualizado = int.Parse(form["idParticipanteActualizado"]);
            ActualizarInscripcion.idPagoActualizado = int.Parse(form["idPagoActualizado"]);
            ActualizarInscripcion.fuentePublicidad = form["fuentePublicidad"];

            json = JsonConvert.SerializeObject(ActualizarInscripcion);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost/api-OlimpiadasChapinas/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var jsonContent = new StringContent(JsonConvert.SerializeObject(ActualizarInscripcion), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("rest/api/ActualizarInscripcion", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    resultJson = await response.Content.ReadAsStringAsync();
                    responseInscripcion result = JsonConvert.DeserializeObject<responseInscripcion>(resultJson);
                    if (result.respuesta == 1)
                        return RedirectToAction("Inscripcion", "Inscripcion");
                } 
                return RedirectToAction("ActualizarInscripcion", "Inscripcion");
            }
        }

        public ActionResult Eliminar(int idEvento, int idParticipante, int idPago)
        {
            string json = "";
            string resultJson = "";
            Byte[] reqByte, resByte;

            WebClient client = new WebClient();
            string url = $"http://localhost/api-OlimpiadasChapinas/rest/api/EliminarInscripcion";
            var request = (HttpWebRequest)WebRequest.Create(url);

            requestInscripcion eliminar = new requestInscripcion
            {
                idEvento = idEvento,
                idParticipante = idParticipante,
                idPago = idPago
            };

            json = JsonConvert.SerializeObject(eliminar);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            client.Headers["content-type"] = "application/json";

            reqByte = Encoding.UTF8.GetBytes(json);
            resByte = client.UploadData(request.Address.ToString(), "post", reqByte);
            resultJson = Encoding.UTF8.GetString(resByte);

            responseInscripcion result = new responseInscripcion();
            result = JsonConvert.DeserializeObject<responseInscripcion>(resultJson);
            client.Dispose();

            return RedirectToAction("Inscripcion", "Inscripcion");
        }
    }
}