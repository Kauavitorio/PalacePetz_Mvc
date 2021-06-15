using Firebase.Auth;
using Firebase.Storage;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using palacepetz.Models.products;
using palacepetz.Models.User;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace palacepetz.Dados.Product
{
    public class actions
    {
        private static string ApiKey = "AIzaSyBvpFy5jw0Q8G9KYoASqs1W968h7mXv1nk";
        private static string Bucket = "palacepetz-22a1b.appspot.com";
        private static string AuthEmail = "testmvc@gmail.com";
        private static string AuthPassword = "testmvc1234";
        private static int statusCode;
        private static string responseBody;
        private static string BASE_URL = "https://palacepetzapi.herokuapp.com/";

        public static async Task<int> InsertNewProduct(DtoProduct prodInfo)
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
            var url = BASE_URL + "products/register";
            var request = (HttpWebRequest)WebRequest.Create(url);
            string json = $"{{\"cd_category\":\"{cd_category}\",\"nm_product\":\"{nm_product}\",\"amount\":\"{amount}\",\"species\":\"{species}\",\"product_price\":\"{product_price}\"" +
                $",\"description\":\"{description}\",\"shelf_life\":\"{shelf_life}\",\"image_prod\":\"{image_prod}\"}}";

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
    }
}