using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using palacepetz.Models.Employee;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Net;

namespace palacepetz.Dados.Manager
{
    public class ManagerActions
    {
        private static string BASE_URL = "https://palacepetzapi.herokuapp.com/";
        private static int statusCode;

        public static DtoEmployee GetEmployeeInfo(int id_employee)
        {
            //  Variable to storing Api Response
            string responseBody;

            //  Variable set for storing api responses
            var url = BASE_URL + "employee/informations/" + id_employee;

            /**** Starting Creating request body ****/
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            /**** End Creating request body ****/

            //  Creating list of categoria
            DtoEmployee employee = new DtoEmployee();
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
                                int user_type_get = (int)itens_response.user_type;
                                employee.id_user = (long)itens_response.id_user;
                                employee.id_employee = (long)itens_response.id_employee;
                                employee.user_type = user_type_get;
                                employee.role = (string)itens_response.role;
                                employee.number_ctps = (string)itens_response.number_ctps;
                                employee.name_user = (string)itens_response.name_user;
                                employee.username = (string)itens_response.username;
                                employee.email = (string)itens_response.email;
                                employee.cpf_user = (string)itens_response.cpf_user;
                                employee.address_user = (string)itens_response.address_user;
                                employee.complement = (string)itens_response.complement;
                                employee.zipcode = (string)itens_response.zipcode;
                                employee.phone_user = (string)itens_response.phone_user;
                                employee.birth_date = (string)itens_response.birth_date;
                                employee.img_user = (string)itens_response.img_user;
                                if (user_type_get == 2) employee.num_crmv = itens_response.num_crmv + "";
                                else employee.num_crmv = null;
                            }
                        }
                    }
                }
                return employee;
            }
            catch (WebException ex)
            {
                System.Diagnostics.Debug.WriteLine("" + ex);
                employee = null;
                return employee;
            }
        }

        public static int Register_New_Employee(DtoEmployee employeeInfo, int id_manager)
        {
            var name_user = employeeInfo.name_user;
            var email = employeeInfo.email;
            var cpf_user = employeeInfo.cpf_user;
            var address_user = employeeInfo.address_user;
            var complement = employeeInfo.complement;
            var birth_date = employeeInfo.birth_date;
            var zipcode = employeeInfo.zipcode;
            var user_type = employeeInfo.user_type;
            var img_user = " ";
            var password = employeeInfo.password;
            var phone_user = employeeInfo.phone_user;
            var role = employeeInfo.role;
            var number_ctps = employeeInfo.number_ctps;
            var num_crmv = employeeInfo.num_crmv;


            var url = BASE_URL + "employee/register-employee";
            var request = (HttpWebRequest)WebRequest.Create(url);
            string json = $"{{\"name_user\":\"{name_user}\",\"email\":\"{email}\",\"cpf_user\":\"{cpf_user}\",\"address_user\":\"{address_user}\",\"complement\":\"{complement}\",\"birth_date\":\"{birth_date}\",\"zipcode\":\"{zipcode}\"" +
                $",\"user_type\":\"{user_type}\",\"img_user\":\"{img_user}\",\"password\":\"{password}\",\"phone_user\":\"{phone_user}\",\"role\":\"{role}\",\"number_ctps\":\"{number_ctps}\",\"id_employee\":\"{id_manager}\",\"num_crmv\":\"{num_crmv}\"}}";

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

        public static int Edit_Employee(DtoEmployee employeeInfo, int id_manager)
        {
            var name_user = employeeInfo.name_user;
            var email = employeeInfo.email;
            var cpf_user = employeeInfo.cpf_user;
            var address_user = employeeInfo.address_user;
            var complement = employeeInfo.complement;
            var birth_date = employeeInfo.birth_date;
            var zipcode = employeeInfo.zipcode;
            var user_type = employeeInfo.user_type;
            var img_user = " ";
            var password = employeeInfo.password;
            var phone_user = employeeInfo.phone_user;
            var role = employeeInfo.role;
            var number_ctps = employeeInfo.number_ctps;
            var num_crmv = employeeInfo.num_crmv;


            var url = BASE_URL + "employee/update-employee";
            var request = (HttpWebRequest)WebRequest.Create(url);
            string json = $"{{\"name_user\":\"{name_user}\",\"email\":\"{email}\",\"cpf_user\":\"{cpf_user}\",\"address_user\":\"{address_user}\",\"complement\":\"{complement}\",\"birth_date\":\"{birth_date}\",\"zipcode\":\"{zipcode}\"" +
                $",\"user_type\":\"{user_type}\",\"img_user\":\"{img_user}\",\"password\":\"{password}\",\"phone_user\":\"{phone_user}\",\"role\":\"{role}\",\"number_ctps\":\"{number_ctps}\",\"id_employee\":\"{id_manager}\",\"id_user\":\"{employeeInfo.id_user}\",\"num_crmv\":\"{num_crmv}\"}}";

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

        public static int Remove_Employee(int id_employee, int id_manager)
        {
            var url = BASE_URL + "employee/delete-employee";
            var request = (HttpWebRequest)WebRequest.Create(url);
            string json = $"{{\"id_user\":\"{id_employee}\",\"id_employee\":\"{id_manager}\"}}";

            System.Diagnostics.Debug.WriteLine("" + json); 
            request.Method = "DELETE";
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