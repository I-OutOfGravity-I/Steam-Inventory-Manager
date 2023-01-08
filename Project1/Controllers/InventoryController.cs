using Microsoft.AspNetCore.Mvc;
using System.Data.Common;
using System.Web;
using System.Text.RegularExpressions;
using Nancy.Json;
using System.Net;
using Project1.Item_Array;
using System.Text.Json;

namespace Project1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class  InventoryController : ControllerBase
    {
        private remove_forbidden_characters remove = new remove_forbidden_characters();
        public Dictionary<int, string> market_hash_names;
        public Dictionary<long, dynamic> Global_DIC;
        public Dictionary<string, dynamic> Market_Appid;
        public Dictionary<string, Item_Group> Global_inventory;
        //+++++++++++++++++++++++++++
        public bool Online = true;
        //+++++++++++++++++++++++++++
        public string[] Profile_names;
        public string[] Cache_Inventories;
        string[] Accounts;
        public int Inventory_Item_Offset = 0;
        public string Appid;
        public int total_items_count_einzeln;
        public int total_items_count_global;
        public int total_items_count_search;
        public int Start_Page_GUI;
        public int End_Page_GUI;
        public bool End_of_Pages = false;
        public bool search = false;
        public int Current_Inventory = 0;
        public bool first_time = false;
        public bool slided = false;
        public bool slide_frozen = false;
        public int Chosen_Item_ID;
        public int Arrow_Auswahl = 0;
        public bool View_Page = true;
        public static string? Item_Price_get;
        public int Currency_ID;

        private readonly ILogger<InventoryController> _logger;

        public InventoryController(ILogger<InventoryController> logger) => _logger = logger;

        public static int TotalLines(string filePath)
        {
            using (StreamReader r = new StreamReader(filePath))
            {
                int i = 0;
                while (r.ReadLine() != null) { i++; }
                return i;
            }
        }
        [HttpGet]


        public static String betweenStrings(String text, String start, String end)
        {
            int p1 = text.IndexOf(start) + start.Length;
            int p2 = text.IndexOf(end, p1);

            if (end == "") return (text.Substring(p1));
            else return text.Substring(p1, p2 - p1);
        }

        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class Action
        {
            public string link { get; set; }
            public string name { get; set; }
        }

        public class Asset
        {
            public int appid { get; set; }
            public string contextid { get; set; }
            public string assetid { get; set; }
            public string classid { get; set; }
            public string instanceid { get; set; }
            public string amount { get; set; }
        }

        public class Description
        {
            public int appid { get; set; }
            public string classid { get; set; }
            public string instanceid { get; set; }
            public int currency { get; set; }
            public string background_color { get; set; }
            public string icon_url { get; set; }
            public string icon_url_large { get; set; }
            public List<Description> descriptions { get; set; }
            public int tradable { get; set; }
            public List<Action> actions { get; set; }
            public string name { get; set; }
            public string name_color { get; set; }
            public string type { get; set; }
            public string market_name { get; set; }
            public string market_hash_name { get; set; }
            public List<MarketAction> market_actions { get; set; }
            public int commodity { get; set; }
            public int market_tradable_restriction { get; set; }
            public int marketable { get; set; }
            public List<Tag> tags { get; set; }
            public string market_buy_country_restriction { get; set; }
            public string value { get; set; }
            public string color { get; set; }
        }

        public class MarketAction
        {
            public string link { get; set; }
            public string name { get; set; }
        }

        public class Root
        {
            public List<Asset> assets { get; set; }
            public List<Description> descriptions { get; set; }
            public int total_inventory_count { get; set; }
            public int success { get; set; }
            public int rwgrsn { get; set; }
        }

        public class Tag
        {
            public string category { get; set; }
            public string internal_name { get; set; }
            public string localized_category_name { get; set; }
            public string localized_tag_name { get; set; }
            public string color { get; set; }
        }


        public String Get()
        {
            Appid = "730";
            String Profile_Name = "123";
            download download = new download();

            int Inventory_Count_in_Profile = 2;
            string[] Profile = { "1", "2" };
            Profile = new string[Inventory_Count_in_Profile];

            Cache_Inventories = new string[2];
            /*
            for (int index = 0; index < Profile.Length; index++)
            {
                Cache_Inventories[index] = System.IO.File.ReadAllText(Environment.CurrentDirectory.ToString() + "/Global Profiles/" + Profile_Name + "/Inventories/" + index + "/Inventory.txt");
            }
            */
            Cache_Inventories[0] = System.IO.File.ReadAllText(Environment.CurrentDirectory + "/inventory1.txt"); ;
            Cache_Inventories[1] = System.IO.File.ReadAllText(Environment.CurrentDirectory + "/inventory2.txt"); ;



            Inventory_Item_Offset = 0;
            string all_json_data = "";
            var jss = new JavaScriptSerializer();
            Accounts = new string[] { "1", "a" };
            int total_inv_Count = 0;
            string assets = null;
            string descriptionz = null;
            var jss1 = new JavaScriptSerializer();
            bool at_least_one_not_used = false;
            int[] not_used_profiles = new int[Profile.Length];
            int last_worked = 0;
            string last_worked_string_length = null;
            Root[] Seperate_Inventories = new Root[Profile.Length];
            for (int index = 0; index < Cache_Inventories.Length; index++)
            {
                Seperate_Inventories[index] = jss.Deserialize<Root>(Cache_Inventories[index]);
                var dictSteamitems1 = jss.Deserialize<Root>(Cache_Inventories[index]);
                if (index == Cache_Inventories.Length - 1)
                {
                    if (dictSteamitems1.total_inventory_count != 0)
                    {
                        assets = "{\"assets\":[" + assets + betweenStrings(Cache_Inventories[index], "{\"assets\":[", "]");
                        descriptionz = "],\"descriptions\":[" + descriptionz + betweenStrings(Cache_Inventories[index], "\"descriptions\":[", "],\"total_inventory_count");
                        total_inv_Count = total_inv_Count + Convert.ToInt32(betweenStrings(Cache_Inventories[index], "\"total_inventory_count\":", ",\"success\":1"));
                        break;
                    }
                    else
                    {
                        at_least_one_not_used = true;
                        not_used_profiles[index] = index + 1;
                        assets = assets.Substring(0, assets.Length - last_worked_string_length.Length);
                        assets = "{\"assets\":[" + assets + betweenStrings(Cache_Inventories[last_worked], "{\"assets\":[", "]");
                        descriptionz = "],\"descriptions\":[" + descriptionz + betweenStrings(Cache_Inventories[last_worked], "\"descriptions\":[", "],\"total_inventory_count");
                        total_inv_Count = total_inv_Count + Convert.ToInt32(betweenStrings(Cache_Inventories[last_worked], "\"total_inventory_count\":", ",\"success\":1"));
                        break;
                    }
                }
                if (dictSteamitems1.total_inventory_count != 0)
                {
                    last_worked_string_length = betweenStrings(Cache_Inventories[index], "{\"assets\":[", "]") + ",";
                    assets = assets + last_worked_string_length;
                    descriptionz = descriptionz + betweenStrings(Cache_Inventories[index], "\"descriptions\":[", "],\"total_inventory_count") + ",";
                    total_inv_Count = total_inv_Count + Convert.ToInt32(betweenStrings(Cache_Inventories[index], "\"total_inventory_count\":", ",\"success\":1"));
                    last_worked = index;
                }
                else
                {
                    at_least_one_not_used = true;
                    not_used_profiles[index] = index + 1;
                }
            }
            //if there is at least one inventory not used throw it out and change all arrays which are depending on Profile.Length
            if (at_least_one_not_used == true)
            {
                int new_length = 0;
                for (int x = 0; x < Profile.Length; x++)
                {
                    if (not_used_profiles[x] == x + 1)
                    {
                        Profile[x] = null;
                        new_length++;
                    }
                }
                string[] temp_Accounts = new string[Profile.Length - new_length];
                string[] temp_Profile = new string[Profile.Length - new_length];
                Root[] temp_Seperate_Inventories = new Root[Profile.Length - new_length];
                new_length = 0;
                for (int x = 0; x < Profile.Length; x++)
                {
                    if (Profile[x] != null)
                    {
                        temp_Profile[new_length] = Profile[x];
                        temp_Seperate_Inventories[new_length] = Seperate_Inventories[x];
                        temp_Accounts[new_length] = Accounts[x];
                        new_length++;
                    }
                }
                Accounts = temp_Accounts;
                Seperate_Inventories = temp_Seperate_Inventories;
                Profile = temp_Profile;
            }
            all_json_data = assets + descriptionz + "],\"total_inventory_count\":" + total_inv_Count.ToString() + "}";
            var dictSteamitems = jss.Deserialize<Root>(all_json_data);
            /*
            Thread thread = new Thread(delegate () { ax.Video_Download_Request(dictSteamitems, Convert.ToInt32(Appid)); });
            thread.Start();
            */
            //download.Video_Download_Request(dictSteamitems, Convert.ToInt32(Appid));

            Item_Group[] temp_Global = new Item_Group[total_inv_Count];
            Dictionary<long, dynamic> dict = new Dictionary<long, dynamic>();


            long value = 0;
            for (int index = 0; index < dictSteamitems.descriptions.Count; index++)
            {
                value = Convert.ToInt64(dictSteamitems.descriptions[index].classid);
                if (!dict.ContainsKey(value))
                {
                    dict[value] = dictSteamitems.descriptions[index];
                }
            }

            market_hash_names = new Dictionary<int, string>();

            int xsd = 0;
            string hash = null;
            for (int Profile_Counter = 0; Profile_Counter < Profile.Length; Profile_Counter++)
            {
                for (int Description_Counter = 0; Description_Counter < Seperate_Inventories[Profile_Counter].descriptions.Count; Description_Counter++)
                {
                    hash = Convert.ToString(Seperate_Inventories[Profile_Counter].descriptions[Description_Counter].market_hash_name);
                    if (!market_hash_names.ContainsValue(hash))
                    {
                        market_hash_names[xsd] = Seperate_Inventories[Profile_Counter].descriptions[Description_Counter].market_hash_name;
                        xsd++;
                    }
                }
            }
            string all_market_hashes = null;
            for (int x = 0; x < market_hash_names.Count; x++)
            {
                all_market_hashes = all_market_hashes + "\"" + market_hash_names[x] + "\"" + ",";
            }
            all_market_hashes = all_market_hashes.Remove(all_market_hashes.Length - 1);


            string[] market_hash_names_from_db = new string[market_hash_names.Count];
            int[] sell_listings = new int[market_hash_names.Count];
            int[] lowest_price = new int[market_hash_names.Count];
            DateTime[] datetime = new DateTime[market_hash_names.Count];

            //
            //Request all Data from Database
            string[] missing = new string[market_hash_names.Count];
            int cound = 0;
            for (int kek = 0; kek < market_hash_names.Count; kek++)
            {
                if (!market_hash_names_from_db.Contains(market_hash_names[kek]))
                {
                    missing[cound] = market_hash_names[kek];
                    cound++;
                }
            }


            Global_inventory = market_hash_names.ToDictionary(lol2 => lol2.Value, lol2 => new Item_Group());
            int amount = dict.Count;

            for (int Profile_Counter = 0; Profile_Counter < Profile.Length; Profile_Counter++)
            {
                for (int Description_Counter = 0; Description_Counter < Seperate_Inventories[Profile_Counter].descriptions.Count; Description_Counter++)
                {
                    if (Global_inventory[Seperate_Inventories[Profile_Counter].descriptions[Description_Counter].market_hash_name].Item == null)
                    {
                        if (Appid == "730")
                        {
                            int cd = 2;
                            while (Global_inventory[Seperate_Inventories[Profile_Counter].descriptions[Description_Counter].market_hash_name].Rarity_Color == null)
                            {
                                try
                                {
                                    Global_inventory[Seperate_Inventories[Profile_Counter].descriptions[Description_Counter].market_hash_name].Rarity_Color = Seperate_Inventories[Profile_Counter].descriptions[Description_Counter].tags[cd].color;
                                    if (Seperate_Inventories[Profile_Counter].descriptions[Description_Counter].tags[cd].color != null)
                                    {
                                        cd = 0;
                                        break;
                                    }
                                    else
                                    {
                                        cd++;
                                    }
                                }
                                catch
                                {
                                    cd++;
                                }
                            }
                            Global_inventory[Seperate_Inventories[Profile_Counter].descriptions[Description_Counter].market_hash_name].category = Seperate_Inventories[Profile_Counter].descriptions[Description_Counter].tags[1].category;
                        }
                        if (Appid != "730" && Appid != "753")
                        {
                            try
                            {
                                Global_inventory[Seperate_Inventories[Profile_Counter].descriptions[Description_Counter].market_hash_name].Rarity_Color = Seperate_Inventories[Profile_Counter].descriptions[Description_Counter].name_color;
                            }
                            catch
                            { }
                        }
                        Global_inventory[Seperate_Inventories[Profile_Counter].descriptions[Description_Counter].market_hash_name].Market_name = Seperate_Inventories[Profile_Counter].descriptions[Description_Counter].name;
                        Global_inventory[Seperate_Inventories[Profile_Counter].descriptions[Description_Counter].market_hash_name].ItemRemovedSymbols = "images/" + remove.replace(Global_inventory[Seperate_Inventories[Profile_Counter].descriptions[Description_Counter].market_hash_name].Market_name) + ".png";
                        Global_inventory[Seperate_Inventories[Profile_Counter].descriptions[Description_Counter].market_hash_name].ItemRemovedSymbols = Global_inventory[Seperate_Inventories[Profile_Counter].descriptions[Description_Counter].market_hash_name].ItemRemovedSymbols.Replace(" ", "-").Replace("|", "-").Replace("?", "-").Replace(":","-");
                        Global_inventory[Seperate_Inventories[Profile_Counter].descriptions[Description_Counter].market_hash_name].Class_ID = Seperate_Inventories[Profile_Counter].descriptions[Description_Counter].classid;
                        Global_inventory[Seperate_Inventories[Profile_Counter].descriptions[Description_Counter].market_hash_name].Icon_Url = Seperate_Inventories[Profile_Counter].descriptions[Description_Counter].icon_url;
                        string replace = remove.replace(Global_inventory[Seperate_Inventories[Profile_Counter].descriptions[Description_Counter].market_hash_name].Market_name);
                        if (!System.IO.File.Exists(Global_inventory[Seperate_Inventories[Profile_Counter].descriptions[Description_Counter].market_hash_name].ItemRemovedSymbols))
                        {
                            using (var client = new WebClient())
                            {
                                client.DownloadFile("https://community.akamai.steamstatic.com/economy/image/" + Global_inventory[Seperate_Inventories[Profile_Counter].descriptions[Description_Counter].market_hash_name].Icon_Url, Global_inventory[Seperate_Inventories[Profile_Counter].descriptions[Description_Counter].market_hash_name].ItemRemovedSymbols);
                            }
                        }
                        Global_inventory[Seperate_Inventories[Profile_Counter].descriptions[Description_Counter].market_hash_name].Item = Seperate_Inventories[Profile_Counter].descriptions[Description_Counter].market_hash_name;
                        Global_inventory[Seperate_Inventories[Profile_Counter].descriptions[Description_Counter].market_hash_name].Marketable = Seperate_Inventories[Profile_Counter].descriptions[Description_Counter].marketable;
                    }
                    if (Appid == "730")
                    {
                        if (Seperate_Inventories[Profile_Counter].descriptions[Description_Counter].tags[1].category == "Weapon")
                        {
                            try
                            {
                                Global_inventory[Seperate_Inventories[Profile_Counter].descriptions[Description_Counter].market_hash_name].Quality = Seperate_Inventories[Profile_Counter].descriptions[Description_Counter].tags[5].localized_tag_name;
                            }
                            catch
                            {
                                Global_inventory[Seperate_Inventories[Profile_Counter].descriptions[Description_Counter].market_hash_name].Quality = Seperate_Inventories[Profile_Counter].descriptions[Description_Counter].tags[4].localized_tag_name;
                            }

                            string[] Sticker_Link = new string[4];
                            string[] Sticker_name = new string[4];
                            if (Seperate_Inventories[Profile_Counter].descriptions[Description_Counter].descriptions.Count > 6)
                            {
                                int i = 0;
                                string sticker = Seperate_Inventories[Profile_Counter].descriptions[Description_Counter].descriptions[Seperate_Inventories[Profile_Counter].descriptions[Description_Counter].descriptions.Count - 1].value;
                                if (sticker != "" && sticker != " ")
                                {
                                    string temp_sticker = sticker;
                                    try
                                    {
                                        while (i < 4)
                                        {
                                            Sticker_Link[i] = betweenStrings(temp_sticker, "src=\"", ".png\">") + ".png";
                                            string asd = "src=\"" + Sticker_Link[i] + "\">";
                                            var regex = new Regex(asd);
                                            temp_sticker = regex.Replace(temp_sticker, "", 1);
                                            i++;
                                        }
                                    }
                                    catch
                                    {
                                    }
                                    temp_sticker = betweenStrings(temp_sticker, "<br>Sticker: ", "<");
                                    temp_sticker = ", " + temp_sticker + ",";
                                    for (int xyz = 0; xyz < i; xyz++)
                                    {
                                        Sticker_name[xyz] = betweenStrings(temp_sticker, ", ", ",");
                                        string asd = ", " + Sticker_name[xyz];
                                        int index = temp_sticker.IndexOf(asd);

                                        temp_sticker = temp_sticker.Remove(index, asd.Length);
                                    }
                                }
                            }
                            else
                            {
                            }
                            string classid = Seperate_Inventories[Profile_Counter].descriptions[Description_Counter].classid;
                            for (int search_assetid = 0; search_assetid < Seperate_Inventories[Profile_Counter].assets.Count; search_assetid++)
                            {
                                if (Seperate_Inventories[Profile_Counter].assets[search_assetid].classid == classid)
                                {
                                    string inspect_link = Seperate_Inventories[Profile_Counter].descriptions[Description_Counter].actions[0].link;
                                    inspect_link = inspect_link.Replace("%owner_steamid%", Profile[Profile_Counter]);
                                    inspect_link = inspect_link.Replace("%assetid%", Seperate_Inventories[Profile_Counter].assets[search_assetid].assetid);
                                    Global_inventory[Seperate_Inventories[Profile_Counter].descriptions[Description_Counter].market_hash_name].Amount = Global_inventory[Seperate_Inventories[Profile_Counter].descriptions[Description_Counter].market_hash_name].Amount + 1;
                                    if (Global_inventory[Seperate_Inventories[Profile_Counter].descriptions[Description_Counter].market_hash_name].Individual_Weapons == null)
                                    {
                                        Global_inventory[Seperate_Inventories[Profile_Counter].descriptions[Description_Counter].market_hash_name].Individual_Weapons = new List<Individual_Weapon>();
                                    }
                                    if (Global_inventory[Seperate_Inventories[Profile_Counter].descriptions[Description_Counter].market_hash_name].owner_int == null)
                                    {
                                        Global_inventory[Seperate_Inventories[Profile_Counter].descriptions[Description_Counter].market_hash_name].owner_int = new int[Profile.Length];
                                    }
                                    Global_inventory[Seperate_Inventories[Profile_Counter].descriptions[Description_Counter].market_hash_name].Individual_Weapons.Add(new Individual_Weapon(Accounts[Profile_Counter], Sticker_Link, inspect_link, Sticker_name, Seperate_Inventories[Profile_Counter].assets[search_assetid].assetid));
                                    Global_inventory[Seperate_Inventories[Profile_Counter].descriptions[Description_Counter].market_hash_name].owner_int[Profile_Counter] = Global_inventory[Seperate_Inventories[Profile_Counter].descriptions[Description_Counter].market_hash_name].owner_int[Profile_Counter] + 1;
                                }
                            }
                        }
                        else
                        {
                            if (Global_inventory[Seperate_Inventories[Profile_Counter].descriptions[Description_Counter].market_hash_name].owner_int == null)
                            {
                                Global_inventory[Seperate_Inventories[Profile_Counter].descriptions[Description_Counter].market_hash_name].owner_int = new int[Profile.Length];
                            }
                            string classid = Seperate_Inventories[Profile_Counter].descriptions[Description_Counter].classid;
                            for (int search_assetid = 0; search_assetid < Seperate_Inventories[Profile_Counter].assets.Count; search_assetid++)
                            {
                                if (Seperate_Inventories[Profile_Counter].assets[search_assetid].classid == classid)
                                {
                                    Global_inventory[Seperate_Inventories[Profile_Counter].descriptions[Description_Counter].market_hash_name].Amount = Global_inventory[Seperate_Inventories[Profile_Counter].descriptions[Description_Counter].market_hash_name].Amount + 1;
                                    Global_inventory[Seperate_Inventories[Profile_Counter].descriptions[Description_Counter].market_hash_name].owner_int[Profile_Counter] = Global_inventory[Seperate_Inventories[Profile_Counter].descriptions[Description_Counter].market_hash_name].owner_int[Profile_Counter] + 1;
                                }
                            }
                        }
                    }
                    if (Global_inventory[Seperate_Inventories[Profile_Counter].descriptions[Description_Counter].market_hash_name].category == null)
                    {
                        if (Global_inventory[Seperate_Inventories[Profile_Counter].descriptions[Description_Counter].market_hash_name].owner_int == null)
                        {
                            Global_inventory[Seperate_Inventories[Profile_Counter].descriptions[Description_Counter].market_hash_name].owner_int = new int[Profile.Length];
                        }
                        string classid = Seperate_Inventories[Profile_Counter].descriptions[Description_Counter].classid;
                        for (int search_assetid = 0; search_assetid < Seperate_Inventories[Profile_Counter].assets.Count; search_assetid++)
                        {
                            if (Seperate_Inventories[Profile_Counter].assets[search_assetid].classid == classid)
                            {
                                Global_inventory[Seperate_Inventories[Profile_Counter].descriptions[Description_Counter].market_hash_name].Amount = Global_inventory[Seperate_Inventories[Profile_Counter].descriptions[Description_Counter].market_hash_name].Amount + 1;
                                Global_inventory[Seperate_Inventories[Profile_Counter].descriptions[Description_Counter].market_hash_name].owner_int[Profile_Counter] = Global_inventory[Seperate_Inventories[Profile_Counter].descriptions[Description_Counter].market_hash_name].owner_int[Profile_Counter] + 1;
                            }
                        }
                    }
                    int Amount_of_Tags = Seperate_Inventories[Profile_Counter].descriptions[Description_Counter].tags.Count;
                    /*
                    for (int y = 0; y < Amount_of_Tags; y++)
                    {
                        Global_inventory[Seperate_Inventories[Profile_Counter].descriptions[Description_Counter].market_hash_name].Tags.Add(Seperate_Inventories[Profile_Counter].descriptions[Description_Counter].tags[y]);
                    }
                    */
                }
            }
            /*
            for (int index = 0; index < Seperate_Inventories[profilecount]["assets"].Count; index++)
                        {
                            long test1 = Convert.ToInt64(Seperate_Inventories[profilecount]["assets"][index]["classid"]);
                            string test2 = dict[test1].market_hash_name;
                            Global_inventory[test2].Amount = Global_inventory[test2].Amount + 1;
                            Global_inventory[test2].Individual_Item_Non_Weapons[0].owner_int[profilecount] = Global_inventory[test2].Individual_Item_Non_Weapons[0].owner_int[profilecount] + 1;
                        }
            
            
            for (int index = 0; index < dictSteamitems["assets"].Count; index++)
            {
                long test1 = Convert.ToInt64(dictSteamitems["assets"][index]["classid"]);
                string test2 = dict[test1].market_hash_name;
                Global_inventory[test2].Amount = Global_inventory[test2].Amount + 1;
            }
            
            uint sum = 0;
            foreach (KeyValuePair<string, Item_Group> entry in Global_inventory)
            {
                sum = Global_inventory[entry.Key].Amount + sum;
            }
            */
            int dictttt = dictSteamitems.total_inventory_count;
            var accountIdHighBits = (Convert.ToInt64(Profile[0]) >> 1) & 0x7FFFFFF;

            Global_DIC = dict;
            total_items_count_global = market_hash_names.Count;
            Current_Inventory = 2;
            total_items_count_einzeln = 0;

            List<Item_Group> abd = new List<Item_Group>();
            foreach (Item_Group abc in Global_inventory.Values)
            {
                abd.Add(abc);
            }
            var json = JsonSerializer.Serialize(abd);
            return json;
        }
    }
}