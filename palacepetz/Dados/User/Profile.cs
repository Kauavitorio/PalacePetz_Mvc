using Firebase.Auth;
using Firebase.Storage;
using palacepetz.Models.User;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace palacepetz.Dados.User
{
    public class Profile
    {
        private static string ApiKey = "AIzaSyBvpFy5jw0Q8G9KYoASqs1W968h7mXv1nk";
        private static string Bucket = "palacepetz-22a1b.appspot.com";
        private static string AuthEmail = "testmvc@gmail.com";
        private static string AuthPassword = "testmvc1234";

        private static int statusCode;
        private static List<DtoUser> userinfo;
        private static string responseBody;
        private static string BASE_URL = "https://palacepetzapi.herokuapp.com/";


        public static int updateAddress(string address_user, string complement, string zipcode, int id_user)
        {
            var url = BASE_URL + "user/update/address/";
            var request = (HttpWebRequest)WebRequest.Create(url);
            string json = $"{{\"address_user\":\"{address_user}\",\"complement\":\"{complement}\",\"zipcode\":\"{zipcode}\",\"id_user\":\"{id_user}\"}}";

            System.Diagnostics.Debug.WriteLine("" + json);
            request.Method = "PATCH";
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

        public static async Task<int> UpdateUserProfileWithImage(FileStream stream, int id_user, DtoUser userinfo)
        {
            var name_user = userinfo.Firstname_user + " " +  userinfo.Lastname_user;
            var cpf_user = userinfo.cpf_user;
            var phone_user = userinfo.phone_user;
            var birth_date = userinfo.birth_date;
            var address_user = userinfo.address_user;
            var complement = userinfo.complement;
            var zipcode = userinfo.zipcode;


            string resultUpload = await Task.Run(() => UploadProfileImage(stream, id_user));
            if(resultUpload != "500")
            {
                UpdateOnAPiUserImage(resultUpload, id_user);
                //  Variable set for storing api responses
                var url = BASE_URL + "user/update/profile";
                var request = (HttpWebRequest)WebRequest.Create(url);
                string json = $"{{\"name_user\":\"{name_user}\",\"cpf_user\":\"{cpf_user}\",\"address_user\":\"{address_user}\",\"complement\":\"{complement}\",\"zipcode\":\"{zipcode}\"" +
                    $",\"phone_user\":\"{phone_user}\",\"birth_date\":\"{birth_date}\",\"id_user\":\"{id_user}\"}}";

                /**** Starting Creating request body ****/
                System.Diagnostics.Debug.WriteLine("" + json);
                request.Method = "PATCH";
                request.ContentType = "application/json";
                request.Accept = "application/json";
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
                /**** End Creating request body ****/

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
            else
            {
                return 500;
            }           
        }

        private static async Task<int> UpdateOnAPiUserImage(string resultUpload, int id_user)
        {
            //  Variable set for storing api responses
            var url = BASE_URL + "user/update/profile/image/";
            var request = (HttpWebRequest)WebRequest.Create(url);
            string json = $"{{\"img_user\":\"{resultUpload}\",\"id_user\":\"{id_user}\"}}";

            /**** Starting Creating request body ****/
            System.Diagnostics.Debug.WriteLine("" + "User image");
            request.Method = "PATCH";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }
            /**** End Creating request body ****/

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

        public static async Task<int> UpdateUserProfile(DtoUser userinfo, int id_user)
        {
            var name_user = userinfo.Firstname_user + " " + userinfo.Lastname_user;
            var cpf_user = userinfo.cpf_user;
            var phone_user = userinfo.phone_user;
            var birth_date = userinfo.birth_date;
            var address_user = userinfo.address_user;
            var complement = userinfo.complement;
            var zipcode = userinfo.zipcode;
            //  Variable set for storing api responses
            var url = BASE_URL + "user/update/profile";
            var request = (HttpWebRequest)WebRequest.Create(url);
            string json = $"{{\"name_user\":\"{name_user}\",\"cpf_user\":\"{cpf_user}\",\"address_user\":\"{address_user}\",\"complement\":\"{complement}\",\"zipcode\":\"{zipcode}\"" +
                $",\"phone_user\":\"{phone_user}\",\"birth_date\":\"{birth_date}\",\"id_user\":\"{id_user}\"}}";

            /**** Starting Creating request body ****/
            System.Diagnostics.Debug.WriteLine("" + json);
            request.Method = "PATCH";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }
            /**** End Creating request body ****/

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

        public static async Task<string> UploadProfileImage(FileStream stream, int filename)
        {
            var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
            try
            {
                var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);
                var cancellation = new CancellationTokenSource();

                var task = new FirebaseStorage(
                    Bucket,
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                        ThrowOnCancel = true
                    })
                    .Child("user")
                    .Child("profile")
                    .Child("User_" + filename)
                    .PutAsync(stream, cancellation.Token);
                try
                {
                    System.Diagnostics.Debug.WriteLine("Upload Ok ");
                    return await task;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Exception was thrown: " + ex);
                    return "500";
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("INVALID_PASSWORD"))
                {
                    System.Diagnostics.Debug.WriteLine("EMail ou senha errado ");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Exception was thrown: " + ex);

                }
                return "500";
            }
        }
    }
}