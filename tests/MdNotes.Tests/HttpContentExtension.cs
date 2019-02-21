using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace MdNotes.Tests
{
    public static class HttpContentExtension
    {
        public static T DeserializeJson<T>(this HttpContent httpContent)
        {
            var task = DeserializeJsonAsync<T>(httpContent);
            task.Wait();
            return task.Result;
        }

        public static async Task<T> DeserializeJsonAsync<T>(this HttpContent httpContent)
        {
            var json = await httpContent.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
