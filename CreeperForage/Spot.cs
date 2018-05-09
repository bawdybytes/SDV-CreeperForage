using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CreeperForage
{
    public class Spot
    {
        public string NPC;
        public string Location;
        public int X;
        public int Y;
        public int PercentChance;
        public static Dictionary<string, List<string>> Loot;
        public static List<Spot> Spots;

        public static void Roll(object sender, EventArgs e)
        {
            int base_luck = (int)((StardewValley.Game1.dailyLuck + 1f) * 500f); //rng 0-100

            Random rng = new Random(DateTime.Now.Millisecond);

            foreach (Spot ss in Spots)
            {
                //despawn old items
                GameLocation l = Game1.getLocationFromName(ss.Location);
                Vector2 pos = new Vector2(ss.X, ss.Y);
                if (l.objects.ContainsKey(pos))
                {
                    //if there's one of our own items here, remove it
                    StardewValley.Object o1 = l.objects[pos];
                    if (o1.displayName.Contains("Panties") || o1.displayName.Contains("Underwear"))
                    {
                        DebugLog("Despawning item from " + ss.Location, LogLevel.Debug);
                        l.objects.Remove(pos);
                    }
                }

                //spawn a new item, if desireable
                if (!l.objects.ContainsKey(pos))
                {
                    //is npc enabled?
                    if (Config.GetNPC(ss.NPC).Enabled)
                    {
                        if (Loot.ContainsKey(ss.NPC))
                        {
                            if(Loot[ss.NPC].Count > 0)
                            {
                                int strikepoint = rng.Next(2001);
                                int chance = (int)(((((float)ss.PercentChance) / 100f) * (((float)base_luck) / 100f)) * 100f);
                                if (chance > strikepoint)
                                {
                                    Item ei = Item.items[Loot[ss.NPC].ElementAt(new Random(DateTime.Now.Millisecond).Next(Loot[ss.NPC].Count))];
                                    dynamic i = (StardewValley.Object)StardewValley.Objects.ObjectFactory.getItemFromDescription(0, ei.internal_id, 1);
                                    if (Stardew.UseBetaMethods)
                                    {
                                        i.isSpawnedObject.Value = true; //1.3
                                        i.quality.Value = ss.RollQuality(); //1.3
                                    }
                                    else
                                    {
                                        i.isSpawnedObject = true; //1.2
                                        i.quality = ss.RollQuality(); //1.2
                                    }
                                    i.ParentSheetIndex = ei.internal_id;
                                    DebugLog("Spawning item " + ei.unique_id + " at " + ss.Location, LogLevel.Debug);
                                    l.objects.Add(pos, i);
                                }
                            }
                        }
                    }
                }
            }
        }

        public int RollQuality()
        {
            int luv = Stardew.GetFriendshipPoints(NPC);
            Random rng = new Random(DateTime.Now.Millisecond);
            int mq = luv / 500;
            int quality = (mq > 0 && rng.Next(100) < 15 * mq) ? 1 : 0;
            if(mq > 1 && rng.Next(100) < 10 * mq){
                quality = 2;
                if(mq > 2 && rng.Next(100) < 5 * mq){
                    quality = 3;
                    if(mq > 3 && rng.Next(100) < 4) quality = 4;
                }
            }
            return quality;
        }


        public static void DebugLog(string msg, LogLevel level)
        {
            #if DEBUG
                Mod.instance.Monitor.Log(msg, level);
            #endif
        }

        public static void Setup()
        {
            StardewModdingAPI.Events.TimeEvents.AfterDayStarted += Roll;

            Loot = new Dictionary<string, List<string>>();
            if (Config.GetNPC("Haley").IsFemale) Loot["Haley"] = new List<string> { "px.haley1", "px.haley2" };
            else Loot["Haley"] = new List<string> { };

            if (Config.GetNPC("Emily").IsFemale) Loot["Emily"] = new List<string> { "px.emily1", "px.emily2" };
            else Loot["Emily"] = new List<string> { };

            if (Config.GetNPC("Penny").IsFemale) Loot["Penny"] = new List<string> { "px.penny1" };
            else Loot["Penny"] = new List<string> { };

            if (Config.GetNPC("Jodi").IsFemale) Loot["Jodi"] = new List<string> { "px.jodi1" };
            else Loot["Jodi"] = new List<string> { };

            if (Config.GetNPC("Leah").IsFemale) Loot["Leah"] = new List<string> { "px.leah1", "px.leah2" };
            else Loot["Leah"] = new List<string> { };

            if (Config.GetNPC("Caroline").IsFemale) Loot["Caroline"] = new List<string> { "px.caroline1", "px.caroline2" };
            else Loot["Caroline"] = new List<string> { };

            if (Config.GetNPC("Abigail").IsFemale) Loot["Abigail"] = new List<string> { "px.abigail1", "px.abigail2" };
            else Loot["Abigail"] = new List<string> { };

            if (Config.GetNPC("Maru").IsFemale) Loot["Maru"] = new List<string> { "px.maru1", "px.maru2" };
            else Loot["Maru"] = new List<string> { };

            if (Config.GetNPC("Robin").IsFemale) Loot["Robin"] = new List<string> { "px.robin1", "px.robin2" };
            else Loot["Robin"] = new List<string> { };
            
            Spots = new List<Spot>();
            int very_rare = 1;
            int rare = 5;
            int normal = 20;
            if (Config.GetNPC("Haley").HomeSpots)
            {
                Spots.Add(new Spot("Haley", "HaleyHouse", 3, 7, normal)); //br
                Spots.Add(new Spot("Haley", "HaleyHouse", 8, 6, normal)); //br
            }
            if (Config.GetNPC("Haley").OtherSpots)
            {
                Spots.Add(new Spot("Haley", "HaleyHouse", 6, 15, rare)); //livingroom
            }
            if (Config.GetNPC("Emily").HomeSpots)
            {
                Spots.Add(new Spot("Emily", "HaleyHouse", 19, 6, normal)); //br
                Spots.Add(new Spot("Emily", "HaleyHouse", 13, 8, normal)); //br
            }
            if (Config.GetNPC("Emily").OtherSpots)
            {
                Spots.Add(new Spot("Emily", "HaleyHouse", 5, 23, rare)); //livingroom
            }
            if (Config.GetNPC("Penny").HomeSpots)
            {
                Spots.Add(new Spot("Penny", "Trailer", 1, 8, normal)); //br
                Spots.Add(new Spot("Penny", "Trailer", 3, 5, normal)); //br
            }
            if (Config.GetNPC("Jodi").HomeSpots)
            {
                Spots.Add(new Spot("Jodi", "SamHouse", 19, 7, normal)); //br
                Spots.Add(new Spot("Jodi", "SamHouse", 20, 6, normal)); //br
            }
            if (Config.GetNPC("Leah").HomeSpots)
            {
                Spots.Add(new Spot("Leah", "LeahHouse", 3, 6, normal)); //br
                Spots.Add(new Spot("Leah", "LeahHouse", 9, 7, normal)); //br
            }
            if (Config.GetNPC("Leah").OtherSpots)
            {
                Spots.Add(new Spot("Leah", "LeahHouse", 13, 14, rare)); //kitchen
            }
            if (Config.GetNPC("Caroline").HomeSpots)
            {
                Spots.Add(new Spot("Caroline", "SeedShop", 27, 5, normal)); //br
                Spots.Add(new Spot("Caroline", "SeedShop", 28, 8, normal)); //br
            }
            if (Config.GetNPC("Abigail").HomeSpots)
            {
                Spots.Add(new Spot("Abigail", "SeedShop", 5, 8, normal)); //br
                Spots.Add(new Spot("Abigail", "SeedShop", 13, 6, normal)); //br
            }
            if (Config.GetNPC("Abigail").OtherSpots)
            {
                Spots.Add(new Spot("Abigail", "SeedShop", 40, 17, rare)); //altar
            }
            if (Config.GetNPC("Maru").HomeSpots)
            {
                Spots.Add(new Spot("Maru", "ScienceHouse", 9, 7, normal)); //br
                Spots.Add(new Spot("Maru", "ScienceHouse", 5, 6, normal)); //br
            }
            if (Config.GetNPC("Maru").OtherSpots)
            {
                Spots.Add(new Spot("Maru", "ScienceHouse", 30, 12, rare)); //kitchen
            }
            if (Config.GetNPC("Robin").HomeSpots)
            {
                Spots.Add(new Spot("Robin", "ScienceHouse", 15, 4, normal)); //br
                Spots.Add(new Spot("Robin", "ScienceHouse", 19, 6, normal)); //br
            }
            if (Config.GetNPC("Robin").OtherSpots)
            {
                Spots.Add(new Spot("Robin", "ScienceHouse", 22, 19, rare)); //lab
            }
        }

        public Spot(string npc, string loc, int x, int y, int chance)
        {
            this.NPC = npc;
            Location = loc;
            X = x;
            Y = y;
            PercentChance = chance;
        }
    }
}
