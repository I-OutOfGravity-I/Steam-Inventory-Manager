using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Project1.Item_Array
{
    public class Item_Group
    {
        private string item;
        private string itemRemovedSymbols;
        private uint amount;
        private string icon_url;
        private string class_id;
        private string rarity_Color;
        private string quality;
        private string Category;
        private List<Individual_Weapon> individual_Weapons;
        private List<Tag> tags;
        private int marketable;
        private string market_name;
        private int[] Owner_int;

        public string Item
        {
            get { return item; }
            set { item = value; }
        }
        
        public string Market_name
        {
            get { return market_name; }
            set { market_name = value; }
        }
        public int Marketable
        {
            get { return marketable; }
            set { marketable = value; }
        }
        public uint Amount
        {
            get { return amount; }
            set { amount = value; }
        }
        public string Icon_Url
        {
            get { return icon_url; }
            set { icon_url = value; }
        }
        public string Class_ID
        {
            get { return class_id; }
            set { class_id = value; }
        }
        public string Rarity_Color
        {
            get { return rarity_Color; }
            set { rarity_Color = value; }
        }

        public int[] owner_int {
            get { return Owner_int; }
            set { Owner_int = value;}
        }
        public string Quality { get => quality; set => quality = value; }
        public List<Individual_Weapon> Individual_Weapons { get => individual_Weapons; set => individual_Weapons = value; }

        public string category { get => Category; set => Category = value; }
        public List<Tag> Tags { get => tags; set => tags = value; }
        public string ItemRemovedSymbols { get => itemRemovedSymbols; set => itemRemovedSymbols = value; }
    }

    public class Tag
    {
        public string category { get; set; }
        public string internal_name { get; set; }
        public string localized_category_name { get; set; }
        public string localized_tag_name { get; set; }
        public string color { get; set; }
    }
}
