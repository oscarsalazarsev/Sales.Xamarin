using System;
using System.Threading.Tasks;
using System.Net.Http;
using Sales.Common.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Sales.Services
{
    public class ApiServices
    {
        public async Task<Response> GetList<T>(string urlBase, string prefix, string controller)
        {
            try
            {
                /*Para consumir un Servicio RESFull*/
                /*Paso 1: Vamos a crear un cliente como un nuevo objeto <HttpClient()> para establecer la comunicación*/
                var client = new HttpClient();
                /*Paso 2: Cargamos la dirección base (urlBase) al Cliente, antes creado*/
                client.BaseAddress = new Uri(urlBase);
                /*Paso 3: Ahora contatenamos el Prefijo y el Controlador para poder acceder al metodo.*/
                var url = $"{prefix}`{controller}"; // Esto es equivalente a: var url = String.Format("{0}{1}", prefix, controller);
                /*Paso 4: Creamos un objeto para capturar la respuesta obtenida por el Servicio RESFull, dicha respuesta puede tardar por eso le colocamos un <await> y ejecutamos el <GetAsync> del cliente antes creado y le pasamos el url complementario (Prefijo y Controlador).*/
                var response = await client.GetAsync(url);
                /*Paso 5: En la linea anterior establecimos la comunicación y ejecutamos el Metodo; ahora debemos obtener el resultado contenido en el Objeto antes creado (<response>); esto puede ser cualquier cosa (Stream, String, etc.) pero en este caso lo que esperamos es un JSON que viene contenido en el objeto <response> lo obtenermos con el metodo (Content.ReadAsStringAsync())*/
                var answer = await response.Content.ReadAsStringAsync();
                /*Paso 6: Debemos validar si la ejecución fue exitosa o no.*/
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,
                    };
                }
                /*Paso 7: Si la Ejecución fue exitosa debemos Serializar(de Objeto a String) o Deserializar(de String a Objeto) el resultado.*/
                var list = JsonConvert.DeserializeObject<List<T>>(answer);
                /*Paso 8: Devolvemos el Objeto <Response> con la variable <IsSuccess> en <True> y en el <Result> la lista Deserializada de la Respuesta.*/
                return new Response
                {
                    IsSuccess = true,
                    Result = list,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message.ToString(),
                };
            }
        }
    }
}
