using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;
using System.Net;
using Project1.Item_Array;

namespace Project1.Controllers
{
    public class download
    {
        public void Video_Download_Request(InventoryController.Root dictSteam, int Appid)
        {
            remove_forbidden_characters remove = new remove_forbidden_characters();
            string[] market_names = new string[dictSteam.descriptions.Count];
            int mnc = 0;
            for (int x = 0; x < dictSteam.descriptions.Count - 1; x++)
            {
                string temp = dictSteam.descriptions[x].name;
                if (market_names.Contains(temp))
                {

                }
                else
                {
                    if (!System.IO.File.Exists(System.IO.Directory.GetCurrentDirectory() + "\\appid" + "\\" + Appid + "\\" + remove.replace(temp) + ".png"))
                    {
                        string icon_url = dictSteam.descriptions[x].icon_url;
                        Thread thread = new Thread(delegate () { Download(temp, icon_url, Appid); });
                        thread.Start();
                        market_names[mnc] = dictSteam.descriptions[x].name;
                        mnc++;
                    }
                }
            }
        }
        public void Download(string Market_Name, string Icon_Url, int Appid)
        {
            remove_forbidden_characters remove = new remove_forbidden_characters();
            Market_Name = remove.replace(Market_Name);
            string file = null;
            string get = null;
            get = "http://cdn.steamcommunity.com/economy/image/" + Icon_Url + "/150x150";
            string lol = remove.replace(Market_Name);
            file = System.IO.Directory.GetCurrentDirectory() + "\\appid" + "\\" + Appid + "\\" + Market_Name + ".png";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(get);
            request.ProtocolVersion = HttpVersion.Version11;
            request.Accept = "*";
            request.ContentType = "application/octet-stream";
            request.KeepAlive = true;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            using (Stream output = System.IO.File.OpenWrite(file))
            using (Stream input = response.GetResponseStream())
            {
                byte[] buffer = new byte[4096];
                int bytesRead;
                while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    output.Write(buffer, 0, bytesRead);
                }
                output.Close();
            }
            response.Close();
        }
    }
}
