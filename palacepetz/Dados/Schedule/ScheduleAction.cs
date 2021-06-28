using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using palacepetz.Models.Schedule;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace palacepetz.Dados.Schedule
{
    public class ScheduleAction
    {
        private static int statusCode;
        private static string responseBody;
        private static string BASE_URL = "https://palacepetzapi.herokuapp.com/";

        public static async Task<int> Create_Schedule(DtoSchedule scheduleinfo, int id_user)
        {
            string date_schedule = scheduleinfo.date_schedule;
            string time_schedule = scheduleinfo.time_schedule;
            int cd_animal = scheduleinfo.cd_animal;
            int cd_veterinary = scheduleinfo.cd_veterinary;
            string description = scheduleinfo.description;
            int service_type = scheduleinfo.service_type;
            int payment_type = scheduleinfo.payment_type;
            int delivery = scheduleinfo.delivery;

            //  Variable set for storing api responses
            var url = BASE_URL + "user/create/schedule";
            var request = (HttpWebRequest)WebRequest.Create(url);
            string json;
            if (service_type == 2)
            {
                json = $"{{\"date_schedule\":\"{date_schedule}\",\"time_schedule\":\"{time_schedule}\",\"cd_animal\":\"{cd_animal}\",\"cd_veterinary\":\"{null}\",\"description\":\"{description}\"" +
                $",\"service_type\":\"{service_type}\",\"payment_type\":\"{payment_type}\",\"delivery\":\"{delivery}\",\"id_user\":\"{id_user}\"}}";
            }
            else
            {
                json = $"{{\"date_schedule\":\"{date_schedule}\",\"time_schedule\":\"{time_schedule}\",\"cd_animal\":\"{cd_animal}\",\"cd_veterinary\":\"{cd_veterinary}\",\"description\":\"{description}\"" +
                $",\"service_type\":\"{service_type}\",\"payment_type\":\"{payment_type}\",\"delivery\":\"{delivery}\",\"id_user\":\"{id_user}\"}}";
            }
            

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
                    if (statusCode == 201)
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
            catch (Exception ex)
            {
                statusCode = 500;
                System.Diagnostics.Debug.WriteLine("" + ex);
                return statusCode;
            }
        }

        public static List<DtoSchedule> GetAllSchedules(int id_user)
        {
            //  Variable to storing Api Response
            string responseBody;

            //  Variable set for storing api responses
            var url = BASE_URL + "user/schedules/" + id_user;

            /**** Starting Creating request body ****/
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            /**** End Creating request body ****/

            //  Creating list of categoria
            List<DtoSchedule> productlist = new List<DtoSchedule>();
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
                                    productlist.Add(
                                        new DtoSchedule
                                        {
                                            cd_schedule = (int)itens_response.cd_schedule,
                                            id_user = (int)itens_response.id_user,
                                            date_schedule = itens_response.date_schedule + "",
                                            time_schedule = itens_response.time_schedule + "",
                                            cd_animal = (int)itens_response.cd_animal,
                                            description = (string)itens_response.description + "",
                                            nm_veterinary = (string)itens_response.nm_veterinary,
                                            nm_animal = itens_response.nm_animal + "",
                                            service_type = (int)itens_response.service_type,
                                            delivery = (int)itens_response.delivery,
                                            status = (int)itens_response.status
                                        }
                                       );
                                }
                            }
                        }
                    }
                    else productlist.Add(null);
                }
                return productlist;
            }
            catch (WebException ex)
            {
                System.Diagnostics.Debug.WriteLine("Erro on category request: " + ex);
                productlist.Add(null);
                return productlist;
            }
        }
    }
}