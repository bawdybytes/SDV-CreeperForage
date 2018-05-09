using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreeperForage
{
    public static class Stardew
    {
        private static bool ? useBetaMethods = null;
        public static bool UseBetaMethods
        {
            get
            {
                if(useBetaMethods == null)
                {
                    if (!Game1.version.StartsWith("1.2")) useBetaMethods = true;
                    else useBetaMethods = false;
                }
                return (bool) useBetaMethods;
            }
        }
        
        public static StardewValley.Farmer GetPlayer()
        {
            if (UseBetaMethods) return Game1.getFarmer(0);
            else return Game1.getAllFarmers().First();
        }

        public static int GetFriendshipPoints(string NPC)
        {
            dynamic f2 = GetPlayer();
            if (UseBetaMethods)
            {
                if (f2.friendshipData.ContainsKey(NPC)) return f2.friendshipData[NPC].Points;
                else return 0;
            }
            else
            {
                if(f2.friendships.ContainsKey(NPC))
                    return f2.friendships[NPC][0];
            }
            return 0;
        }

        public static void SetFriendshipPoints(string NPC, int points)
        {
            dynamic f2 = GetPlayer();
            if (UseBetaMethods)
            {
                if (!f2.friendshipData.ContainsKey(NPC)) f2.friendshipData[NPC] = new Friendship(points);
                else f2.friendshipData[NPC].Points = points;
            }
            else
            {
                if (!f2.friendships.ContainsKey(NPC)) f2.friendships[NPC] = new int[6];
                f2.friendships[NPC][0] = points;
            }
        }
    }
}
