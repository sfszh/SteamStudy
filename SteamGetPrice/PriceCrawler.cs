using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Runtime.Serialization.Json;

namespace SteamGetPrice
{
    class PriceCrawler
    {

        public string Run()
        {
            try
            {
                string req = CreateRequest(730);
                return MakeRequest(req);
            }
            catch (Exception e){
                Console.WriteLine(e.Message);
                Console.Read();
                return null;
            }

        }

        public string CreateRequest(int appid)
        {
            string urlRequest = "http://store.steampowered.com/api/appdetails/?appids="
                + appid
                + "&cc=EE&l=english&v=1";
            return urlRequest;
        }

        public string MakeRequest(string requestUrl)
        {
            try
            {
                HttpWebRequest req = WebRequest.Create(requestUrl) as HttpWebRequest;
                using (HttpWebResponse resp = req.GetResponse() as HttpWebResponse)
                {
                    if (resp.StatusCode != HttpStatusCode.OK)
                    {
                        throw new Exception(string.Format(
                            "Server error (HTTP {0}: {1}.",
                            resp.StatusCode,
                            resp.StatusDescription));
                    }
                    DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(HttpWebResponse));
                    StreamReader sReader = new StreamReader(resp.GetResponseStream());
                    //Console.WriteLine(sReader.ReadToEnd());
                    
                    //object objResponse = jsonSerializer.ReadObject(resp.GetResponseStream());
                    //WebResponse jsonResponse = objResponse as WebResponse;
                    //return jsonResponse;
                    return sReader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}
