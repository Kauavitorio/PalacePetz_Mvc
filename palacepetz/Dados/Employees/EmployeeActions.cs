using Firebase.Auth;
using Firebase.Storage;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using palacepetz.Models.Employee;
using palacepetz.Models.products;
using palacepetz.Models.Schedule;
using palacepetz.Models.User;
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
        public static List<DtoUser> GetCustomers(int id_employee)
        {
            //  Variable to storing Api Response
            string responseBody;

            //  Variable set for storing api responses
            var url = BASE_URL + "employee/customer/list/" + id_employee;

            /**** Starting Creating request body ****/
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            /**** End Creating request body ****/

            //  Creating list of cards
            List<DtoUser> customerList = new List<DtoUser>();
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
                                customerList.Add(
                                    new DtoUser
                                    {
                                        id_user = (int)itens_response.id_user,
                                        name_user = (string)itens_response.name_user,
                                        email = (string)itens_response.email,
                                        cpf_user = (string)itens_response.cpf_user,
                                        phone_user = (string)itens_response.phone_user,
                                        user_type = (int) itens_response.user_type,
                                        status = (int) itens_response.status
                                    });
                            }
                        }
                    }
                }
                return customerList;
            }
            catch (WebException ex)
            {
                System.Diagnostics.Debug.WriteLine("" + ex);
                customerList.Add(null);
                return customerList;
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

        public static List<SelectListItem> GetProductsSelectList()
        {
            //  Variable to storing Api Response
            string responseBody;

            //  Variable set for storing api responses
            var url = BASE_URL + "products/list";

            /**** Starting Creating request body ****/
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            /**** End Creating request body ****/

            //  Creating list of categoria
            List<SelectListItem> productsList = new List<SelectListItem>();
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
                                productsList.Add(
                                        new SelectListItem
                                        {
                                            Value = itens_response.cd_prod + "",
                                            Text = (string)itens_response.nm_product
                                        }
                                       );
                            }
                        }
                    }
                }
                return productsList;
            }
            catch (WebException ex)
            {
                System.Diagnostics.Debug.WriteLine("" + ex);
                productsList.Add(null);
                return productsList;
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

        public static int PutMoreProductAmount(int cd_prod, int amount)
        {
            //  Variable set for storing api responses
            var url = BASE_URL + "employee/products/update/amount/" + cd_prod + "/" + amount;
            var request = (HttpWebRequest)WebRequest.Create(url);

            /**** Starting Creating request body ****/
            request.Method = "PATCH";
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
            var new_desc = description.ToString();
            //var new_desc = description.Replace("\n", " ").Replace("\r", "");

            //  Variable set for storing api responses
            var url = BASE_URL + "employee/update/product";
            var request = (HttpWebRequest)WebRequest.Create(url);
            string json = $"{{\"cd_category\":\"{cd_category}\",\"nm_product\":\"{nm_product}\",\"amount\":\"{amount}\",\"species\":\"{species}\",\"product_price\":\"{product_price}\"" +
                $",\"description\":\"{new_desc}\",\"shelf_life\":\"{shelf_life}\",\"image_prod\":\"{image_prod}\",\"cd_prod\":\"{cd_prod}\"}}";

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

        public static async Task<int> DisableCustomer(int id_user, int id_employee)
        {

            //  Variable set for storing api responses
            var url = BASE_URL + "employee/customer/disable/";
            var request = (HttpWebRequest)WebRequest.Create(url);
            string json = $"{{\"id_user\":\"{id_user}\",\"id_employee\":\"{id_employee}\"}}";

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

        public static async Task<int> EnableCustomer(int id_user, int id_employee)
        {

            //  Variable set for storing api responses
            var url = BASE_URL + "employee/customer/enable/";
            var request = (HttpWebRequest)WebRequest.Create(url);
            string json = $"{{\"id_user\":\"{id_user}\",\"id_employee\":\"{id_employee}\"}}";

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

        public static List<DtoSchedule> GetAllScheduledServices(int id_employee, int user_type)
        {
            //  Variable to storing Api Response
            string responseBody;

            //  Variable set for storing api responses
            var url = BASE_URL + "employee/services/scheduled/" + id_employee;

            /**** Starting Creating request body ****/
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            /**** End Creating request body ****/

            //  Creating list of cards
            List<DtoSchedule> scheduledList = new List<DtoSchedule>();
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
                                if(user_type == 2)
                                {
                                    if((int)itens_response.service_type == 1)
                                    {
                                        if ((int)itens_response.service_type == 1)
                                        {
                                            if ((int)itens_response.cd_veterinary == id_employee)
                                            {
                                                scheduledList.Add(
                                                   new DtoSchedule
                                                   {
                                                       cd_schedule = (int)itens_response.cd_schedule,
                                                       id_user = (int)itens_response.id_user,
                                                       name_user = (string)itens_response.name_user,
                                                       date_schedule = itens_response.date_schedule + "",
                                                       time_schedule = itens_response.time_schedule + "",
                                                       cd_animal = (int)itens_response.cd_animal,
                                                       description = (string)itens_response.description + "",
                                                       nm_veterinary = (string)itens_response.nm_veterinary,
                                                       nm_animal = itens_response.nm_animal + "",
                                                       service_type = (int)itens_response.service_type,
                                                       delivery = (int)itens_response.delivery,
                                                       status = (int)itens_response.status
                                                   });
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    scheduledList.Add(
                                   new DtoSchedule
                                   {
                                       cd_schedule = (int)itens_response.cd_schedule,
                                       id_user = (int)itens_response.id_user,
                                       name_user = (string)itens_response.name_user,
                                       date_schedule = itens_response.date_schedule + "",
                                       time_schedule = itens_response.time_schedule + "",
                                       cd_animal = (int)itens_response.cd_animal,
                                       description = (string)itens_response.description + "",
                                       nm_veterinary = (string)itens_response.nm_veterinary,
                                       nm_animal = itens_response.nm_animal + "",
                                       service_type = (int)itens_response.service_type,
                                       delivery = (int)itens_response.delivery,
                                       status = (int)itens_response.status
                                   });
                                }
                               
                            }
                        }
                    }
                }
                return scheduledList;
            }
            catch (WebException ex)
            {
                System.Diagnostics.Debug.WriteLine("" + ex);
                scheduledList.Add(null);
                return scheduledList;
            }
        }

        public static DtoUser GetCustomerInfo(int id_user)
        {
            //  Variable to storing Api Response
            string responseBody;

            //  Variable set for storing api responses
            var url = BASE_URL + "employee/custumer/get/" + id_user;

            /**** Starting Creating request body ****/
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            /**** End Creating request body ****/

            //  Creating list of categoria
            DtoUser user = new DtoUser();
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
                                user.id_user = (int)itens_response.id_user;
                                user.name_user = (string)itens_response.name_user;
                                user.cpf_user = (string)itens_response.cpf_user;
                                user.phone_user = (string)itens_response.phone_user;
                                user.email = (string)itens_response.email;
                                user.zipcode = (string)itens_response.zipcode;
                                user.address_user = (string)itens_response.address_user;
                                user.complement = (string)itens_response.complement;
                            }
                        }
                    }
                }
                return user;
            }
            catch (WebException ex)
            {
                System.Diagnostics.Debug.WriteLine("" + ex);
                user = null;
                return user;
            }
        }

        public static DtoSchedule GetScheduledServicenfo(int id_employee, int cd_schedule, int id_user)
        {
            //  Variable to storing Api Response
            string responseBody;

            //  Variable set for storing api responses
            var url = BASE_URL + "employee/services/scheduled/details/" + id_employee + "/" + cd_schedule + "/" + id_user;

            /**** Starting Creating request body ****/
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            /**** End Creating request body ****/

            //  Creating list of categoria
            DtoSchedule service = new DtoSchedule();
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
                                service.cd_schedule = (int)itens_response.cd_schedule;
                                service.id_user = (int)itens_response.id_user;
                                service.name_user = (string)itens_response.name_user;
                                service.date_schedule = itens_response.date_schedule + "";
                                service.time_schedule = itens_response.time_schedule + "";
                                service.cd_animal = (int)itens_response.cd_animal;
                                service.description = (string)itens_response.description + "";
                                service.nm_veterinary = (string)itens_response.nm_veterinary;
                                service.nm_animal = itens_response.nm_animal + "";
                                service.service_type = (int)itens_response.service_type;
                                service.delivery = (int)itens_response.delivery; 
                                service.status = (int)itens_response.status;
                            }
                        }
                    }
                }
                return service;
            }
            catch (WebException ex)
            {
                System.Diagnostics.Debug.WriteLine("" + ex);
                service = null;
                return service;
            }
        }

        public static async Task<int> UpdateCustomerProfile(DtoUser userinfo, int id_user)
        {
            var name_user = userinfo.name_user;
            var cpf_user = userinfo.cpf_user;
            var phone_user = userinfo.phone_user;
            var birth_date = userinfo.birth_date;
            var address_user = userinfo.address_user;
            var complement = userinfo.complement;
            var zipcode = userinfo.zipcode;
            var password = userinfo.password;
            //  Variable set for storing api responses
            var url = BASE_URL + "employee/custumer/edit/";
            var request = (HttpWebRequest)WebRequest.Create(url);
            string json = $"{{\"name_user\":\"{name_user}\",\"cpf_user\":\"{cpf_user}\",\"address_user\":\"{address_user}\",\"complement\":\"{complement}\",\"zipcode\":\"{zipcode}\"" +
                $",\"phone_user\":\"{phone_user}\",\"birth_date\":\"{birth_date}\",\"id_user\":\"{id_user}\",\"password\":\"{password}\"}}";

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

        public static List<DtoOrders> GetAllOrder(int id_employee)
        {
            //  Variable to storing Api Response
            string responseBody;

            //  Variable set for storing api responses
            var url = BASE_URL + "employee/order/get/" + id_employee;

            /**** Starting Creating request body ****/
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            /**** End Creating request body ****/

            //  Creating list of cards
            List<DtoOrders> ordersList = new List<DtoOrders>();
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
                                ordersList.Add(
                                    new DtoOrders
                                    {
                                        cd_order = (long)itens_response.cd_order,
                                        id_user = (int)itens_response.id_user,
                                        date_order = (string)itens_response.date_order,
                                        status = (string)itens_response.status
                                    }
                                   );
                            }
                        }
                    }
                }
                return ordersList;
            }
            catch (WebException ex)
            {
                System.Diagnostics.Debug.WriteLine("" + ex);
                ordersList.Add(null);
                return ordersList;
            }
        }

        public static DtoOrders GetDetailsOrder(int id_employee, int cd_order)
        {
            //  Variable to storing Api Response
            string responseBody;

            //  Variable set for storing api responses
            var url = BASE_URL + "employee/order/get/details/" + id_employee + "/" + cd_order;

            /**** Starting Creating request body ****/
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            /**** End Creating request body ****/

            //  Creating list of cards
            DtoOrders order = new DtoOrders();
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
                                order = new DtoOrders
                                {
                                    cd_order = (long)itens_response.cd_order,
                                    id_user = (int)itens_response.id_user,
                                    date_order = (string)itens_response.date_order,
                                    status = (string)itens_response.status
                                };
                            }
                        }
                    }
                }
                return order;
            }
            catch (WebException ex)
            {
                System.Diagnostics.Debug.WriteLine("" + ex);
                order = null;
                return order;
            }
        }

        public static async Task<int> UpdateOrderStatus(DtoOrders orderinfo, int id_employee)
        {
            var status = orderinfo.status;
            var cd_order = orderinfo.cd_order;
            var id_user = orderinfo.id_user;
            //  Variable set for storing api responses
            var url = BASE_URL + "employee/order/update/" + id_employee;
            var request = (HttpWebRequest)WebRequest.Create(url);
            string json = $"{{\"status\":\"{status}\",\"cd_order\":\"{cd_order}\",\"id_user\":\"{id_user}\"}}";

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
    }
}