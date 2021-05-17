using Firebase.Auth;
using Firebase.Storage;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using palacepetz.Models.category;
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

namespace palacepetz.Dados.Category
{
    public class actions
    {
        int statusCode;
        //  Declaring base API URL
        const string BASE_URL = "https://palacepetzapi.herokuapp.com/";

        //  Created method to get all Category and save on list
        public static List<SelectListItem> GetAllCategorys()
        {
            //  Variable to storing Api Response
            string responseBody;

            //  Variable set for storing api responses
            var url = BASE_URL + "category/list/";

            /**** Starting Creating request body ****/
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            /**** End Creating request body ****/

            //  Creating list of categoria
            List<SelectListItem> categoriaslist = new List<SelectListItem>();
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
                                categoriaslist.Add(
                                    new SelectListItem
                                    {
                                        Value = itens_response.cd_category.ToString(),
                                        Text = (string)itens_response.nm_category
                                    }
                                   );
                            }
                        }
                    }
                }
                return categoriaslist;
            }
            catch (WebException ex)
            {
                System.Diagnostics.Debug.WriteLine("Erro on category request: " + ex);
                categoriaslist.Add(
                                    new SelectListItem
                                    {
                                        Value = ex.ToString(),
                                        Text = ex.ToString()
                                    }
                                   );
                return categoriaslist;
            }
        }
    }
}