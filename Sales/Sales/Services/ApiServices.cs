using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Sales.Common.Models;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Sales.Helpers;

namespace Sales.Services
{
    public class ApiServices
    {
        public async Task<Response> CheckConnection()
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = Languages.TurnOnInternet,
                };
            }

            //var urlTest = Application.Current.Resources["UrlTest"].ToString();
            var urlTest = "google.com";
            var isReachable = await CrossConnectivity.Current.IsRemoteReachable(urlTest);
            if (!isReachable)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = Languages.NoInternet,
                };
            }

            return new Response
            {
                IsSuccess = true,
            };
        }

        public async Task<TokenResponse> GetToken(string urlBase, string username, string password)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var response = await client.PostAsync(
                    "/Token",
                    new StringContent($"grant_type=password&userName={username}&password={password}", Encoding.UTF8, "application/x-www-form-urlencoded"));
                var resultJSON = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<TokenResponse>(resultJSON);
                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }

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
                var url = $"{prefix}{controller}"; // Esto es equivalente a: var url = String.Format("{0}{1}", prefix, controller);
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

        public async Task<Response> GetList<T>(string urlBase, string prefix, string controller, string tokenType, string accessToken)
        {
            try
            {
                /*Para consumir un Servicio RESFull*/
                /*Paso 1: Vamos a crear un cliente como un nuevo objeto <HttpClient()> para establecer la comunicación*/
                var client = new HttpClient();
                /*Paso 2: Cargamos la dirección base (urlBase) al Cliente, antes creado*/
                client.BaseAddress = new Uri(urlBase);
                /*Paso 3: Consumir un servicio de forma segura*/
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                /*Paso 4: Ahora contatenamos el Prefijo y el Controlador para poder acceder al metodo.*/
                var url = $"{prefix}{controller}"; // Esto es equivalente a: var url = String.Format("{0}{1}", prefix, controller);
                /*Paso 5: Creamos un objeto para capturar la respuesta obtenida por el Servicio RESFull, dicha respuesta puede tardar por eso le colocamos un <await> y ejecutamos el <GetAsync> del cliente antes creado y le pasamos el url complementario (Prefijo y Controlador).*/
                var response = await client.GetAsync(url);
                /*Paso 6: En la linea anterior establecimos la comunicación y ejecutamos el Metodo; ahora debemos obtener el resultado contenido en el Objeto antes creado (<response>); esto puede ser cualquier cosa (Stream, String, etc.) pero en este caso lo que esperamos es un JSON que viene contenido en el objeto <response> lo obtenermos con el metodo (Content.ReadAsStringAsync())*/
                var answer = await response.Content.ReadAsStringAsync();
                /*Paso 7: Debemos validar si la ejecución fue exitosa o no.*/
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,
                    };
                }
                /*Paso 8: Si la Ejecución fue exitosa debemos Serializar(de Objeto a String) o Deserializar(de String a Objeto) el resultado.*/
                var list = JsonConvert.DeserializeObject<List<T>>(answer);
                /*Paso 9: Devolvemos el Objeto <Response> con la variable <IsSuccess> en <True> y en el <Result> la lista Deserializada de la Respuesta.*/
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

        public async Task<Response> Post<T>(string urlBase, string prefix, string controller, T model)
        {
            try
            {
                /*Para el Metodo POST traigo todo el Codigo del GET y le agrego estas Lineas al comienzo, para serializar el modelo enviado en los parametros*/
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                /*Para consumir un Servicio RESFull*/
                /*Paso 1: Vamos a crear un cliente como un nuevo objeto <HttpClient()> para establecer la comunicación*/
                var client = new HttpClient();
                /*Paso 2: Cargamos la dirección base (urlBase) al Cliente, antes creado*/
                client.BaseAddress = new Uri(urlBase);
                /*Paso 3: Ahora contatenamos el Prefijo y el Controlador para poder acceder al metodo.*/
                var url = $"{prefix}{controller}"; // Esto es equivalente a: var url = String.Format("{0}{1}", prefix, controller);
                /*Paso 4: Creamos un objeto para capturar la respuesta obtenida por el Servicio RESFull, dicha respuesta puede tardar por eso le colocamos un <await> y ejecutamos el <GetAsync> del cliente antes creado y le pasamos el url complementario (Prefijo y Controlador).*/
                var response = await client.PostAsync(url, content);
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
                var obj = JsonConvert.DeserializeObject<T>(answer);
                /*Paso 8: Devolvemos el Objeto <Response> con la variable <IsSuccess> en <True> y en el <Result> la lista Deserializada de la Respuesta.*/
                return new Response
                {
                    IsSuccess = true,
                    Result = obj,
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

        public async Task<Response> Post<T>(string urlBase, string prefix, string controller, T model, string tokenType, string accessToken)
        {
            try
            {
                /*Para el Metodo POST traigo todo el Codigo del GET y le agrego estas Lineas al comienzo, para serializar el modelo enviado en los parametros*/
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                /*Para consumir un Servicio RESFull*/
                /*Paso 1: Vamos a crear un cliente como un nuevo objeto <HttpClient()> para establecer la comunicación*/
                var client = new HttpClient();
                /*Paso 2: Cargamos la dirección base (urlBase) al Cliente, antes creado*/
                client.BaseAddress = new Uri(urlBase);
                /*Paso 3: Consumir un servicio de forma segura*/
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                /*Paso 4: Ahora contatenamos el Prefijo y el Controlador para poder acceder al metodo.*/
                var url = $"{prefix}{controller}"; // Esto es equivalente a: var url = String.Format("{0}{1}", prefix, controller);
                /*Paso 5: Creamos un objeto para capturar la respuesta obtenida por el Servicio RESFull, dicha respuesta puede tardar por eso le colocamos un <await> y ejecutamos el <GetAsync> del cliente antes creado y le pasamos el url complementario (Prefijo y Controlador).*/
                var response = await client.PostAsync(url, content);
                /*Paso 6: En la linea anterior establecimos la comunicación y ejecutamos el Metodo; ahora debemos obtener el resultado contenido en el Objeto antes creado (<response>); esto puede ser cualquier cosa (Stream, String, etc.) pero en este caso lo que esperamos es un JSON que viene contenido en el objeto <response> lo obtenermos con el metodo (Content.ReadAsStringAsync())*/
                var answer = await response.Content.ReadAsStringAsync();
                /*Paso 7: Debemos validar si la ejecución fue exitosa o no.*/
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,
                    };
                }
                /*Paso 8: Si la Ejecución fue exitosa debemos Serializar(de Objeto a String) o Deserializar(de String a Objeto) el resultado.*/
                var obj = JsonConvert.DeserializeObject<T>(answer);
                /*Paso 9: Devolvemos el Objeto <Response> con la variable <IsSuccess> en <True> y en el <Result> la lista Deserializada de la Respuesta.*/
                return new Response
                {
                    IsSuccess = true,
                    Result = obj,
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

        public async Task<Response> Delete(string urlBase, string prefix, string controller, int id)
        {
            try
            {
                /*Para consumir un Servicio RESFull*/
                /*Paso 1: Vamos a crear un cliente como un nuevo objeto <HttpClient()> para establecer la comunicación*/
                var client = new HttpClient();
                /*Paso 2: Cargamos la dirección base (urlBase) al Cliente, antes creado*/
                client.BaseAddress = new Uri(urlBase);
                /*Paso 3: Ahora contatenamos el Prefijo y el Controlador para poder acceder al metodo.*/
                var url = $"{prefix}{controller}/{id}"; // Esto es equivalente a: var url = String.Format("{0}{1}", prefix, controller);
                /*Paso 4: Creamos un objeto para capturar la respuesta obtenida por el Servicio RESFull, dicha respuesta puede tardar por eso le colocamos un <await> y ejecutamos el <GetAsync> del cliente antes creado y le pasamos el url complementario (Prefijo y Controlador).*/
                var response = await client.DeleteAsync(url);
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
                /*Paso 7: Devolvemos el Objeto <Response> con la variable <IsSuccess> en <True> y en el <Result> la lista Deserializada de la Respuesta.*/
                return new Response
                {
                    IsSuccess = true,
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

        public async Task<Response> Delete(string urlBase, string prefix, string controller, int id, string tokenType, string accessToken)
        {
            try
            {
                /*Para consumir un Servicio RESFull*/
                /*Paso 1: Vamos a crear un cliente como un nuevo objeto <HttpClient()> para establecer la comunicación*/
                var client = new HttpClient();
                /*Paso 2: Cargamos la dirección base (urlBase) al Cliente, antes creado*/
                client.BaseAddress = new Uri(urlBase);
                /*Paso 3: Consumir un servicio de forma segura*/
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                /*Paso 4: Ahora contatenamos el Prefijo y el Controlador para poder acceder al metodo.*/
                var url = $"{prefix}{controller}/{id}"; // Esto es equivalente a: var url = String.Format("{0}{1}", prefix, controller);
                /*Paso 5: Creamos un objeto para capturar la respuesta obtenida por el Servicio RESFull, dicha respuesta puede tardar por eso le colocamos un <await> y ejecutamos el <GetAsync> del cliente antes creado y le pasamos el url complementario (Prefijo y Controlador).*/
                var response = await client.DeleteAsync(url);
                /*Paso 6: En la linea anterior establecimos la comunicación y ejecutamos el Metodo; ahora debemos obtener el resultado contenido en el Objeto antes creado (<response>); esto puede ser cualquier cosa (Stream, String, etc.) pero en este caso lo que esperamos es un JSON que viene contenido en el objeto <response> lo obtenermos con el metodo (Content.ReadAsStringAsync())*/
                var answer = await response.Content.ReadAsStringAsync();
                /*Paso 7: Debemos validar si la ejecución fue exitosa o no.*/
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,
                    };
                }
                /*Paso 8: Devolvemos el Objeto <Response> con la variable <IsSuccess> en <True> y en el <Result> la lista Deserializada de la Respuesta.*/
                return new Response
                {
                    IsSuccess = true,
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

        public async Task<Response> Put<T>(string urlBase, string prefix, string controller, T model, int id)
        {
            try
            {
                /*Para el Metodo POST traigo todo el Codigo del GET y le agrego estas Lineas al comienzo, para serializar el modelo enviado en los parametros*/
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                /*Para consumir un Servicio RESFull*/
                /*Paso 1: Vamos a crear un cliente como un nuevo objeto <HttpClient()> para establecer la comunicación*/
                var client = new HttpClient();
                /*Paso 2: Cargamos la dirección base (urlBase) al Cliente, antes creado*/
                client.BaseAddress = new Uri(urlBase);
                /*Paso 3: Ahora contatenamos el Prefijo y el Controlador para poder acceder al metodo.*/
                var url = $"{prefix}{controller}/{id}"; // Esto es equivalente a: var url = String.Format("{0}{1}", prefix, controller);
                                                        /*Paso 4: Creamos un objeto para capturar la respuesta obtenida por el Servicio RESFull, dicha respuesta puede tardar por eso le colocamos un <await> y ejecutamos el <GetAsync> del cliente antes creado y le pasamos el url complementario (Prefijo y Controlador).*/
                var response = await client.PutAsync(url, content);
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
                var obj = JsonConvert.DeserializeObject<T>(answer);
                /*Paso 8: Devolvemos el Objeto <Response> con la variable <IsSuccess> en <True> y en el <Result> la lista Deserializada de la Respuesta.*/
                return new Response
                {
                    IsSuccess = true,
                    Result = obj,
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

        public async Task<Response> Put<T>(string urlBase, string prefix, string controller, T model, int id, string tokenType, string accessToken)
        {
            try
            {
                /*Para el Metodo POST traigo todo el Codigo del GET y le agrego estas Lineas al comienzo, para serializar el modelo enviado en los parametros*/
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                /*Para consumir un Servicio RESFull*/
                /*Paso 1: Vamos a crear un cliente como un nuevo objeto <HttpClient()> para establecer la comunicación*/
                var client = new HttpClient();
                /*Paso 2: Cargamos la dirección base (urlBase) al Cliente, antes creado*/
                client.BaseAddress = new Uri(urlBase);
                /*Paso 3: Consumir un servicio de forma segura*/
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                /*Paso 4: Ahora contatenamos el Prefijo y el Controlador para poder acceder al metodo.*/
                var url = $"{prefix}{controller}/{id}"; // Esto es equivalente a: var url = String.Format("{0}{1}", prefix, controller);
                /*Paso 5: Creamos un objeto para capturar la respuesta obtenida por el Servicio RESFull, dicha respuesta puede tardar por eso le colocamos un <await> y ejecutamos el <GetAsync> del cliente antes creado y le pasamos el url complementario (Prefijo y Controlador).*/
                var response = await client.PutAsync(url, content);
                /*Paso 6: En la linea anterior establecimos la comunicación y ejecutamos el Metodo; ahora debemos obtener el resultado contenido en el Objeto antes creado (<response>); esto puede ser cualquier cosa (Stream, String, etc.) pero en este caso lo que esperamos es un JSON que viene contenido en el objeto <response> lo obtenermos con el metodo (Content.ReadAsStringAsync())*/
                var answer = await response.Content.ReadAsStringAsync();
                /*Paso 7: Debemos validar si la ejecución fue exitosa o no.*/
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,
                    };
                }
                /*Paso 8: Si la Ejecución fue exitosa debemos Serializar(de Objeto a String) o Deserializar(de String a Objeto) el resultado.*/
                var obj = JsonConvert.DeserializeObject<T>(answer);
                /*Paso 9: Devolvemos el Objeto <Response> con la variable <IsSuccess> en <True> y en el <Result> la lista Deserializada de la Respuesta.*/
                return new Response
                {
                    IsSuccess = true,
                    Result = obj,
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

        public async Task<Response> GetUser(string urlBase, string prefix, string controller, string email, string tokenType, string accessToken)
        {
            try
            {
                var getUserRequest = new GetUserRequest
                {
                    Email = email,
                };

                var request = JsonConvert.SerializeObject(getUserRequest);
                var content = new StringContent(request, Encoding.UTF8, "application/json");

                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                var url = $"{prefix}{controller}";
                var response = await client.PostAsync(url, content);
                var answer = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,
                    };
                }

                var user = JsonConvert.DeserializeObject<MyUserASP>(answer);
                return new Response
                {
                    IsSuccess = true,
                    Result = user,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<FacebookResponse> GetFacebook(string accessToken)
        {
            var requestUrl = "https://graph.facebook.com/v2.8/me/?fields=name," +
                             "picture.width(999),cover,age_range,devices,email,gender," +
                             "is_verified,birthday,languages,work,website,religion," +
                             "location,locale,link,first_name,last_name," +
                             "hometown&access_token=" + accessToken;
            var httpClient = new HttpClient();
            var userJson = await httpClient.GetStringAsync(requestUrl);
            var facebookResponse =
                JsonConvert.DeserializeObject<FacebookResponse>(userJson);
            return facebookResponse;
        }

        public async Task<InstagramResponse> GetInstagram(string accessToken)
        {
            var client = new HttpClient();
            var userJson = await client.GetStringAsync(accessToken);
            var InstagramJson = JsonConvert.DeserializeObject<InstagramResponse>(userJson);
            return InstagramJson;
        }

        public async Task<TokenResponse> LoginTwitter(string urlBase, string servicePrefix, string controller, TwitterResponse profile)
        {
            try
            {
                var request = JsonConvert.SerializeObject(profile);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = $"{servicePrefix}{controller}";
                var response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var tokenResponse = await GetToken(
                    urlBase,
                    profile.IdStr,
                    profile.IdStr);
                return tokenResponse;
            }
            catch
            {
                return null;
            }
        }

        public async Task<TokenResponse> LoginInstagram(string urlBase, string servicePrefix, string controller, InstagramResponse profile)
        {
            try
            {
                var request = JsonConvert.SerializeObject(profile);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = $"{servicePrefix}{controller}";
                var response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var tokenResponse = await GetToken(
                    urlBase,
                    profile.UserData.Id,
                    profile.UserData.Id);
                return tokenResponse;
            }
            catch
            {
                return null;
            }
        }

        public async Task<TokenResponse> LoginFacebook(string urlBase, string servicePrefix, string controller, FacebookResponse profile)
        {
            try
            {
                var request = JsonConvert.SerializeObject(profile);
                var content = new StringContent(
                    request,
                    Encoding.UTF8,
                    "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = $"{servicePrefix}{controller}";
                var response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var tokenResponse = await GetToken(
                    urlBase,
                    profile.Id,
                    profile.Id);
                return tokenResponse;
            }
            catch
            {
                return null;
            }
        }

    }
}
