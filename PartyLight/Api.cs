using PartyLight.Models.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PartyLight
{

    class Api
    {

        public const string secondaryTileId = "PowerSecondaryTile";
        public const string secondaryTileParam = "p_toggle";

        public static async Task Toggle(MainView model)
        {
            WebRequest request = WebRequest.Create("http://" + model.Address + "/args.lua?submit=1&mode=power&power=toggle");
            request.Headers["Cache-Control"] = "no-cache";
            await request.GetResponseAsync();
        }

    }

}
