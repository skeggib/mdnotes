using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;

namespace MdNotes.Tests
{
    public class UserGetTests
    {
        [Fact]
        public async Task CanGetUser()
        {
            var name = Utils.GetUniqueString();
            var responseCreate = new HttpClient().Post(
                $"{Utils.BaseUri}users",
                new JsonContent($"{{\"name\": \"{name}\"}}"));
            var json = await responseCreate.Content.ReadAsStringAsync();
            var userKey = JsonConvert.DeserializeObject<UserKey>(json);

            var request = new HttpRequestMessage(HttpMethod.Get, $"{Utils.BaseUri}users/{userKey.User.Id}");
            request.Headers.Add("Authorization", $"Bearer {userKey.DefaultKey}");
            var responseGet = new HttpClient().Send(request);
            var json2 = await responseGet.Content.ReadAsStringAsync();
            var userGet = JsonConvert.DeserializeObject<User>(json2);

            Assert.Equal(name, userGet.Name);
        }

        [Fact]
        public async Task CanGetAllUsers()
        {
            var responseCreate1 = new HttpClient().Post(
                $"{Utils.BaseUri}users",
                new JsonContent($"{{\"name\": \"{Utils.GetUniqueString()}\"}}"));
            var user1 = JsonConvert.DeserializeObject<UserKey>(
                await responseCreate1.Content.ReadAsStringAsync());

            var responseCreate2 = new HttpClient().Post(
                $"{Utils.BaseUri}users",
                new JsonContent($"{{\"name\": \"{Utils.GetUniqueString()}\"}}"));
            var user2 = JsonConvert.DeserializeObject<UserKey>(
                await responseCreate2.Content.ReadAsStringAsync());

            var responseGet = new HttpClient().Get($"{Utils.BaseUri}users");
            var users = JsonConvert.DeserializeObject<User[]>(
                await responseGet.Content.ReadAsStringAsync());

            Assert.True(users.Where(user => user.Id == user1.User.Id).Count() == 1);
            Assert.True(users.Where(user => user.Id == user2.User.Id).Count() == 1);
            Assert.Equal(user1.User.Name, users.Where(user => user.Id == user1.User.Id).First().Name);
            Assert.Equal(user2.User.Name, users.Where(user => user.Id == user2.User.Id).First().Name);
        }

        [Fact]
        public void GetingNonExistingUserGives404()
        {
            var response = new HttpClient().Get(
                $"{Utils.BaseUri}users/9999");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}