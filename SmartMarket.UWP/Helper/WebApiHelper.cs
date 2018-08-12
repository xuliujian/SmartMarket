using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.IO;
using Newtonsoft.Json;

namespace SmartMarket.UWP.Helper
{
    public class WebAPIServiceHelper
    {
        /// <summary>
        /// 调用WebAPI的get方法
        /// </summary>
        /// <param name="uri">地址</param>
        /// <returns>Json数据</returns>
        public static string GetResponseJson(string url)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add( new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = httpClient.GetAsync(url).Result;


            if (response.IsSuccessStatusCode)
            {
                string responseJson = response.Content.ReadAsStringAsync().Result;
                return responseJson;
            }
            else
            {
                string sError = response.Content.ReadAsStringAsync().Result;
                throw new Exception(sError);
                //HttpClientError httpError = response.Content.ReadAsAsync<HttpClientError>().Result;
                //throw new Exception(httpError.ExceptionMessage == null ? httpError.MessageDetail : httpError.ExceptionMessage);
            }
        }


        /// <summary>
        /// 调用WebAPI的get方法
        /// </summary>
        /// <param name="uri">地址</param>
        /// <returns>Json数据</returns>

        public static T GetResponseJson<T>(string url)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = httpClient.GetAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    T responseJson = JsonConvert.DeserializeObject<T>(result);
                    return responseJson;
                }
                else
                {
                    string sError = response.Content.ReadAsStringAsync().Result;
                    throw new Exception(sError);
                }
            }
        }


        /// <summary>
        /// 调用WebAPI的get方法
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static byte[] GetResponseStream(string uri)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                var response = httpClient.GetAsync(uri).Result;
                if (response.IsSuccessStatusCode)
                {
                    byte[] bytes = response.Content.ReadAsByteArrayAsync().Result;
                    return bytes;
                }
                else
                {
                    string sError = response.Content.ReadAsStringAsync().Result;
                    throw new Exception(sError);
                    //HttpClientError httpError = response.Content.ReadAsAsync<HttpClientError>().Result;
                    //throw new Exception(httpError.ExceptionMessage == null ? httpError.MessageDetail : httpError.ExceptionMessage);
                }
            }
        }


        /// <summary>
        /// 将json数据Post到指定Uri
        /// </summary>
        /// <param name="uri">链接</param>
        /// <param name="json">数据</param>
        /// <returns></returns>
        public static string PostResponseJson(string uri, string postJson)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                HttpContent httpContent = new StringContent(postJson);
                httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                var response = httpClient.PostAsync(uri, httpContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseJson = response.Content.ReadAsStringAsync().Result;
                    return responseJson;
                }
                else
                {
                    string sError = response.Content.ReadAsStringAsync().Result;
                    throw new Exception(sError);
                    //HttpClientError httpError = response.Content.ReadAsAsync<HttpClientError>().Result;
                    //throw new Exception(httpError.ExceptionMessage == null ? httpError.MessageDetail : httpError.ExceptionMessage);
                }
            }
        }

        public static string PostResponseJson(string uri, string postJson, string cookie)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                HttpContent httpContent = new StringContent(postJson);
                httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                var response = httpClient.PostAsync(uri, httpContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseJson = response.Content.ReadAsStringAsync().Result;
                    return responseJson;
                }
                else
                {
                    string sError = response.Content.ReadAsStringAsync().Result;
                    throw new Exception(sError);
                    //HttpClientError httpError = response.Content.ReadAsAsync<HttpClientError>().Result;
                    //throw new Exception(httpError.ExceptionMessage == null ? httpError.MessageDetail : httpError.ExceptionMessage);
                }
            }
        }
        /// <summary>
        /// 将object数据Post到指定Uri
        /// </summary>
        /// <param name="uri">链接</param>
        /// <param name="content">数据</param>
        /// <returns></returns>
        public static T PostResponseJson<T>(string uri, object content)
        {
            using (var httpClient = new HttpClient())
            {
                //httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                //HttpResponseMessage response = httpClient.PostAsJsonAsync(uri, content).Result;
                HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(content));
                httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                var response = httpClient.PostAsync(uri, httpContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    T t = JsonConvert.DeserializeObject<T>(result);
                    return t;
                }
                else
                {
                    string sError = response.Content.ReadAsStringAsync().Result;
                    throw new Exception(sError);
                    //HttpClientError httpError = response.Content.ReadAsAsync<HttpClientError>().Result;
                    //throw new Exception(httpError.ExceptionMessage == null ? httpError.MessageDetail : httpError.ExceptionMessage);
                }
            }
        }


        /// <summary>
        /// 将byte[]数据Post到指定Uri
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="fileByte"></param>
        /// <param name="formData"></param>
        /// <returns></returns>
        public static string PostResponseFormData(string uri, Dictionary<string, byte[]> fileByte, Dictionary<string, string> formData = null)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                var httpContent = new MultipartFormDataContent();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/json"));
                if (formData != null)
                {
                    foreach (var key in formData.Keys)
                    {
                        var dataContent = new ByteArrayContent(Encoding.UTF8.GetBytes(formData[key]));
                        dataContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                        {
                            Name = key
                        };
                        httpContent.Add(dataContent);
                    }
                }
                foreach (var key in fileByte.Keys)
                {
                    var fileContent = new ByteArrayContent(fileByte[key]);
                    fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                    {
                        FileName = key
                    };
                    httpContent.Add(fileContent);
                }
                var response = httpClient.PostAsync(uri, httpContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseJson = response.Content.ReadAsStringAsync().Result;
                    return responseJson;
                }
                else
                {
                    string sError = response.Content.ReadAsStringAsync().Result;
                    throw new Exception(sError);
                    //HttpClientError httpError = response.Content.ReadAsAsync<HttpClientError>().Result;
                    //throw new Exception(httpError.ExceptionMessage == null ? httpError.MessageDetail : httpError.ExceptionMessage);
                }
            }
        }


        /// <summary>
        /// 将json数据Post到指定Uri
        /// </summary>
        /// <param name="uri">链接</param>
        /// <param name="json">数据</param>
        /// <returns></returns>
        public static byte[] PostResponseJson2Bytes(string uri, string postJson)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                HttpContent httpContent = new StringContent(postJson);
                httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                var response = httpClient.PostAsync(uri, httpContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    byte[] bytes = response.Content.ReadAsByteArrayAsync().Result;
                    return bytes;
                }
                else
                {
                    string sError = response.Content.ReadAsStringAsync().Result;
                    throw new Exception(sError);
                    //HttpClientError httpError = response.Content.ReadAsAsync<HttpClientError>().Result;
                    //throw new Exception(httpError.ExceptionMessage == null ? httpError.MessageDetail : httpError.ExceptionMessage);
                }
            }
        }


        /// <summary>
        /// 将json数据Post到指定Uri
        /// </summary>
        /// <param name="uri">链接</param>
        /// <param name="json">数据</param>
        /// <returns></returns>
        public static string PostResponseStream(string uri, string filename, Stream stream)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.MaxResponseContentBufferSize = 256000;
                //httpClient.DefaultRequestHeaders.Add("user-agent", "User-Agent    Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; Touch; MALNJS; rv:11.0) like Gecko");//设置请求头
                MultipartFormDataContent mulContent = new MultipartFormDataContent();
                HttpContent fileContent = new StreamContent(stream);

                //fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(" text/html; charset=utf-8");
                mulContent.Add(fileContent, Guid.NewGuid().ToString(), filename);


                var response = httpClient.PostAsync(uri, mulContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseJson = response.Content.ReadAsStringAsync().Result;
                    return responseJson;
                }
                else
                {
                    string sError = response.Content.ReadAsStringAsync().Result;
                    throw new Exception(sError);
                }
            }
        }
        public static bool PostForm(string url, string postJson, string cookieStr)
        {
            CookieContainer cookies = new CookieContainer();
            string[] cookieStrs = cookieStr.Split('=');
            Cookie cookie = new Cookie(cookieStrs[0], cookieStrs[1]);
            string domain = Regex.Replace(url, "[a-zA-z]+://", "");
            domain = domain.Split('/')[0];
            domain = domain.Split(':')[0];
            cookie.Domain = domain.Equals("localhost") ? "127.0.0.1" : domain;
            cookies.Add(cookie);
            byte[] postData = Encoding.ASCII.GetBytes(postJson.Substring(0, postJson.Length - 1));
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.AllowAutoRedirect = false;
            request.ContentType = "application/x-www-form-urlencoded;charset=gbk";
            request.CookieContainer = cookies;
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.11 (KHTML, like Gecko) Chrome/23.0.1271.95 Safari/537.11";
            request.ContentLength = postData.Length;


            Stream requestStream = request.GetRequestStream();
            requestStream.Write(postData, 0, postData.Length);
            requestStream.Close();

            return true;
        }


        /// <summary>
        /// 带令牌post指定Url
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="ticket">令牌</param>
        /// <returns></returns>
        public static string PostResponsewithToken(string url, string ticket)
        {
            WebRequest request = WebRequest.Create(url);
            request.ContentType = "application/json";
            request.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + ticket);
            request.Method = "Get";
            request.ContentLength = 0;
            WebResponse response = request.GetResponse();
            using (StreamReader stream = new StreamReader(response.GetResponseStream()))
            {
                var s = stream.ReadToEnd();
                return s;
            }
        }

        private sealed class HttpClientError
        {
            public string ExceptionMessage { get; set; }
            public string ExceptionType { get; set; }
            public HttpClientError InnerException { get; set; }
            public string Message { get; set; }
            public string MessageDetail { get; set; }
            public HttpClientError ModelState { get; set; }
            public string StackTrace { get; set; }
        }
    }
}
