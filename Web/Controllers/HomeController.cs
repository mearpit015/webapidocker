using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        static string accessToken = string.Empty;
        static string baseurl = "https://localhost:44302/api";
        static string _userName = string.Empty;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            LoginAsync().Wait();
            ConsumeApiTest2().Wait();
            ViewBag.UserName = _userName;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public static async Task<dynamic> ConsumeApiTest()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Add the Authorization header with the AccessToken.
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);

                // create the URL string.
                string url = string.Format("" + baseurl + "/Login/GetValue");

                // make the request
                HttpResponseMessage response = await client.GetAsync(url);

                // parse the response and return the data.
                string jsonString = await response.Content.ReadAsStringAsync();
                object responseData = JsonConvert.DeserializeObject(jsonString);
                return (dynamic)responseData;
            }
        }

        public static async Task<dynamic> ConsumeApiTest2()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Add the Authorization header with the AccessToken.
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);

                // create the URL string.
                string url = string.Format("" + baseurl + "/Login/Post");

                var json = JsonConvert.SerializeObject("");
                var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json"); //
                // make the request
                HttpResponseMessage response = await client.PostAsync(url, stringContent);

                // parse the response and return the data.
                string jsonString = await response.Content.ReadAsStringAsync();
                object responseData = JsonConvert.DeserializeObject(jsonString);
                _userName = (dynamic)responseData;
                return (dynamic)responseData;
            }
        }

        public static async Task LoginAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseurl);

                // We want the response to be JSON.
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Build up the data to POST.
                List<KeyValuePair<string, string>> postData = new List<KeyValuePair<string, string>>();
                postData.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
                //postData.Add(new KeyValuePair<string, string>("client_id",     clientId));
                //postData.Add(new KeyValuePair<string, string>("client_secret", clientSecret));

                FormUrlEncodedContent content = new FormUrlEncodedContent(postData);
                //var json = JsonConvert.SerializeObject("");
                //var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json"); //
                // Post to the Server and parse the response.
                HttpResponseMessage response = await client.GetAsync(""+ baseurl +"/Login/");
                string jsonString = await response.Content.ReadAsStringAsync();
                object responseData = JsonConvert.DeserializeObject(jsonString);

                // return the Access Token.
                //return ((dynamic)responseData).access_token;
                var json = Newtonsoft.Json.Linq.JObject.Parse(jsonString);
                accessToken = json["token"].ToString();
            }
        }

    }
}
