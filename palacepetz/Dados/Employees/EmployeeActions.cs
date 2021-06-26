using Firebase.Auth;
using Firebase.Storage;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using palacepetz.Models.Employee;
using palacepetz.Models.products;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace palacepetz.Dados.Employees
{
    public class EmployeeActions
    {

        private static string ApiKey = "AIzaSyBvpFy5jw0Q8G9KYoASqs1W968h7mXv1nk";
        private static string Bucket = "palacepetz-22a1b.appspot.com";
        private static string AuthEmail = "testmvc@gmail.com";
        private static string AuthPassword = "testmvc1234";
        private static string BASE_URL = "https://palacepetzapi.herokuapp.com/";
        private static int statusCode;
        private static string responseBody;

        public static List<DtoEmployee> GetAllEmployee(int id_employee)
        {
            //  Variable to storing Api Response
            string responseBody;

            //  Variable set for storing api responses
            var url = BASE_URL + "employee/employee/" + id_employee;

            /**** Starting Creating request body ****/
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            /**** End Creating request body ****/

            //  Creating list of cards
            List<DtoEmployee> employeeList = new List<DtoEmployee>();
            try
            {
                //  Sending request to api
                using (WebResponse response = request.GetResponse())
                {
                    //  After sending the request to the api, the system was created to handle the data received
                    using (Stream strReader = response.GetResponseStream())
                    {
                        //  Checking if respose is null
                        if (strReader == null) return null;

                        //  If not null will start to read respose
                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            //  Saving everything that was read in the responseBody
                            responseBody = objReader.ReadToEnd();

                            //  System created to be able to read individual items from the api response
                            dynamic config = JsonConvert.DeserializeObject<ExpandoObject>(responseBody, new ExpandoObjectConverter());

                            //  Selecting everything within the json "Search" list and putting where i want
                            foreach (var itens_response in ((IEnumerable<dynamic>)config.Search))
                            {
                                employeeList.Add(
                                    new DtoEmployee
                                    {
                                        id_user = (long)itens_response.id_user,
                                        id_employee = (long)itens_response.id_employee,
                                        user_type = (int)itens_response.user_type,
                                        role = (string)itens_response.role,
                                        number_ctps = (string)itens_response.number_ctps,
                                        name_user = (string)itens_response.name_user,
                                        username = (string)itens_response.username,
                                        email = (string)itens_response.email,
                                        cpf_user = (string)itens_response.cpf_user,
                                        address_user = (string)itens_response.address_user,
                                        complement = (string)itens_response.complement,
                                        zipcode = (string)itens_response.zipcode,
                                        phone_user = (string)itens_response.phone_user,
                                        birth_date = (string)itens_response.birth_date,
                                        img_user = (string)itens_response.img_user
                                    });
                            }
                        }
                    }
                }
                return employeeList;
            }
            catch (WebException ex)
            {
                System.Diagnostics.Debug.WriteLine("" + ex);
                employeeList.Add(null);
                return employeeList;
            }
        }

        public static string GetEmployeeInformation(int id_user)
        {
            //  Variable set for storing api responses
            var url = BASE_URL + "employee/informations";
            var request = (HttpWebRequest)WebRequest.Create(url);
            string json = $"{{\"id_user\":\"{id_user}\"}}";

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
                                System.Diagnostics.Debug.WriteLine("" + responseBody);
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
        public static List<SelectListItem> GetVeterinarys()
        {
            //  Variable to storing Api Response
            string responseBody;

            //  Variable set for storing api responses
            var url = BASE_URL + "employee/employee/1444";

            /**** Starting Creating request body ****/
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            /**** End Creating request body ****/

            //  Creating list of categoria
            List<SelectListItem> veterinaryList = new List<SelectListItem>();
            try
            {
                //  Sending request to api
                using (WebResponse response = request.GetResponse())
                {
                    //  After sending the request to the api, the system was created to handle the data received
                    using (Stream strReader = response.GetResponseStream())
                    {
                        //  Checking if respose is null
                        if (strReader == null) return null;

                        //  If not null will start to read respose
                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            //  Saving everything that was read in the responseBody
                            responseBody = objReader.ReadToEnd();

                            //  System created to be able to read individual items from the api response
                            dynamic config = JsonConvert.DeserializeObject<ExpandoObject>(responseBody, new ExpandoObjectConverter());

                            //  Selecting everything within the json "Search" list and putting where i want
                            foreach (var itens_response in ((IEnumerable<dynamic>)config.Search))
                            {
                                if((int)itens_response.user_type == 2)
                                {
                                    veterinaryList.Add(
                                        new SelectListItem
                                        {
                                            Value = itens_response.id_user + "",
                                            Text = (string)itens_response.name_user
                                        }
                                       );
                                }
                            }
                        }
                    }
                }
                return veterinaryList;
            }
            catch (WebException ex)
            {
                System.Diagnostics.Debug.WriteLine("" + ex);
                veterinaryList.Add(null);
                return veterinaryList;
            }
        }

        public static string GetStatistics()
        {
            //  Variable set for storing api responses
            var url = BASE_URL + "employee/company/statistics/";
            var request = (HttpWebRequest)WebRequest.Create(url);
            string json = $"{{\"id_user\":\"{100}\"}}";

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
                                System.Diagnostics.Debug.WriteLine("" + responseBody);
                                return responseBody;
                            }
                        }
                    }
                    else return statusCode.ToString();
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

        public static int DeleteProduct(int cd_prod)
        {
            //  Variable set for storing api responses
            var url = BASE_URL + "user/pet/remove/";
            var request = (HttpWebRequest)WebRequest.Create(url);

            /**** Starting Creating request body ****/
            request.Method = "DELETE";
            request.ContentType = "application/json";
            request.Accept = "application/json";
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
                            if (strReader == null) return statusCode;

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
                        return statusCode;
                }
            }
            catch (WebException ex)
            {
                System.Diagnostics.Debug.WriteLine("" + ex);
                return statusCode;
            }
        }

        public static async Task<int> UpdateProduct(DtoProduct prodInfo, int cd_prod)
        {
            string cd_category = prodInfo.cd_category + "";
            string nm_product = prodInfo.nm_product;
            string amount = prodInfo.amount;
            string species = prodInfo.species;
            double product_price = prodInfo.product_price;
            string description = prodInfo.description;
            string shelf_life = prodInfo.shelf_life;
            string image_prod = prodInfo.image_prod;

            //  Variable set for storing api responses
            var url = BASE_URL + "employee/update/product";
            var request = (HttpWebRequest)WebRequest.Create(url);
            string json = $"{{\"cd_category\":\"{cd_category}\",\"nm_product\":\"{nm_product}\",\"amount\":\"{amount}\",\"species\":\"{species}\",\"product_price\":\"{product_price}\"" +
                $",\"description\":\"{description}\",\"shelf_life\":\"{shelf_life}\",\"image_prod\":\"{image_prod}\",\"cd_prod\":\"{cd_prod}\"}}";

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
                System.Diagnostics.Debug.WriteLine("" + ex);
                return statusCode;
            }
        }

        public static async Task<string> Upload(FileStream stream, string filename)
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
                    .Child("products")
                    .Child("images")
                    .Child("Prods_" + filename)
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