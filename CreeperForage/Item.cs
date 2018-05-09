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
            CreateBasicPersonalItem("Haley", 1, "These look expensive.", 41);
            CreateBasicPersonalItem("Haley", 2, "These have clearly been worn.", 29);
            CreateBasicPersonalItem("Abigail", 1, "How exciting!", 29);
            CreateBasicPersonalItem("Abigail", 2, "They smell just like " + Config.GetNPC("Abigail").GetPronoun(1) + ".", 33);
            CreateBasicPersonalItem("Emily", 1, "I thought they'd be brighter.", 27);
            CreateBasicPersonalItem("Emily", 2, "This is a nice material.", 35);
            CreateBasicPersonalItem("Penny", 1, "How charming.", 29);
            CreateBasicPersonalItem("Penny", 2, "They smell kind of sweet.", 32);
            CreateBasicPersonalItem("Leah", 1, "I don't think " + Config.GetNPC("Abigail").GetPronoun(0) + "'d mind.", 31);
            CreateBasicPersonalItem("Leah", 2, "What a lucky find!", 34); 
            CreateBasicPersonalItem("Jodi", 1, "Huh.", 22);
            CreateBasicPersonalItem("Jodi", 2, "A rare sight.", 22);
            CreateBasicPersonalItem("Caroline", 1, "About what I'd expect.", 24);
            CreateBasicPersonalItem("Caroline", 2, "Well how about that.", 32);
            CreateBasicPersonalItem("Maru", 1, "A little less shy on the inside.", 32);
            CreateBasicPersonalItem("Maru", 2, "Thoroughly lived in.", 33);
            CreateBasicPersonalItem("Robin", 1, "A lucky find?", 33);
            CreateBasicPersonalItem("Robin", 2, "They smell faintly of sawdust.", 36);
        }

        public static void CreateBasicPersonalItem(string npc, int variant, string desc, int price)
        {
            SetupItem("px." + npc.ToLower() + variant, "px" + Config.GetNPC(npc).Abbreviate(npc) + (Config.GetNPC(npc).HasMaleItems() ? "m" : "f") + variant, Config.GetNPC(npc).Name + "'s " + (Config.GetNPC(npc).HasMaleItems() ? "Underwear" : "Panties"), desc, price, 10);
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
