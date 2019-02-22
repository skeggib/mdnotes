using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;

namespace MdNotes.Tests
{
    public class UserDeleteTests
    {
        [Fact]
        public async Task CanDeleteUser()
        {
            var name = Utils.GetUniqueString();
            var responseDelete = new HttpClient().Post(
                $"{Utils.BaseUri}users",
                new JsonContent($"{{\"name\": \"{name}\"}}"));
            var json = await responseDelete.Content.ReadAsStringAsync();
            var userKey = JsonConvert.DeserializeObject<UserKey>(json);

            var request = new HttpRequestMessage(HttpMethod.Delete, $"{Utils.BaseUri}users/{userKey.User.Id}");
            request.Headers.Add("Authorization", $"Bearer {userKey.DefaultKey}");
            var response = new HttpClient().Send(request);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseGet = new HttpClient().Get($"{Utils.BaseUri}users/{userKey.User.Id}");
            Assert.Equal(HttpStatusCode.NotFound, responseGet.StatusCode);
        }

        [Fact]
        public async Task DeletingNonExistingUserGives404()
        {
            var name = Utils.GetUniqueString();
            var responseDelete = new HttpClient().Post(
                $"{Utils.BaseUri}users",
                new JsonContent($"{{\"name\": \"{name}\"}}"));
            var json = await responseDelete.Content.ReadAsStringAsync();
            var userKey = JsonConvert.DeserializeObject<UserKey>(json);

            var request = new HttpRequestMessage(HttpMethod.Delete, $"{Utils.BaseUri}users/9999");
            request.Headers.Add("Authorization", $"Bearer {userKey.DefaultKey}");
            var response = new HttpClient().Send(request);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DeletingWithoutTokenGives401()
        {
            var name = Utils.GetUniqueString();
            var responseDelete = new HttpClient().Post(
                $"{Utils.BaseUri}users",
                new JsonContent($"{{\"name\": \"{name}\"}}"));
            var json = await responseDelete.Content.ReadAsStringAsync();
            var userKey = JsonConvert.DeserializeObject<UserKey>(json);

            var request = new HttpRequestMessage(HttpMethod.Delete, $"{Utils.BaseUri}users/{userKey.User.Id}");
            request.Headers.Add("Authorization", $"Bearer 9999");
            var response = new HttpClient().Send(request);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}