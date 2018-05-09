using StardewModdingAPI;
using StardewValley;
using StardewModdingAPI.Events;

namespace CreeperForage
{
    public class Mod : StardewModdingAPI.Mod
    {
        public static Mod instance;

        public override void Entry(IModHelper helper)
        {
            instance = this;

            Config.Load();
            if (Config.ready)
            {
                Item.Setup();
                Spot.Setup();
            }
            #if DEBUG
                InputEvents.ButtonPressed += InputEvents_ButtonPressed;
            #endif          
        }

        private void InputEvents_ButtonPressed(object sender, EventArgsInput e)
        {
            if (e.Button == SButton.OemTilde){
                
                Monitor.Log("Testing disposition on Haley.");
                Monitor.Log("Before points: " + Stardew.GetFriendshipPoints("Haley"));
                Stardew.SetFriendshipPoints("Haley", 2500);
                Monitor.Log("After points: " + Stardew.GetFriendshipPoints("Haley"));

                var f = Stardew.GetPlayer();
                this.Monitor.Log($"Loc: {Game1.currentLocation.Name} @ {(int)(f.getTileX())} x {(int)(f.getTileY())}", LogLevel.Alert);
            }
        }
    }
}