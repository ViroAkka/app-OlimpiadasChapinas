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
using static app_OlimpiadasChapinas.Models.csEstructuraPremiacion;
using app_OlimpiadasChapinas.Models;

namespace app_OlimpiadasChapinas.Controllers
{
    public class PremiacionController : Controller
    {
        // GET: Premiacion
        public ActionResult Premiacion(string idPremiacion)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("loginUsuario", "Usuario");
            }

            DataSet data = new DataSet();
            var url = string.IsNullOrEmpty(idPremiacion)
                ? $"http://localhost/api-OlimpiadasChapinas/rest/api/ListarPremiacion"
                : $"http://localhost/api-OlimpiadasChapinas/rest/api/ListarPremiacionPorID?idPremiacion={idPremiacion}";

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

        public ActionResult newPremiacion()
        {
            DataSet eventos = new DataSet();
            DataSet puestos = new DataSet();
            DataSet participantes = new DataSet();
            DataSet usuarios = new DataSet();

            string urlEventos = $"http://localhost/api-OlimpiadasChapinas/rest/api/ListarEvento";
            string urlPuestos = $"http://localhost/api-OlimpiadasChapinas/rest/api/ListarPuestoPremiacion";
            string urlParticipantes = $"http://localhost/api-OlimpiadasChapinas/rest/api/ListarParticipante";
            string urlUsuarios = $"http://localhost/api-OlimpiadasChapinas/rest/api/ListarUsuario";

            try
            {
                eventos = createDataSet(urlEventos);
                puestos = createDataSet(urlPuestos);
                participantes = createDataSet(urlParticipantes);
                usuarios = createDataSet(urlUsuarios);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocurrió un error: {ex.Message}");
            }

            csDataSets model = new csDataSets
            {
                Evento = eventos,
                PuestoPremiacion = puestos,
                Participante = participantes,
                Usuario = usuarios
            };

            return View(model);
        }

        public ActionResult Guardar(FormCollection form)
        {
            string json = "";
            string resultJson = "";
            Byte[] reqByte, resByte;
            requestPremiacion InsertarPremiacion = new requestPremiacion();
            InsertarPremiacion.idEvento = returnID(form["idEvento"]);
            InsertarPremiacion.idPuesto = returnID(form["idPuesto"]);
            InsertarPremiacion.idParticipante = returnID(form["idParticipante"]);

            json = JsonConvert.SerializeObject(InsertarPremiacion);

            WebClient client = new WebClient();
            string url = $"http://localhost/api-OlimpiadasChapinas/rest/api/InsertarPremiacion";
            var request = (HttpWebRequest)WebRequest.Create(url);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            client.Headers["content-type"] = "application/json";

            reqByte = Encoding.UTF8.GetBytes(json);
            resByte = client.UploadData(request.Address.ToString(), "post", reqByte);
            resultJson = Encoding.UTF8.GetString(resByte);

            responsePremiacion result = new responsePremiacion();
            result = JsonConvert.DeserializeObject<responsePremiacion>(resultJson);
            client.Dispose();

            if (result.respuesta == 1) return RedirectToAction("Premiacion", "Premiacion");
            else return RedirectToAction("newPremiacion", "Premiacion");
        }

        public ActionResult ActualizarPremiacion(string idPremiacion)
        {
            DataSet data = new DataSet();
            DataSet eventos = new DataSet();
            DataSet puestos = new DataSet();
            DataSet participantes = new DataSet();
            DataSet usuarios = new DataSet();

            var url = $"http://localhost/api-OlimpiadasChapinas/rest/api/ListarPremiacionPorID?idPremiacion={idPremiacion}";
            string urlEventos = $"http://localhost/api-OlimpiadasChapinas/rest/api/ListarEvento";
            string urlPuestos = $"http://localhost/api-OlimpiadasChapinas/rest/api/ListarPuestoPremiacion";
            string urlParticipantes = $"http://localhost/api-OlimpiadasChapinas/rest/api/ListarParticipante";
            string urlUsuarios = $"http://localhost/api-OlimpiadasChapinas/rest/api/ListarUsuario";

            try
            {
                data = createDataSet(url);
                eventos = createDataSet(urlEventos);
                puestos = createDataSet(urlPuestos);
                participantes = createDataSet(urlParticipantes);
                usuarios = createDataSet(urlUsuarios);                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocurrió un error: {ex.Message}");
            }

            csDataSets model = new csDataSets
            {
                Data = data,
                Evento = eventos,
                PuestoPremiacion = puestos,
                Participante = participantes,
                Usuario = usuarios
            };

            return View(model);
        }

        public async Task<ActionResult> Actualizar(FormCollection form)
        {
            string json = "";
            string resultJson = "";
            requestPremiacion ActualizarPremiacion = new requestPremiacion();
            ActualizarPremiacion.idPremiacion = int.Parse(form["idPremiacion"]);
            ActualizarPremiacion.idEvento = returnID(form["idEvento"]);
            ActualizarPremiacion.idPuesto = returnID(form["idPuesto"]);
            ActualizarPremiacion.idParticipante = returnID(form["idParticipante"]);

            json = JsonConvert.SerializeObject(ActualizarPremiacion);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost/api-OlimpiadasChapinas/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var jsonContent = new StringContent(JsonConvert.SerializeObject(ActualizarPremiacion), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("rest/api/ActualizarPremiacion", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    resultJson = await response.Content.ReadAsStringAsync();
                    responsePremiacion result = JsonConvert.DeserializeObject<responsePremiacion>(resultJson);
                    if (result.respuesta == 1)
                        return RedirectToAction("Premiacion", "Premiacion");
                }

                return RedirectToAction("ActualizarPremiacion", "Premiacion");
            }
        }

        public ActionResult Eliminar(string idPremiacion)
        {
            string json = "";
            string resultJson = "";
            Byte[] reqByte, resByte;

            WebClient client = new WebClient();
            string url = $"http://localhost/api-OlimpiadasChapinas/rest/api/EliminarPremiacion";
            var request = (HttpWebRequest)WebRequest.Create(url);

            requestPremiacion eliminar = new requestPremiacion();
            eliminar.idPremiacion = int.Parse(idPremiacion);

            json = JsonConvert.SerializeObject(eliminar);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            client.Headers["content-type"] = "application/json";

            reqByte = Encoding.UTF8.GetBytes(json);
            resByte = client.UploadData(request.Address.ToString(), "post", reqByte);
            resultJson = Encoding.UTF8.GetString(resByte);

            responsePremiacion result = new responsePremiacion();
            result = JsonConvert.DeserializeObject<responsePremiacion>(resultJson);
            client.Dispose();

            return RedirectToAction("Premiacion", "Premiacion");
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