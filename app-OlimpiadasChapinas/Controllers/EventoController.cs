using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using static app_OlimpiadasChapinas.Models.csEstructuraEvento;
using System.Threading.Tasks;
using app_OlimpiadasChapinas.Models;

namespace app_OlimpiadasChapinas.Controllers
{
    public class EventoController : Controller
    {
        // GET: Evento
        public ActionResult Evento(string idEvento)
        {
            DataSet data = new DataSet();
            var url = string.IsNullOrEmpty(idEvento) 
                ? $"http://localhost/api-OlimpiadasChapinas/rest/api/ListarEvento" 
                : $"http://localhost/api-OlimpiadasChapinas/rest/api/ListarEventoPorID?idEvento={idEvento}";

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

        public ActionResult newEvento()
        {
            DataSet deportes = new DataSet();
            DataSet eventos = new DataSet();

            string urlDeportes = $"http://localhost/api-OlimpiadasChapinas/rest/api/ListarDeporte";
            string urlEventos = $"http://localhost/api-OlimpiadasChapinas/rest/api/ListarEvento";
               
            try
            {
                deportes = createDataSet(urlDeportes);

                eventos = createDataSet(urlEventos);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocurrió un error: {ex.Message}");
            }

            var model = new csDataSets
            {
                Deporte = deportes,
                Evento = eventos
            };

            return View(model);
        }

        public ActionResult Guardar(FormCollection form)
        {
            string json = "";
            string resultJson = "";
            Byte[] reqByte, resByte;
            requestEvento InsertarEvento = new requestEvento();
            InsertarEvento.idDeporte = returnID(form["idDeporte"]);
            InsertarEvento.idEventoPadre = returnID(form["idEventoPadre"]);
            InsertarEvento.nombre = form["nombre"];
            InsertarEvento.fechaInicio = form["fechaInicio"];
            InsertarEvento.fechaFin = form["fechaFin"];
            InsertarEvento.cantidadParticipantes= int.Parse(form["cantidadParticipantes"]);
            InsertarEvento.montoInscripcion = double.Parse(form["montoInscripcion"]);

            json = JsonConvert.SerializeObject(InsertarEvento);

            WebClient client = new WebClient();
            string url = $"http://localhost/api-OlimpiadasChapinas/rest/api/InsertarEvento";
            var request = (HttpWebRequest)WebRequest.Create(url);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            client.Headers["content-type"] = "application/json";

            reqByte = Encoding.UTF8.GetBytes(json);
            resByte = client.UploadData(request.Address.ToString(), "post", reqByte);
            resultJson = Encoding.UTF8.GetString(resByte);

            responseEvento result = new responseEvento();
            result = JsonConvert.DeserializeObject<responseEvento>(resultJson);
            client.Dispose();

            if (result.respuesta == 1) return RedirectToAction("Evento", "Evento");
            else return RedirectToAction("newEvento", "Evento");
        }

        public ActionResult ActualizarEvento(string idEvento)
        {
            DataSet data = new DataSet();
            DataSet deportes = new DataSet();
            DataSet eventos = new DataSet();

            var url = $"http://localhost/api-OlimpiadasChapinas/rest/api/listarEventoPorID?idEvento={idEvento}";

            string urlDeportes = $"http://localhost/api-OlimpiadasChapinas/rest/api/listarDeporte";
            string urlEventos= $"http://localhost/api-OlimpiadasChapinas/rest/api/listarEvento";

            try
            {
                data = createDataSet(url);
                deportes = createDataSet(urlDeportes);
                eventos = createDataSet(urlEventos);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocurrió un error: {ex.Message}");
            }

            var model = new csDataSets
            {
                Data = data,
                Deporte = deportes,
                Evento = eventos
            };

            return View(model);
        }

        public async Task<ActionResult> Actualizar(FormCollection form)
        {
            string json = "";
            string resultJson = "";
            requestEvento ActualizarEvento = new requestEvento();
            ActualizarEvento.idEvento = int.Parse(form["idEvento"]);
            ActualizarEvento.idDeporte = returnID(form["idDeporte"]);
            ActualizarEvento.idEventoPadre = returnID(form["idEventoPadre"]);
            ActualizarEvento.nombre = form["nombre"];
            ActualizarEvento.fechaInicio = form["fechaInicio"];
            ActualizarEvento.fechaFin = form["fechaFin"];
            ActualizarEvento.cantidadParticipantes = int.Parse(form["cantidadParticipantes"]);
            ActualizarEvento.montoInscripcion = double.Parse(form["montoInscripcion"]);

            json = JsonConvert.SerializeObject(ActualizarEvento);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost/api-OlimpiadasChapinas/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var jsonContent = new StringContent(JsonConvert.SerializeObject(ActualizarEvento), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("rest/api/ActualizarEvento", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    resultJson = await response.Content.ReadAsStringAsync();
                    responseEvento result = JsonConvert.DeserializeObject<responseEvento>(resultJson);
                    if (result.respuesta == 1)
                        return RedirectToAction("Evento", "Evento");
                }

                return RedirectToAction("ActualizarEvento", "Evento");
            }
        }

        public ActionResult Eliminar(string idEvento)
        {
            string json = "";
            string resultJson = "";
            Byte[] reqByte, resByte;

            WebClient client = new WebClient();
            string url = $"http://localhost/api-OlimpiadasChapinas/rest/api/EliminarEvento";
            var request = (HttpWebRequest)WebRequest.Create(url);

            requestEvento eliminar = new requestEvento();
            eliminar.idEvento = int.Parse(idEvento);

            json = JsonConvert.SerializeObject(eliminar);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            client.Headers["content-type"] = "application/json";

            reqByte = Encoding.UTF8.GetBytes(json);
            resByte = client.UploadData(request.Address.ToString(), "post", reqByte);
            resultJson = Encoding.UTF8.GetString(resByte);

            responseEvento result = new responseEvento();
            result = JsonConvert.DeserializeObject<responseEvento>(resultJson);
            client.Dispose();

            return RedirectToAction("Evento", "Evento");
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

