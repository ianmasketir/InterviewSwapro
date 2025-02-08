using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Reflection;
using System.Text;

namespace PORECT.Helper
{
    public class ApiProcessHelper
    {
        readonly PORECTLog logger = new PORECTLog();

        /// <summary>
        /// Post to an api.
        /// </summary>
        /// <param name="json"></param>
        /// <param name="baseUrl"></param>
        /// <param name="reqEndpoint"></param>
        /// <returns>HTTP Response Code</returns>
        public int Post(string json, string baseUrl, string reqEndpoint)
        {
            try
            {
                int result = 0;

                var data = new StringContent(json, Encoding.UTF8, "application/json");
                string joinUrl = string.Concat(baseUrl, reqEndpoint);
                using (var client = new HttpClient())
                {
                    var response = client.PostAsync(joinUrl, data).Result;
                    //var results = Task.Run(async () => await response.Content.ReadAsStringAsync()).Result;
                    result = (int)response.StatusCode;

                    if (!response.IsSuccessStatusCode)
                        throw new Exception(string.Format("Post failed. Status Code: {0}", (int)response.StatusCode));

                    return result;
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Post to an api.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <param name="baseUrl"></param>
        /// <param name="reqEndpoint"></param>
        /// <returns>Class Model provided when method called.</returns>
        public async Task<T> PostGeneric<T>(string json, string baseUrl, string reqEndpoint) where T : class
        {
            try
            {
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                string joinUrl = string.Concat(baseUrl, reqEndpoint);

                using (var client = new HttpClient())
                {
                    var response = await client.PostAsync(joinUrl, data);
                    var responseContent = await response.Content.ReadAsStringAsync();
                    //var result = JsonSerializer.Deserialize<T>(responseContent);
                    var result = JsonConvert.DeserializeObject<T>(responseContent);

                    if (!response.IsSuccessStatusCode)
                        throw new Exception($"Post failed. Status Code: {(int)response.StatusCode}");

                    return result;
                }
            }
            catch
            {
                throw;
            }
        }

        public string PostString(string json, string baseUrl, string reqEndpoint, object formData = default, bool isFormData = false, bool bypassHTTPS = false, List<ParamTaskViewModel> paramHeader = default, ParamTaskViewModel paramauth = default)
        {
            try
            {
                string result = string.Empty;

                var httpClientHandler = bypassHTTPS ?
                    new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
                    } : new HttpClientHandler();

                using (var client = bypassHTTPS ? new HttpClient(httpClientHandler) : new HttpClient())
                {
                    #region Header
                    if (paramHeader != default && paramHeader.Count > 0)
                    {
                        foreach (var param in paramHeader)
                            client.DefaultRequestHeaders.Add(param.colName, param.value);
                    }
                    #endregion Header

                    #region Authorization
                    if (paramauth != default)
                    {
                        string auth = Convert.ToBase64String(Encoding.ASCII.GetBytes(paramauth.value));
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                                                                        paramauth.colName, auth);
                        //client.DefaultRequestHeaders.Add("Authorization", $"{paramauth.colName} {auth}");
                    }
                    #endregion Authorization

                    HttpResponseMessage response = default;
                    string joinUrl = string.Concat(baseUrl, reqEndpoint);
                    if (!isFormData)
                    {
                        var data = new StringContent(json, Encoding.UTF8, "application/json");
                        logger.LogInfo("ApiProcessHelper", "PostString", "Check json non-formdata", json);

                        response = client.PostAsync(joinUrl, data).Result;
                        //var results = Task.Run(async () => await response.Content.ReadAsStringAsync()).Result;
                        int responseCode = (int)response.StatusCode;
                    }
                    else if (formData != null)
                    {
                        //asumsi: formData = ClassModel/List, IFormFile, other properties
                        var sendData = new MultipartFormDataContent();

                        var items = formData.GetType().GetProperties();
                        foreach (var item in items)
                        {
                            json = string.Empty;
                            var getValues = (item.GetValue(formData)) ?? string.Empty;
                            string typeDesc = string.Empty;
                            #region Get Data Type
                            var objType = getValues.GetType();
                            var objTypeInfo = objType.GetTypeInfo();
                            typeDesc = objTypeInfo.Name; //can be type "List`1" or ClassModel or FormFile
                            #endregion Get Data Type
                            if (typeDesc.IndexOf("Model") > -1)
                            {
                                json = JsonConvert.SerializeObject(getValues);
                                logger.LogInfo("ApiProcessHelper", "PostString", "Check Model to json formdata", json);
                                sendData.Add(new StringContent(json), item.Name);
                            }
                            else if (typeDesc == "FormFile")
                            {
                                var file = (IFormFile)getValues;
                                if (file.Length > 0)
                                    sendData.Add(new StreamContent(file.OpenReadStream()), item.Name, file.FileName);
                            }
                            else
                            {
                                sendData.Add(new StringContent(getValues.ToString()), item.Name);
                            }
                        }
                        logger.LogInfo("ApiProcessHelper", "PostString", "Check formdata to send", JsonConvert.SerializeObject(sendData));
                        response = client.PostAsync(joinUrl, sendData).Result;
                        int responseCode = (int)response.StatusCode;
                    }
                    else
                    {
                        throw new ArgumentNullException("Form data is null");
                    }

                    result = response.Content.ReadAsStringAsync().Result;
                    if (!response.IsSuccessStatusCode)
                        throw new Exception(string.Format("Post failed. Status Code: {0}. Message: {1}", (int)response.StatusCode, result));
                    //else
                    //    result = response.Content.ReadAsStringAsync().Result;

                    return result;
                }
            }
            catch
            {
                throw;
            }
        }
        public string PostString(Dictionary<string, string> xForm, string baseUrl, string reqEndpoint, bool bypassHTTPS = false, List<ParamTaskViewModel> paramHeader = default)
        {
            try
            {
                string result = string.Empty;

                var httpClientHandler = bypassHTTPS ?
                    new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
                    } : new HttpClientHandler();

                using (var client = bypassHTTPS ? new HttpClient(httpClientHandler) : new HttpClient())
                {
                    #region Header
                    if (paramHeader != default && paramHeader.Count > 0)
                    {
                        foreach (var param in paramHeader)
                            client.DefaultRequestHeaders.Add(param.colName, param.value);
                    }
                    #endregion

                    string joinUrl = string.Concat(baseUrl, reqEndpoint);
                    var data = new FormUrlEncodedContent(xForm);

                    var response = client.PostAsync(joinUrl, data).Result;
                    int responseCode = (int)response.StatusCode;

                    if (!response.IsSuccessStatusCode)
                        throw new Exception(string.Format("Post failed. Status Code: {0}", (int)response.StatusCode));
                    else
                        result = response.Content.ReadAsStringAsync().Result;

                    return result;
                }
            }
            catch
            {
                throw;
            }
        }

        //public T PostGeneric<T>(string json, string baseUrl, string reqEndpoint)
        //    where T : class
        //{
        //    try
        //    {
        //        T result = default;

        //        var data = new StringContent(json, Encoding.UTF8, "application/json");
        //        string joinUrl = string.Concat(baseUrl, reqEndpoint);
        //        using (var client = new HttpClient())
        //        {
        //            var response = client.PostAsync(joinUrl, data).Result;
        //            //var responseContent = response.Content.ReadAsStringAsync().Result;
        //            result = JsonSerializer.Deserialize<T>(response);

        //            if (!response.IsSuccessStatusCode)
        //                throw new Exception(string.Format("Post failed. Status Code: {0}", (int)response.StatusCode));

        //            return result;
        //        }
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        /// <summary>
        /// Get data from an api.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param"></param>
        /// <param name="baseUrl"></param>
        /// <param name="reqEndpoint"></param>
        /// <returns>Class Model provided when method called.</returns>
        public async Task<T> GetAsync<T>(List<ParamTaskViewModel> param, string baseUrl, string reqEndpoint)
            where T : class
        {
            try
            {
                T result = default;

                string parameter = string.Empty;
                for (int i = 0; i < param.Count; i++)
                {
                    parameter += (i == 0) ? "?" : "&";
                    parameter += string.Concat(param[i].colName, "=", param[i].value);
                }
                string joinUrl = string.Concat(baseUrl, reqEndpoint, parameter);

                string process = await GetProcessAsync(joinUrl);
                if (!string.IsNullOrEmpty(process))
                    result = JsonConvert.DeserializeObject<T>(process);
                    //result = JsonSerializer.Deserialize<T>(process);

                return result;
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Get data from an api.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param"></param>
        /// <param name="baseUrl"></param>
        /// <param name="reqEndpoint"></param>
        /// <returns>Class Model provided when method called.</returns>
        public T Get<T>(List<ParamTaskViewModel> param, string baseUrl, string reqEndpoint, bool bypassHTTPS = false, List<ParamTaskViewModel> paramHeader = default, ParamTaskViewModel paramauth = default)
            where T : class
        {
            try
            {
                T result = default;

                string parameter = string.Empty;
                for (int i = 0; i < param.Count; i++)
                {
                    parameter += (i == 0) ? "?" : "&";
                    parameter += string.Concat(param[i].colName, "=", param[i].value);
                }
                string joinUrl = string.Concat(baseUrl, reqEndpoint, parameter);

                string process = GetProcess(joinUrl, bypassHTTPS, paramHeader, paramauth);
                if (!string.IsNullOrEmpty(process))
                    result = System.Text.Json.JsonSerializer.Deserialize<T>(process);

                return result;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get list data from an api.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param"></param>
        /// <param name="baseUrl"></param>
        /// <param name="reqEndpoint"></param>
        /// <returns>List of Class Model provided when method called.</returns>
        public List<T> GetList<T>(List<ParamTaskViewModel> param, string baseUrl, string reqEndpoint)
            where T : class
        {
            try
            {
                List<T> result = new List<T>();

                string parameter = string.Empty;
                for (int i = 0; i < param.Count; i++)
                {
                    parameter += (i == 0) ? "?" : "&";
                    parameter += string.Concat(param[i].colName, "=", param[i].value);
                }
                string joinUrl = string.Concat(baseUrl, reqEndpoint, parameter);

                string process = GetProcess(joinUrl);
                if (!string.IsNullOrEmpty(process))
                    result = JsonConvert.DeserializeObject<List<T>>(process);
                    //result = JsonSerializer.Deserialize<List<T>>(process);

                return result;
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Get list data from an api.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param"></param>
        /// <param name="baseUrl"></param>
        /// <param name="reqEndpoint"></param>
        /// <returns>List of Class Model provided when method called.</returns>
        public async Task<List<T>> GetListAsync<T>(List<ParamTaskViewModel> param, string baseUrl, string reqEndpoint)
            where T : class
        {
            try
            {
                List<T> result = new List<T>();

                string parameter = string.Empty;
                for (int i = 0; i < param.Count; i++)
                {
                    parameter += (i == 0) ? "?" : "&";
                    parameter += string.Concat(param[i].colName, "=", param[i].value);
                }
                string joinUrl = string.Concat(baseUrl, reqEndpoint, parameter);

                string process = await GetProcessAsync(joinUrl);
                if (!string.IsNullOrEmpty(process))
                    result = JsonConvert.DeserializeObject<List<T>>(process);
                    //result = JsonSerializer.Deserialize<List<T>>(process);

                return result;
            }
            catch
            {
                throw;
            }
        }

        private string GetProcess(string joinUrl, bool bypassHTTPS = false, List<ParamTaskViewModel> paramHeader = default, ParamTaskViewModel paramauth = default)
        {
            try
            {
                string result = string.Empty;

                var httpClientHandler = bypassHTTPS ?
                    new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
                    } : new HttpClientHandler();

                //using (var client = new HttpClient())
                using (var client = bypassHTTPS ? new HttpClient(httpClientHandler) : new HttpClient())
                {
                    #region Header
                    if (paramHeader != default && paramHeader.Count > 0)
                    {
                        foreach (var param in paramHeader)
                            client.DefaultRequestHeaders.Add(param.colName, param.value);
                    }
                    #endregion Header

                    #region Authorization
                    if (paramauth != default)
                    {
                        string auth = Convert.ToBase64String(Encoding.ASCII.GetBytes(paramauth.value));
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                                                                        paramauth.colName, auth);
                        //client.DefaultRequestHeaders.Add("Authorization", $"{paramauth.colName} {auth}");
                    }
                    #endregion Authorization

                    var response = client.GetAsync(joinUrl).Result;
                    //var results = Task.Run(async () => await response.Content.ReadAsStringAsync()).Result;
                    int responseCode = (int)response.StatusCode;

                    if (response.IsSuccessStatusCode)
                        result = response.Content.ReadAsStringAsync().Result;
                    else
                        throw new Exception(string.Format("Get failed. Status Code: {0}", (int)response.StatusCode));

                    return result;
                }
            }
            catch
            {
                throw;
            }
        }
        private async Task<string> GetProcessAsync(string joinUrl)
        {
            try
            {
                string result = string.Empty;

                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(joinUrl);
                    //var results = Task.Run(async () => await response.Content.ReadAsStringAsync()).Result;
                    int responseCode = (int)response.StatusCode;

                    if (response.IsSuccessStatusCode)
                        result = response.Content.ReadAsStringAsync().Result;
                    else
                        throw new Exception(string.Format("Get failed. Status Code: {0}", (int)response.StatusCode));

                    return result;
                }
            }
            catch
            {
                throw;
            }
        }

    }
}
