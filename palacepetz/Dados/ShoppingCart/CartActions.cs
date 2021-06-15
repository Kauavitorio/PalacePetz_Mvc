using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using palacepetz.Models.products;
using palacepetz.Models.shoppingcart;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace palacepetz.Dados.ShoppingCart
{
    public class CartActions
    {
        private static int statusCode;
        private static string responseBody;
        private static string BASE_URL = "https://palacepetzapi.herokuapp.com/";
        public static string GetCartSize(int id_user)
        {
            //  Variable to storing Api Response

            //  Variable set for storing api responses
            var url = BASE_URL + "shoppingcart/size/" + id_user;

            /**** Starting Creating request body ****/
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
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
                    else if (statusCode == 404)
                        return "404";
                    else
                        return "500";
                }
            }
            catch (WebException ex)
            {
                if (ex.Message.Contains("(405)"))
                    return "405";
                else
                    return "500";
            }
        }


        public static int Insert_New_Product_OnUserCart(DtoProduct prodInfo, string totalPrice, string sub_total)
        {
            long cd_prod = prodInfo.cd_prod;
            int id_user = prodInfo.id_user;
            string amount = prodInfo.amount;
            double product_price = prodInfo.product_price;

            //  Variable set for storing api responses
            var url = BASE_URL + "shoppingcart/insert";
            var request = (HttpWebRequest)WebRequest.Create(url);
            string json = $"{{\"cd_prod\":\"{cd_prod}\",\"id_user\":\"{id_user}\",\"product_price\":\"{product_price}\",\"totalPrice\":\"{totalPrice}\",\"product_amount\":\"{amount}\"" +
                $",\"sub_total\":\"{sub_total}\"}}";

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
            catch (WebException ex)
            {
                System.Diagnostics.Debug.WriteLine("" + ex);
                return statusCode;
            }
        }

        public static List<DtoShoppingCart> Get_user_cart(int id_user)
        {

            //  Variable to storing Api Response
            string responseBody;

            //  Variable set for storing api responses
            var url = BASE_URL + "shoppingcart/" + id_user;

            /**** Starting Creating request body ****/
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            /**** End Creating request body ****/

            //  Creating list of categoria
            List<DtoShoppingCart> shoppingcartList = new List<DtoShoppingCart>();
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
                            try
                            {
                                foreach (var itens_response in ((IEnumerable<dynamic>)config.Search))
                                {
                                    shoppingcartList.Add(
                                        new DtoShoppingCart
                                        {
                                            cd_cart = itens_response.cd_prod,
                                            cd_prod = itens_response.cd_prod,
                                            id_user = itens_response.id_user,
                                            product_amount = itens_response.product_amount + "",
                                            amount = itens_response.amount,
                                            product_price = itens_response.product_price + "",
                                            totalPrice = itens_response.totalPrice + "",
                                            sub_total = itens_response.sub_total + "",
                                            nm_product = itens_response.nm_product + "",
                                            image_prod = itens_response.image_prod + ""
                                        }
                                       );
                                }
                            }
                            catch
                            {
                                shoppingcartList.Add(null);
                            }
                        }
                    }
                }
                return shoppingcartList;
            }
            catch (WebException ex)
            {
                System.Diagnostics.Debug.WriteLine("Erro on category request: " + ex);
                shoppingcartList.Add(null);
                return shoppingcartList;
            }
        }

        public static string GetFullPriceCart(int id_user)
        {

            double fullprice = 0;
            //  Variable to storing Api Response
            string responseBody;

            //  Variable set for storing api responses
            var url = BASE_URL + "shoppingcart/" + id_user;

            /**** Starting Creating request body ****/
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            /**** End Creating request body ****/

            //  Creating list of categoria
            List<DtoShoppingCart> shoppingcartList = new List<DtoShoppingCart>();
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
                                string getPrice = itens_response.totalPrice + "";
                                var val = double.Parse(getPrice,
                                    NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint | NumberStyles.AllowCurrencySymbol);
                                fullprice += val;
                            };
                        }
                    }
                }
                return fullprice + "";
            }
            catch (WebException ex)
            {
                System.Diagnostics.Debug.WriteLine("Erro on category request: " + ex);
                fullprice = 0;
                return fullprice +" " + ex;
            }
        }

        public static string Remove_item_from_cart(int cd_cart, int id_user)
        {
            //  Variable set for storing api responses
            var url = BASE_URL + "shoppingcart/remove/" + id_user + "/" + cd_cart;
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
                            if (strReader == null) return "500";

                            //  If not null will start to read respose
                            using (StreamReader objReader = new StreamReader(strReader))
                            {
                                //  Saving everything that was read in the responseBody
                                responseBody = objReader.ReadToEnd();
                                return statusCode + "";
                            }
                        }
                    }
                    else if (statusCode == 417)
                        return "417";
                    else
                        return "500";
                }
            }
            catch (WebException ex)
            {
                if (ex.Message.Contains("(417)"))
                    return "417";
                else
                    return "500";
            }
        }
    }
}