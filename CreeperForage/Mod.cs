using StardewModdingAPI;
using StardewValley;
using StardewModdingAPI.Events;

namespace CreeperForage
{
    public class Mod : StardewModdingAPI.Mod
    {
        internal static Mod instance;

        public override void Entry(IModHelper helper)
        {
            instance = this;

            Item.Setup();
            Spot.Setup();

            #if DEBUG
                InputEvents.ButtonPressed += InputEvents_ButtonPressed;
            #endif          
        }

        private void InputEvents_ButtonPressed(object sender, EventArgsInput e)
        {
            if (e.Button == SButton.OemTilde){
                var f = Game1.getFarmer(0);
                foreach (var friend in f.friendshipData)
                    foreach (var v in friend.Values)
                        v.Points = 2500;
                this.Monitor.Log($"Loc: {Game1.currentLocation.Name} @ {(int)(f.getTileX())} x {(int)(f.getTileY())}", LogLevel.Alert);
            }
        }
    }
}