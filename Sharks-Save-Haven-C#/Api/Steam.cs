using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Linq;

namespace Sharks_Save_Haven.Api
{
    public class Steam
    {
        public static async Task<SteamGame> GetSteamGame(int appId)
        {
            HttpClient client = new HttpClient();
            var x = await client.GetFromJsonAsync<Dictionary<int,SteamGameResponse>>($"https://store.steampowered.com/api/appdetails?appids={appId}");

            return x.First().Value.Data;
        }

        public static async Task<byte[]> GetSteamHeaderImage(int appId){
            HttpClient client = new HttpClient();
            try
            {
                var x = await client.GetByteArrayAsync($"https://cdn.akamai.steamstatic.com/steam/apps/{appId}/header.jpg");
                return x;
            }
            catch (System.Exception)
            {
                return null;
            }
        }
    }

    public class SteamGame 
    {
        public string Name { get; set; }
        public string[] Publishers { get; set; }
        public SteamPlatforms Platforms { get; set; }
        public SteamReleaseDate Release_Date { get; set; }
        public string Header_Image { get; set; }
    }

    public class SteamGameResponse
    {
        public SteamGame Data { get; set; }
    }

    public class SteamPlatforms{
        public bool Windows { get; set; }
        public bool Mac { get; set; }
        public bool Linux { get; set; }
    }

    public class SteamReleaseDate{
        public bool Coming_Soon { get; set; }
        public string Date { get; set; }
    }
}