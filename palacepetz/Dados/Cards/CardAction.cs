using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace palacepetz.Dados.Cards
{
    public class CardAction
    {
        private static string BASE_URL = "https://palacepetzapi.herokuapp.com/";
        private static int statusCode;


        public static int registerCard(int id_user, string number_card, int cvv_card, string nmUser_card, string shelflife_card, string flag_card) 
        {
            var url = BASE_URL + "user/register/card";
            var request = (HttpWebRequest)WebRequest.Create(url);
            string json = $"{{\"id_user\":\"{id_user}\",\"flag_card\":\"{flag_card}\",\"number_card\":\"{number_card}\",\"shelflife_card\":\"{shelflife_card}\"" +
                $",\"cvv_card\":\"{cvv_card}\",\"nmUser_card\":\"{nmUser_card}\"}}";

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
                            Console.WriteLine(responseBody);
                            return statusCode;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle error
                System.Diagnostics.Debug.WriteLine("" + ex.ToString());
                return statusCode;
            }
        }
    }
}