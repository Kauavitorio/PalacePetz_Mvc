using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using palacepetz.Models.products;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace palacepetz.Dados.Product
{
    public class AcProducts
    {
        private static string BASE_URL = "https://palacepetzapi.herokuapp.com/";

        public List<DtoProduct> GetAllCategories()
        {
            //  Variable to storing Api Response
            string responseBody;

            //  Variable set for storing api responses
            var url = BASE_URL + "products/list/filter/popular";

            /**** Starting Creating request body ****/
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            /**** End Creating request body ****/

            //  Creating list of categoria
            List<DtoProduct> productlist = new List<DtoProduct>();
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
                                productlist.Add(
                                    new DtoProduct
                                    {
                                        cd_prod = itens_response.cd_prod,
                                        cd_category = itens_response.cd_category + "",
                                        nm_category = itens_response.nm_category + "",
                                        nm_product = itens_response.nm_product,
                                        amount = itens_response.amount + "",
                                        species = itens_response.species + "",
                                        product_price = itens_response.product_price + "",
                                        description = itens_response.description + "",
                                        date_prod = itens_response.date_prod + "",
                                        shelf_life = itens_response.shelf_life + "",
                                        image_prod = itens_response.image_prod + "",
                                        popular = itens_response.popular + ""
                                    }
                                   );
                            }
                        }
                    }
                }
                return productlist;
            }
            catch (WebException ex)
            {
                System.Diagnostics.Debug.WriteLine("Erro on category request: " + ex);
                productlist.Add(
                                    new DtoProduct
                                    {
                                        nm_category = ex.ToString()
                                    }
                                   );
                return productlist;
            }
        }
    }
}