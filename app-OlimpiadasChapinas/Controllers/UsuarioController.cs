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
using static app_OlimpiadasChapinas.Models.csEstructuraUsuario;

namespace app_OlimpiadasChapinas.Controllers
{
    public class UsuarioController : Controller
    {
        // GET: Usuario
        public ActionResult Usuario(string idUsuario, string email)
        {
            DataSet data = new DataSet();
            var url = ""; 
            if (string.IsNullOrEmpty(idUsuario) && string.IsNullOrEmpty(email))
            {
                url = $"http://localhost/api-OlimpiadasChapinas/rest/api/ListarUsuario";
            }
            else if (string.IsNullOrEmpty(email))
            {
                url = $"http://localhost/api-OlimpiadasChapinas/rest/api/ListarUsuarioPorID?idUsuario={idUsuario}";
            }
            else if (string.IsNullOrEmpty(idUsuario))
            {
                url = $"http://localhost/api-OlimpiadasChapinas/rest/api/ListarUsuarioPorEmail?email={email}";
            } else
            {
                url = $"http://localhost/api-OlimpiadasChapinas/rest/api/ListarUsuario";
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

        public ActionResult newUsuario()
        {
            return View();
        }

        public ActionResult Guardar(FormCollection form)
        {
            string json = "";
            string resultJson = "";
            Byte[] reqByte, resByte;
            requestUsuario InsertarUsuario = new requestUsuario();
            InsertarUsuario.nombre = form["nombre"];
            InsertarUsuario.apellido = form["apellido"];
            InsertarUsuario.email = form["email"];
            InsertarUsuario.contraseña_hash = form["contraseña_hash"];
            InsertarUsuario.telefono = form["telefono"];
            InsertarUsuario.DNI = form["DNI"];

            json = JsonConvert.SerializeObject(InsertarUsuario);

            WebClient client = new WebClient();
            string url = $"http://localhost/api-OlimpiadasChapinas/rest/api/InsertarUsuario";
            var request = (HttpWebRequest)WebRequest.Create(url);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            client.Headers["content-type"] = "application/json";

            reqByte = Encoding.UTF8.GetBytes(json);
            resByte = client.UploadData(request.Address.ToString(), "post", reqByte);
            resultJson = Encoding.UTF8.GetString(resByte);

            responseUsuario result = new responseUsuario();
            result = JsonConvert.DeserializeObject<responseUsuario>(resultJson);
            client.Dispose();

            if (result.respuesta == 1) return RedirectToAction("Usuario", "Usuario");
            else return RedirectToAction("newUsuario", "Usuario");
        }

        public ActionResult ActualizarUsuario(string email)
        {
            DataSet data = new DataSet();
            var url = $"http://localhost/api-OlimpiadasChapinas/rest/api/ListarUsuarioPorEmail?email={email}";

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
            requestUsuario ActualizarUsuario = new requestUsuario();
            ActualizarUsuario.nombre = form["nombre"];
            ActualizarUsuario.apellido = form["apellido"];
            ActualizarUsuario.email = form["email"];
            ActualizarUsuario.contraseñaAlmacenada = form["contraseñaAlmacenada"];
            ActualizarUsuario.contraseñaActualizada = form["contraseñaActualizada"];
            ActualizarUsuario.telefono = form["telefono"];

            json = JsonConvert.SerializeObject(ActualizarUsuario);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost/api-OlimpiadasChapinas/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var jsonContent = new StringContent(JsonConvert.SerializeObject(ActualizarUsuario), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("rest/api/ActualizarUsuario", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    resultJson = await response.Content.ReadAsStringAsync();
                    responseUsuario result = JsonConvert.DeserializeObject<responseUsuario>(resultJson);
                    if (result.respuesta == 1)
                        return RedirectToAction("Usuario", "Usuario");
                }

                return RedirectToAction("ActualizarUsuario", "Usuario");
            }
        }

        public ActionResult Eliminar(string email)
        {
            string json = "";
            string resultJson = "";
            Byte[] reqByte, resByte;

            WebClient client = new WebClient();
            string url = $"http://localhost/api-OlimpiadasChapinas/rest/api/EliminarUsuario";
            var request = (HttpWebRequest)WebRequest.Create(url);

            requestUsuario eliminar = new requestUsuario();
            eliminar.email = email;

            json = JsonConvert.SerializeObject(eliminar);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            client.Headers["content-type"] = "application/json";

            reqByte = Encoding.UTF8.GetBytes(json);
            resByte = client.UploadData(request.Address.ToString(), "post", reqByte);
            resultJson = Encoding.UTF8.GetString(resByte);

            responseUsuario result = new responseUsuario();
            result = JsonConvert.DeserializeObject<responseUsuario>(resultJson);
            client.Dispose();

            return RedirectToAction("Usuario", "Usuario");
        }
    }
}