using System;
using System.Text;
using System.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.IO;
using System.Net;

namespace Attendify
{
    public static class Helper
    {
        private static string loginEndpoint = "http://35.197.206.56/login";
        private static string getAttendanceEndpoint = "http://35.197.206.56/attendance/student/";
        public static string logoPath = "https://www.lincoln.ac.uk/home/media/responsive2017/styleassets/images/HQ-Black-Landscaperesize2.png";

        private static async Task<JsonValue> postReq(string endpoint, string json)
        {

            try
            {

                var client = new HttpClient();
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(endpoint, content); ;

                using (Stream stream = await response.Content.ReadAsStreamAsync())
                { 
                    return await Task.Run(() => JsonValue.Load(stream));
                }

            }
            catch (Exception e)
            {

                throw e;
            }


        }

        private static async Task<JsonValue> getReq(string endpoint, string token)
        {

            try
            {

                var client = new HttpClient();
                var httpRequestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(endpoint),
                    Headers = {
                        { HttpRequestHeader.Authorization.ToString(), "Bearer "+token },
                        { HttpRequestHeader.Accept.ToString(), "application/json" }
                    }
                };

                var response = await client.SendAsync(httpRequestMessage);

                using (Stream stream = await response.Content.ReadAsStreamAsync())
                {
                    return await Task.Run(() => JsonValue.Load(stream));
                }

            }
            catch (Exception e)
            {

                throw e;
            }


        }

        public static async Task<JsonValue> parseJwtAsync(string token)
        {
            try
            {
            var base64Url = token.Split('.')[1];
            var base64 = base64Url.Replace('-', '+').Replace('_', '/');
            byte[] data = Convert.FromBase64String(base64);
            string decodedString = Encoding.UTF8.GetString(data);
            byte[] byteArray = Encoding.UTF8.GetBytes(decodedString);
            using (MemoryStream stream = new MemoryStream(byteArray))
            {

                return await Task.Run(() => JsonValue.Load(stream));

            }
        }
            catch (Exception e)
            {

                throw e;
            }
}

        public static async Task<JsonValue> Login(string em, string pass)
        {

            string json = JsonConvert.SerializeObject(new
            {
                email = em,
                password = pass
            });

            return await Task.Run(() => postReq(loginEndpoint, json));

        }

        public static async Task<JsonValue> GetAttendance(string token,string student_id)
        {
        

            return await Task.Run(() => getReq(getAttendanceEndpoint + student_id, token));

        }
    }
}
