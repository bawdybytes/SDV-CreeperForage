using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using System.Collections.Generic;

namespace CreeperForage
{
    public class Assets : IAssetEditor
    {
        public bool CanEdit<T>(IAssetInfo asset)
        {
            return asset.AssetNameEquals("Data\\ObjectInformation") || asset.AssetNameEquals("Maps\\springobjects");
        }

        public void Edit<T>(IAssetData asset)
        {
            if (asset.AssetNameEquals("Data\\ObjectInformation"))
                foreach (var item in Item.items)
                    try { asset.AsDictionary<int, string>().Data.Add(item.Value.internal_id, item.Value.GetData()); } catch { }
            else
            {//if (asset.AssetNameEquals("Maps\\springobjects")){
                var oldTex = asset.AsImage().Data;
                if (oldTex.Width != 4096)
                {
                    Texture2D newTex = new Texture2D(StardewValley.Game1.graphics.GraphicsDevice, oldTex.Width, System.Math.Max(oldTex.Height, 4096));
                    asset.ReplaceWith(newTex);
                    asset.AsImage().PatchImage(oldTex);
                }
                foreach (var obj in Item.items)
                    try { asset.AsImage().PatchImage(obj.Value.texture, null, new Rectangle(obj.Value.internal_id % 24 * 16, obj.Value.internal_id / 24 * 16, 16, 16)); } catch { }
            }

        }
    }

    public class Item
    {
        public static int base_id = 1920;
        public static Dictionary<string, Item> items;

        public string unique_id;
        public string displayname;
        public string description;
        public int internal_id;
        public int price;
        public int edibility;
        public Texture2D texture;

        public static void Setup()
        {
            Mod.instance.Helper.Content.AssetEditors.Add(new Assets());
            items = new Dictionary<string, Item>();
            SetupItem("px.haley1", "pxhl1", "Haley's Panties", "These look expensive.", 31, 10);
            SetupItem("px.haley2", "pxhl2", "Haley's Panties", "These have clearly been worn.", 34, 10);
            SetupItem("px.abigail1", "pxab1", "Abigail's Panties", "How exciting!", 29, 10);
            SetupItem("px.abigail2", "pxab2", "Abigail's Panties", "They smell just like her.", 33, 10);
            SetupItem("px.emily1", "pxem1", "Emily's Panties", "I thought they'd be brighter.", 27, 10);
            SetupItem("px.emily2", "pxem2", "Emily's Panties", "This is a nice material.", 35, 10);
            SetupItem("px.penny1", "pxpn1", "Penny's Panties", "How charming.", 29, 10);
            SetupItem("px.leah1", "pxlh1", "Leah's Panties", "I don't think she'd mind.", 31, 10);
            SetupItem("px.leah2", "pxlh2", "Leah's Panties", "What a lucky find!", 34, 10);
            SetupItem("px.jodi1", "pxjd1", "Jodi's Panties", "Huh.", 29, 10);
            SetupItem("px.caroline1", "pxcr1", "Caroline's Panties", "About what I'd expect.", 27, 10);
            SetupItem("px.caroline2", "pxcr2", "Caroline's Panties", "Well how about that.", 32, 10);
            SetupItem("px.maru1", "pxmu1", "Maru's Panties", "A little less shy on the inside.", 32, 10);
            SetupItem("px.maru2", "pxmu2", "Maru's Panties", "Thoroughly lived in.", 33, 10);
            SetupItem("px.robin1", "pxrb1", "Robin's Panties", "A lucky find?", 33, 10);
            SetupItem("px.robin2", "pxrb2", "Robin's Panties", "They smell faintly of sawdust.", 36, 10);
        }

        public static void SetupItem(string id, string tx, string name, string desc, int price, int edibility)
        {
            items[id] = new Item(id, tx, name, desc, price, edibility);
        }

        public Item(string uid, string tx, string name, string desc, int price, int edibility)
        {
            unique_id = uid;
            displayname = name;
            description = desc;
            internal_id = base_id++;
            this.price = price;
            this.edibility = edibility;
            this.texture = Mod.instance.Helper.Content.Load<Texture2D>("./Assets/" + tx + ".png", ContentSource.ModFolder);
        }

        public string GetData()
        {
            return $"{displayname}/{price}/{edibility}/Basic {StardewValley.Object.artisanGoodsCategory}/{displayname}/{description}";
        }
    }
}
