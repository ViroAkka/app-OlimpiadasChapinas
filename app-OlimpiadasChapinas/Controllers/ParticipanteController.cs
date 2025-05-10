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
using static app_OlimpiadasChapinas.Models.csEstructuraParticipante;
using app_OlimpiadasChapinas.Models;

namespace app_OlimpiadasChapinas.Controllers
{
    public class ParticipanteController : Controller
    {
        // GET: Participante
        public ActionResult Participante(string idParticipante)
        {
            DataSet data = new DataSet();
            var url = string.IsNullOrEmpty(idParticipante)
                ? $"http://localhost/api-OlimpiadasChapinas/rest/api/ListarParticipante"
                : $"http://localhost/api-OlimpiadasChapinas/rest/api/ListarParticipantePorID?idParticipante={idParticipante}";

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

        public ActionResult newParticipante()
        {
            DataSet paises = new DataSet();
            DataSet usuarios = new DataSet();

            string urlPaises = $"http://localhost/api-OlimpiadasChapinas/rest/api/ListarPais";
            string urlUsuarios = $"http://localhost/api-OlimpiadasChapinas/rest/api/ListarUsuario";

            try
            {
                paises = createDataSet(urlPaises);
                usuarios = createDataSet(urlUsuarios);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocurrió un error: {ex.Message}");
            }

            csDataSets model = new csDataSets
            {
                Pais = paises,
                Usuario = usuarios
            };

            return View(model);
        }

        public ActionResult Guardar(FormCollection form)
        {
            string json = "";
            string resultJson = "";
            Byte[] reqByte, resByte;
            requestParticipante InsertarParticipante = new requestParticipante();
            InsertarParticipante.idPais = returnIDPais(form["idPais"]);
            InsertarParticipante.idUsuario = returnID(form["idUsuario"]);
            InsertarParticipante.fechaNacimiento = form["fechaNacimiento"];
            InsertarParticipante.altura = double.Parse(form["altura"]);
            InsertarParticipante.peso = double.Parse(form["peso"]);
            InsertarParticipante.genero = form["genero"];

            json = JsonConvert.SerializeObject(InsertarParticipante);

            WebClient client = new WebClient();
            string url = $"http://localhost/api-OlimpiadasChapinas/rest/api/InsertarParticipante";
            var request = (HttpWebRequest)WebRequest.Create(url);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            client.Headers["content-type"] = "application/json";

            reqByte = Encoding.UTF8.GetBytes(json);
            resByte = client.UploadData(request.Address.ToString(), "post", reqByte);
            resultJson = Encoding.UTF8.GetString(resByte);

            responseParticipante result = new responseParticipante();
            result = JsonConvert.DeserializeObject<responseParticipante>(resultJson);
            client.Dispose();

            if (result.respuesta == 1) return RedirectToAction("Participante", "Participante");
            else return RedirectToAction("newParticipante", "Participante");
        }

        public ActionResult ActualizarParticipante(int idParticipante)
        {
            DataSet data = new DataSet();
            DataSet paises = new DataSet();
            DataSet usuarios = new DataSet();

            var url = $"http://localhost/api-OlimpiadasChapinas/rest/api/ListarParticipantePorID?idParticipante={idParticipante}";
            var urlPaises = $"http://localhost/api-OlimpiadasChapinas/rest/api/ListarPais";
            var urlUsuarios = $"http://localhost/api-OlimpiadasChapinas/rest/api/ListarUsuario";

            try
            {
                data = createDataSet(url);
                paises = createDataSet(urlPaises);
                usuarios = createDataSet(urlUsuarios);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocurrió un error: {ex.Message}");
            }

            csDataSets model = new csDataSets
            {
                Data = data,
                Pais = paises,
                Usuario = usuarios
            };

            return View(model);
        }

        public async Task<ActionResult> Actualizar(FormCollection form)
        {
            string json = "";
            string resultJson = "";
            requestParticipante ActualizarParticipante = new requestParticipante();
            ActualizarParticipante.idParticipante = returnID(form["idParticipante"]);
            ActualizarParticipante.idPais = returnIDPais(form["idPais"]);
            ActualizarParticipante.idUsuario = returnID(form["idUsuario"]);
            ActualizarParticipante.fechaNacimiento = form["fechaNacimiento"];
            ActualizarParticipante.altura = double.Parse(form["altura"]);
            ActualizarParticipante.peso = double.Parse(form["peso"]);
            ActualizarParticipante.genero = form["genero"];

            json = JsonConvert.SerializeObject(ActualizarParticipante);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost/api-OlimpiadasChapinas/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var jsonContent = new StringContent(JsonConvert.SerializeObject(ActualizarParticipante), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("rest/api/ActualizarParticipante", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    resultJson = await response.Content.ReadAsStringAsync();
                    responseParticipante result = JsonConvert.DeserializeObject<responseParticipante>(resultJson);
                    if (result.respuesta == 1)
                        return RedirectToAction("Participante", "Participante");
                }

                return RedirectToAction("ActualizarParticipante", "Participante");
            }
        }

        public ActionResult Eliminar(string idParticipante)
        {
            string json = "";
            string resultJson = "";
            Byte[] reqByte, resByte;

            WebClient client = new WebClient();
            string url = $"http://localhost/api-OlimpiadasChapinas/rest/api/EliminarParticipante";
            var request = (HttpWebRequest)WebRequest.Create(url);

            requestParticipante eliminar = new requestParticipante();
            eliminar.idParticipante = int.Parse(idParticipante);

            json = JsonConvert.SerializeObject(eliminar);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            client.Headers["content-type"] = "application/json";

            reqByte = Encoding.UTF8.GetBytes(json);
            resByte = client.UploadData(request.Address.ToString(), "post", reqByte);
            resultJson = Encoding.UTF8.GetString(resByte);

            responseParticipante result = new responseParticipante();
            result = JsonConvert.DeserializeObject<responseParticipante>(resultJson);
            client.Dispose();

            return RedirectToAction("Participante", "Participante");
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

        static string returnIDPais(string opcion)
        {
            string id = "";
            string[] datos = new string[2];

            datos = opcion.Split('-');

            id = datos[0];

            return id;
        }
    }
}