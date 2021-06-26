using Firebase.Auth;
using Firebase.Storage;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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

namespace palacepetz.Dados.User.Pets
{
    public class ActionPets
    {
        private static string ApiKey = "AIzaSyBvpFy5jw0Q8G9KYoASqs1W968h7mXv1nk";
        private static string Bucket = "palacepetz-22a1b.appspot.com";
        private static string AuthEmail = "testmvc@gmail.com";
        private static string AuthPassword = "testmvc1234";

        private static int statusCode;
        private static string responseBody;
        private static string BASE_URL = "https://palacepetzapi.herokuapp.com/";

        public static async Task<int> RegisterPet_With_image(FileStream stream, int id_user, DtoPet petinfo)
        {
            var nm_animal = petinfo.nm_animal;
            var breed_animal = petinfo.breed_animal;
            var age_animal = petinfo.age_animal;
            var weight_animal = petinfo.weight_animal;
            var species_animal = petinfo.species_animal;


            string resultUpload = await Task.Run(() => UploadImage(stream, id_user, petinfo.nm_animal));
            if (resultUpload != "500")
            {
                //  Variable set for storing api responses
                var url = BASE_URL + "user/pet/insert";
                var request = (HttpWebRequest)WebRequest.Create(url);
                string json = $"{{\"nm_animal\":\"{nm_animal}\",\"id_user\":\"{id_user}\",\"image_animal\":\"{resultUpload}\",\"breed_animal\":\"{breed_animal}\",\"age_animal\":\"{age_animal}\"" +
                    $",\"weight_animal\":\"{weight_animal}\",\"species_animal\":\"{species_animal}\"}}";

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
            else
            {
                return 500;
            }
        }

        public static async Task<int> RegisterPet(int id_user, DtoPet petinfo)
        {
            var nm_animal = petinfo.nm_animal;
            var breed_animal = petinfo.breed_animal;
            var img_pet = "";
            var age_animal = petinfo.age_animal;
            var weight_animal = petinfo.weight_animal;
            var species_animal = petinfo.species_animal;


            //  Variable set for storing api responses
            var url = BASE_URL + "user/pet/insert";
            var request = (HttpWebRequest)WebRequest.Create(url);
            string json = $"{{\"nm_animal\":\"{nm_animal}\",\"id_user\":\"{id_user}\",\"image_animal\":\"{img_pet}\",\"breed_animal\":\"{breed_animal}\",\"age_animal\":\"{age_animal}\"" +
                $",\"weight_animal\":\"{weight_animal}\",\"species_animal\":\"{species_animal}\"}}";

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
        public static async Task<int> EditPet(int id_user, DtoPet petinfo, string img_pet)
        {
            var nm_animal = petinfo.nm_animal;
            var breed_animal = petinfo.breed_animal;
            var age_animal = petinfo.age_animal;
            var weight_animal = petinfo.weight_animal;
            var species_animal = petinfo.species_animal;
            var cd_animal = petinfo.cd_animal;


            //  Variable set for storing api responses
            var url = BASE_URL + "user/pet/update";
            var request = (HttpWebRequest)WebRequest.Create(url);
            string json = $"{{\"nm_animal\":\"{nm_animal}\",\"id_user\":\"{id_user}\",\"image_animal\":\"{img_pet}\",\"breed_animal\":\"{breed_animal}\",\"age_animal\":\"{age_animal}\"" +
                $",\"weight_animal\":\"{weight_animal}\",\"species_animal\":\"{species_animal}\",\"cd_animal\":\"{cd_animal}\"}}";

            /**** Starting Creating request body ****/
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

        public static async Task<int> EditPet_With_image(FileStream stream, int id_user, DtoPet petinfo)
        {
            var nm_animal = petinfo.nm_animal;
            var breed_animal = petinfo.breed_animal;
            var age_animal = petinfo.age_animal;
            var weight_animal = petinfo.weight_animal;
            var species_animal = petinfo.species_animal; ;
            var cd_animal = petinfo.cd_animal;


            string resultUpload = await Task.Run(() => UploadImage(stream, id_user, petinfo.nm_animal));
            if (resultUpload != "500")
            {
                //  Variable set for storing api responses
                var url = BASE_URL + "user/pet/update";
                var request = (HttpWebRequest)WebRequest.Create(url);
                string json = $"{{\"nm_animal\":\"{nm_animal}\",\"id_user\":\"{id_user}\",\"image_animal\":\"{resultUpload}\",\"breed_animal\":\"{breed_animal}\",\"age_animal\":\"{age_animal}\"" +
                    $",\"weight_animal\":\"{weight_animal}\",\"species_animal\":\"{species_animal}\",\"cd_animal\":\"{cd_animal}\"}}";

                /**** Starting Creating request body ****/
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
            else
            {
                return 500;
            }
        }

        public static async Task<string> UploadImage(FileStream stream, int filename, string nm_pet)
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
                    .Child("user")
                    .Child("pets")
                    .Child("User_" + filename + "_" + nm_pet)
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

        //  Created method to get all Pets of User
        public static List<DtoPet> GetPetList(int id_user)
        {
            //  Variable to storing Api Response
            string responseBody;

            //  Variable set for storing api responses
            var url = BASE_URL + "user/pet/" + id_user;

            /**** Starting Creating request body ****/
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            /**** End Creating request body ****/

            //  Creating list of categoria
            List<DtoPet> petList = new List<DtoPet>();
            try
            {
                //  Sending request to api
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    statusCode = (int)response.StatusCode;
                    if(statusCode == 200)
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
                                    petList.Add(
                                        new DtoPet
                                        {
                                            cd_animal = (long)itens_response.cd_animal,
                                            id_user = (long)itens_response.id_user,
                                            breed_animal = (string)itens_response.breed_animal,
                                            nm_animal = (string)itens_response.nm_animal,
                                            age_animal = (string)itens_response.age_animal,
                                            weight_animal = (string)itens_response.weight_animal,
                                            species_animal = (string)itens_response.species_animal,
                                            image_animal = (string)itens_response.image_animal
                                        }
                                       );
                                }
                            }
                        }
                    }
                    else
                    {
                        petList.Add(null);
                    }
                    
                }
                return petList;
            }
            catch (WebException ex)
            {
                System.Diagnostics.Debug.WriteLine("" + ex);
                petList.Add(null);
                return petList;
            }
        }

        //  Created method to get all one Pets of User
        public static DtoPet GetOnePetList(int id_user, int cd_animal)
        {
            //  Variable to storing Api Response
            string responseBody;

            //  Variable set for storing api responses
            var url = BASE_URL + "user/pet/" + id_user + "/" + cd_animal;

            /**** Starting Creating request body ****/
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            /**** End Creating request body ****/

            //  Creating list of categoria
            DtoPet pet = new DtoPet();
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
                                pet.cd_animal = (long)itens_response.cd_animal;
                                pet.id_user = (long)itens_response.id_user;
                                pet.breed_animal = (string)itens_response.breed_animal;
                                pet.nm_animal = (string)itens_response.nm_animal;
                                pet.age_animal = (string)itens_response.age_animal;
                                pet.weight_animal = (string)itens_response.weight_animal;
                                pet.species_animal = (string)itens_response.species_animal;
                                pet.image_animal = (string)itens_response.image_animal;
                            }
                        }
                    }
                }
                return pet;
            }
            catch (WebException ex)
            {
                System.Diagnostics.Debug.WriteLine("" + ex);
                pet = null;
                return pet;
            }
        }

        public static int Remove_user_pet(int cd_animal, int id_user)
        {
            //  Variable set for storing api responses
            var url = BASE_URL + "user/pet/remove/" + cd_animal + "/" + id_user;
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

        public static List<SelectListItem> GetPetListShedule(int id_user)
        {
            //  Variable to storing Api Response
            string responseBody;

            //  Variable set for storing api responses
            var url = BASE_URL + "user/pet/" + id_user;

            /**** Starting Creating request body ****/
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            /**** End Creating request body ****/

            //  Creating list of categoria
            List<SelectListItem> cd_animal = new List<SelectListItem>();
            try
            {
                //  Sending request to api
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    statusCode = (int)response.StatusCode;
                    if(statusCode == 200)
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
                                    cd_animal.Add(
                                        new SelectListItem
                                        {
                                            Value = itens_response.cd_animal + "",
                                            Text = itens_response.nm_animal + ""
                                        }
                                       );
                                }
                            }
                        }
                    }
                    else
                    {
                        cd_animal.Add(new SelectListItem
                        {
                            Value = "999",
                            Text = ""
                        }
                        );
                    }
                    
                }
                return cd_animal;
            }
            catch (WebException ex)
            {
                System.Diagnostics.Debug.WriteLine("" + ex);
                cd_animal.Add(null);
                return cd_animal;
            }
        }
    }
}