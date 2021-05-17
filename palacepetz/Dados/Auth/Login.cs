using Firebase.Auth;
using palacepetz.Models.User;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace palacepetz.Dados.Auth
{
    public class Login
    {
        private static int statusCode;
        private static List<DtoUser> userinfo;
        private static string responseBody;
        private static string ApiKey = "AIzaSyBvpFy5jw0Q8G9KYoASqs1W968h7mXv1nk";
        private static string BASE_URL = "https://palacepetzapi.herokuapp.com/";


        public static async Task<string> Authlogin(string email, string password)
        {
            //  Variable to storing Api Response

            //  Variable set for storing api responses
            var url = BASE_URL + "user/login/";
            var request = (HttpWebRequest)WebRequest.Create(url);
            string json = $"{{\"email\":\"{email}\",\"password\":\"{password}\"}}";

            /**** Starting Creating request body ****/
            System.Diagnostics.Debug.WriteLine("" + json);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }
            /**** End Creating request body ****/

            //  Creating list of categoria
            userinfo = new List<DtoUser>();
            try
            {
                //  Sending request to api
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    statusCode = (int)response.StatusCode;
                    if (statusCode == 200)
                    {
                        //  After sending the request to the api, the system was created to handle the data received
                        using (Stream strReader = response.GetResponseStream())
                        {
                            //  Checking if respose is null
                            if (strReader == null) return "500";

                            //  If not null will start to read respose
                            using (StreamReader objReader = new StreamReader(strReader))
                            {
                                //  Saving everything that was read in the responseBody
                                responseBody = objReader.ReadToEnd();
                                return responseBody;
                            }
                        }
                    }
                    else if (statusCode == 405)
                        return "405";
                    else if (statusCode == 401)
                        return "401";
                    else
                        return "500";
                }
            }
            catch (WebException ex)
            {
                if (ex.Message.Contains("(405)"))
                    return "405";
                else if (ex.Message.Contains("(401)"))
                    return "401";
                else
                    return "500";
            }
        }

        public static async Task<int> AuthRegister(DtoUser user)
        {
            string name_user = user.name_user;
            string email = user.email;
            string cpf_user = user.cpf_user;
            string password = user.password;

            var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
            int resultInsertApi = RegisterUserApi(email, name_user, cpf_user, password);
            if (resultInsertApi == 201)
            {
                try
                {
                    var registerUser = await auth.CreateUserWithEmailAndPasswordAsync(email, password, name_user, false);
                    return resultInsertApi;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Exception was thrown: " + ex.ToString());
                    return 500;
                }
            }
            else
            {
                return resultInsertApi;
            }

        }

        public static async Task<int> RecoverPassword(string email)
        {
            try
            {
                //  Variable set for storing api responses
                var url = BASE_URL + "user/request/password/";
                var request = (HttpWebRequest)WebRequest.Create(url);
                string json = $"{{\"email\":\"{email}\"}}";

                /**** Starting Creating request body ****/
                System.Diagnostics.Debug.WriteLine("" + json);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Accept = "application/json";
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
                /**** End Creating request body ****/

                //  Creating list of categoria
                userinfo = new List<DtoUser>();
                try
                {
                    //  Sending request to api
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        statusCode = (int)response.StatusCode;
                        if (statusCode == 200)
                        {
                            //  After sending the request to the api, the system was created to handle the data received
                            using (Stream strReader = response.GetResponseStream())
                            {
                                //  Checking if respose is null
                                if (strReader == null) return 500;

                                //  If not null will start to read respose
                                using (StreamReader objReader = new StreamReader(strReader))
                                {
                                    //  Saving everything that was read in the responseBody
                                    responseBody = objReader.ReadToEnd();
                                    return statusCode;
                                }
                            }
                        }
                        else
                        {
                            return statusCode;
                        }
                    }
                }
                catch (WebException ex)
                {

                    return statusCode;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception was thrown: " + ex.ToString());
                return statusCode;
            }
        }
        public static async Task<int> ChangePassword(string verify_id,int id_user, string newpassword)
        {
            try
            {
                //  Variable set for storing api responses
                var url = BASE_URL + "user/change/password/";
                var request = (HttpWebRequest)WebRequest.Create(url);
                string json = $"{{\"verify_id\":\"{verify_id}\",\"id_user\":\"{id_user}\",\"newpassword\":\"{newpassword}\"}}";

                /**** Starting Creating request body ****/
                System.Diagnostics.Debug.WriteLine("" + json);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Accept = "application/json";
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
                /**** End Creating request body ****/

                //  Creating list of categoria
                userinfo = new List<DtoUser>();
                try
                {
                    //  Sending request to api
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        statusCode = (int)response.StatusCode;
                        if (statusCode == 200)
                        {
                            //  After sending the request to the api, the system was created to handle the data received
                            using (Stream strReader = response.GetResponseStream())
                            {
                                //  Checking if respose is null
                                if (strReader == null) return 500;

                                //  If not null will start to read respose
                                using (StreamReader objReader = new StreamReader(strReader))
                                {
                                    //  Saving everything that was read in the responseBody
                                    responseBody = objReader.ReadToEnd();
                                    return statusCode;
                                }
                            }
                        }
                        else
                        {
                            return statusCode;
                        }
                    }
                }
                catch (WebException ex)
                {

                    return statusCode;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception was thrown: " + ex.ToString());
                return statusCode;
            }
        }

        private static int GetUserInformation(string email, string password)
        {
            //  Variable to storing Api Response

            //  Variable set for storing api responses
            var url = BASE_URL + "user/login/";
            var request = (HttpWebRequest)WebRequest.Create(url);
            string json = $"{{\"email\":\"{email}\",\"password\":\"{password}\"}}";

            /**** Starting Creating request body ****/
            System.Diagnostics.Debug.WriteLine("" + json);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }
            /**** End Creating request body ****/

            //  Creating list of categoria
            userinfo = new List<DtoUser>();
            try
            {
                //  Sending request to api
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    statusCode = (int)response.StatusCode;
                    if (statusCode == 200)
                    {
                        //  After sending the request to the api, the system was created to handle the data received
                        using (Stream strReader = response.GetResponseStream())
                        {
                            //  Checking if respose is null
                            if (strReader == null) return 500;

                            //  If not null will start to read respose
                            using (StreamReader objReader = new StreamReader(strReader))
                            {
                                //  Saving everything that was read in the responseBody
                                responseBody = objReader.ReadToEnd();
                                return statusCode;
                            }
                        }
                    }
                    else
                    {
                        return statusCode;
                    }
                }
            }
            catch (WebException ex)
            {

                return statusCode;
            }
        }
        private static int RegisterUserApi(string email, string name_user, string cpf_user, string password)
        {
            var url = BASE_URL + "user/register";
            var request = (HttpWebRequest)WebRequest.Create(url);
            string json = $"{{\"name_user\":\"{name_user}\",\"email\":\"{email}\",\"cpf_user\":\"{cpf_user}\",\"password\":\"{password}\"}}";

            System.Diagnostics.Debug.WriteLine("" + json);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }
            try
            {   // WebResponse with this not work
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    statusCode = (int)response.StatusCode;
                    using (Stream strReader = response.GetResponseStream())
                    {
                        if (strReader == null) return 500;
                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            string responseBody = objReader.ReadToEnd();
                            // Do something with responseBody
                            Console.WriteLine(responseBody);
                            System.Diagnostics.Debug.WriteLine("" + responseBody);
                            return statusCode;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle error
                System.Diagnostics.Debug.WriteLine("response:  " + statusCode);
                System.Diagnostics.Debug.WriteLine("" + ex.ToString());
                return statusCode;
            }
        }

    }
}