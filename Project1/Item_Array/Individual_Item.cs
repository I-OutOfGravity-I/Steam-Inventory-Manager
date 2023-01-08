using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1.Item_Array
{
    public class Individual_Weapon
    {
        private string inspect_link;
        private string[] sticker_link;
        private string[] sticker_name;
        private string Owner;
        private string assetid;

        public string Inspect_Link{ get { return inspect_link; } set { inspect_link = value; } }
        public string Assetid { get => assetid; set => assetid = value; }
        public string owner { get => Owner; set => Owner = value; }
        public string[] Sticker_link { get => sticker_link; set => sticker_link = value; }
        public string[] Sticker_name { get => sticker_name; set => sticker_name = value; }

        public Individual_Weapon(string Owner, string[] Sticker_link, string inspect_link,string[] Sticker_name,string assetid)
        {
            this.Owner = Owner;
            this.Sticker_link = Sticker_link;
            this.inspect_link = inspect_link;
            this.Sticker_name = Sticker_name;
            this.Assetid = assetid;
        }
    }
}
