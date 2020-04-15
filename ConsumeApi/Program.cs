using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ConsumeApi
{
    class Program
    {
        static string accessToken = string.Empty;//"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJhcnBpdDAxMCIsImVtYWlsIjoiYXJwaXRAZ21haWwuY29tIiwianRpIjoiMjhkMzczNGMtYjBiZC00ODcwLWE0ZDMtNTAyMWJmYzc2N2I1IiwiZXhwIjoxNTg2OTE4MjgzLCJpc3MiOiJodHRwczovL2Rldi1nZzl4d3lkNC5hdXRoMC5jb20vYXBpL3YyLyIsImF1ZCI6Imh0dHBzOi8vZGV2LWdnOXh3eWQ0LmF1dGgwLmNvbS9hcGkvdjIvIn0.2qfA-__MEp-R_-DFJEyh5yzoN2qlu8EtBxG9k9eKfZs";
        static string baseurl = "https://localhost:44302/api"; //
        static void Main(string[] args)
        {
             LoginAsync().Wait();
            ConsumeApiTest().Wait();
            ConsumeApiTest2().Wait();
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
                return (dynamic)responseData;
            }
        }

        public static async System.Threading.Tasks.Task LoginAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44302/api/");

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
                HttpResponseMessage response = await client.GetAsync("https://localhost:44302/api/Login/");
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
