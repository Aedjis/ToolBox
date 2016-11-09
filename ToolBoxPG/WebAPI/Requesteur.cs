using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace ToolBoxPG.WebAPI
{
    public class Requesteur
    {
        private const string _apiUrl = "http://localhost:53129/";

        private static void AddApiCredential(KeyValuePair<string, string> Credentials, ref HttpWebRequest Request)
        {
            if (Credentials.Key != null && Credentials.Value != null)
            {
                Request.Credentials = new NetworkCredential(Credentials.Key, Credentials.Value);
            }
        }

        /// <summary>
        ///     méthod qui va communiqué avec l'api en get.
        /// </summary>
        /// <param name="Credentials">Les credentials sous la forme [Login, Password]</param>
        /// <param name="Api">la route du service demandé</param>
        /// <param name="Parametre">
        ///     le ou les paramtre a envoyé mit sous la forme de keyvaluepaire ou la key est le nom du
        ///     parametre et le value est sa valeur.
        /// </param>
        /// <returns>une chaine de caractére non deséréaliser</returns>
        public static T ApiGet<T>(KeyValuePair<string, string> Credentials, string Api, params KeyValuePair<string, string>[] Parametre)
        {
            string UrlParams = ConvertUrlParams(Parametre);

            HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(_apiUrl + Api + UrlParams);
            Request.Method = "GET";
            Request.ContentType = "application/json";
            Request.ContentLength = 0;
            AddApiCredential(Credentials, ref Request);

            WebResponse WebResponse = Request.GetResponse();
            Stream WebStream = WebResponse.GetResponseStream();
            if (WebStream == null) throw new NullReferenceException();
            StreamReader ResponseReader = new StreamReader(WebStream);
            string Retour = ResponseReader.ReadToEnd();
            ResponseReader.Close();

            return JsonConvert.DeserializeObject<T>(Retour);
        }

        /// <summary>
        ///     méthod qui va communiqué avec l'api en get.
        /// </summary>
        /// <param name="Api">la route du service demandé</param>
        /// <param name="Parametre">
        ///     le ou les paramtre a envoyé mit sous la forme de keyvaluepaire ou la key est le nom du
        ///     parametre et le value est sa valeur.
        /// </param>
        /// <returns>une chaine de caractére non deséréaliser</returns>
        public static T ApiGet<T>(string Api, params KeyValuePair<string, string>[] Parametre)
        {
            return ApiGet<T>(default(KeyValuePair<string, string>), Api, Parametre);
        }
        
        /// <summary>
        ///     method qui va convertire les parametre pour l'api
        /// </summary>
        /// <param name="Parametre">les paramtre</param>
        /// <returns></returns>
        private static string ConvertUrlParams(IReadOnlyList<KeyValuePair<string, string>> Parametre)
        {
            string Retour = "";
            if (Parametre.Count > 0)
            {
                Retour += "?";
                for (int I = 0; I < Parametre.Count; I++)
                {
                    Retour += Parametre[I].Key + "=" + Parametre[I].Value;
                    if (I < Parametre.Count - 1)
                        Retour += "&";
                }
            }
            return Retour;
        }

        /// <summary>
        ///     méthod qui va communiqué avec l'api en post.
        /// </summary>
        /// <param name="Api">la route du service demandé</param>
        /// <param name="Insert">l'objet à envoyé en parametre a l'api.</param>
        /// <returns>une chaine de caractére non deséréaliser (qui représente souvent un bool)</returns>
        public static T ApiPost<T>(string Api, object Insert)
        {
            return ApiPost<T>(default(KeyValuePair<string, string>), Api, Insert);
        }

        /// <summary>
        ///     méthod qui va communiqué avec l'api en post.
        /// </summary>
        /// <param name="Credentials">Les credentials sous la forme [Login, Password]</param>
        /// <param name="Api">la route du service demandé</param>
        /// <param name="Insert">l'objet à envoyé en parametre a l'api.</param>
        /// <returns>une chaine de caractére non deséréaliser (qui représente souvent un bool)</returns>
        public static T ApiPost<T>(KeyValuePair<string, string> Credentials, string Api, object Insert)
        {
            string SerializedObject = JsonConvert.SerializeObject(Insert);

            HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(_apiUrl + Api);
            Request.Method = "POST";
            Request.ContentType = "application/json";
            Request.ContentLength = Encoding.UTF8.GetByteCount(SerializedObject); //serializedObject.Length;
            using (StreamWriter Writer = new StreamWriter(Request.GetRequestStream()))
            {
                Writer.Write(SerializedObject); 
            }
            AddApiCredential(Credentials, ref Request);

            WebResponse WebResponse = Request.GetResponse();
            Stream WebStream = WebResponse.GetResponseStream();
            if (WebStream == null) throw new NullReferenceException();
            StreamReader ResponseReader = new StreamReader(WebStream);
            string Retour = ResponseReader.ReadToEnd();
            ResponseReader.Close();
            return JsonConvert.DeserializeObject<T>(Retour);
        }

        /// <summary>
        ///     méthod qui va communiqué avec l'api en put.
        /// </summary>
        /// <param name="Api">la route du service demandé</param>
        /// <param name="Update">l'objet à envoyé en parametre a l'api.</param>
        /// <returns>une chaine de caractére non deséréaliser (qui représente souvent un bool)</returns>
        public static T ApiPut<T>(string Api, object Update)
        {
            return ApiPut<T>(default(KeyValuePair<string, string>), Api, Update);
        }

        /// <summary>
        ///     méthod qui va communiqué avec l'api en put.
        /// </summary>
        /// <param name="Credentials">Les credentials sous la forme [Login, Password]</param>
        /// <param name="Api">la route du service demandé</param>
        /// <param name="Update">l'objet à envoyé en parametre a l'api.</param>
        /// <returns>une chaine de caractére non deséréaliser (qui représente souvent un bool)</returns>
        public static T ApiPut<T>(KeyValuePair<string,string> Credentials, string Api, object Update)
        {
            string SerializedObject = JsonConvert.SerializeObject(Update);

            HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(_apiUrl + Api);
            Request.Method = "PUT";
            Request.ContentType = "application/json";
            Request.ContentLength = Encoding.UTF8.GetByteCount(SerializedObject); //serializedObject.Length;
            using (StreamWriter Writer = new StreamWriter(Request.GetRequestStream()))
            {
                Writer.Write(SerializedObject); 
            }
            AddApiCredential(Credentials ,ref Request);

            WebResponse WebResponse = Request.GetResponse();
            Stream WebStream = WebResponse.GetResponseStream();
            if (WebStream == null) throw new NullReferenceException();
            StreamReader ResponseReader = new StreamReader(WebStream);
            string Retour = ResponseReader.ReadToEnd();
            ResponseReader.Close();
            return JsonConvert.DeserializeObject<T>(Retour);
        }


    }
}