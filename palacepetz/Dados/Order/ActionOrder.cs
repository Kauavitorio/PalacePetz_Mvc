using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace palacepetz.Dados.Order
{
    public class ActionOrder
    {
        private static int statusCode;
        private static string responseBody;
        private static string BASE_URL = "https://palacepetzapi.herokuapp.com/";

        public static async Task<int> Finish_Order(int id_user, string cd_card, string sub_total, string totalPrice)
        {
            string discount = "0";
            string coupom = "";
            //  Variable set for storing api responses
            var url = BASE_URL + "order/finish-order/";
            var request = (HttpWebRequest)WebRequest.Create(url);
            string json = $"{{\"id_user\":\"{id_user}\",\"cd_card\":\"{cd_card}\",\"discount\":\"{discount}\",\"coupom\":\"{coupom}\",\"sub_total\":\"{sub_total}\"," +
                $"\"totalPrice\":\"{totalPrice}\"}}";

            /**** Starting Creating request body ****/
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
                                System.Diagnostics.Debug.WriteLine("" + responseBody);
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
                System.Diagnostics.Debug.WriteLine("" + ex.ToString());
                return statusCode;
            }
        }
    }
}