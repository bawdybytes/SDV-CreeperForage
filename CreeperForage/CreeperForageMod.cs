using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreeperForage
{
    public class CreeperForageMod : Mod
    {
        public override void Entry(IModHelper helper)
        {
            this.Monitor.Log("Heya creepers!", LogLevel.Alert);
        }
    }
}
