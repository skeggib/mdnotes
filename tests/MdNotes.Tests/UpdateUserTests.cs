using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;

namespace MdNotes.Tests
{
    public class UpdateUserTests
    {
        private UserKey _userKey;

        public UpdateUserTests()
        {
            var name = Utils.GetUniqueString();
            var postTask = new HttpClient().PostAsync(
                $"{Utils.BaseUri}users", 
                new JsonContent($"{{\"name\": \"{name}\"}}"));
            postTask.Wait();
            var response = postTask.Result;
            var readTask = response.Content.ReadAsStringAsync();
            readTask.Wait();
            var json = readTask.Result;
            _userKey = JsonConvert.DeserializeObject<UserKey>(json);
        }

        [Fact]
        public async Task CanUpdateUser()
        {
            var newName = Utils.GetUniqueString();

            var request = new HttpRequestMessage(HttpMethod.Put, $"{Utils.BaseUri}users/{_userKey.User.Id}");
            request.Headers.Add("Authorization", $"Bearer {_userKey.DefaultKey}");
            request.Content = new JsonContent($"{{ \"name\": \"{newName}\" }}");
            
            var putResponse = new HttpClient().Send(request);
            Assert.Equal(HttpStatusCode.OK, putResponse.StatusCode);

            var responseGet = await new HttpClient().GetAsync(
                $"{Utils.BaseUri}users/{_userKey.User.Id}");
            var userGet = JsonConvert.DeserializeObject<User>(
                await responseGet.Content.ReadAsStringAsync());
            Assert.Equal(newName, userGet.Name);
        }

        [Fact]
        public async Task UpdatingNonExistingUserGives404()
        {
            var response = await new HttpClient().PutAsync(
                $"{Utils.BaseUri}users/9999",
                new JsonContent($"{{ \"name\": \"test\" }}"));
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public void InvalidBodyGives400()
        {
            var request = new HttpRequestMessage(HttpMethod.Put, $"{Utils.BaseUri}users/{_userKey.User.Id}");
            request.Headers.Add("Authorization", $"Bearer {_userKey.DefaultKey}");
            request.Content = new JsonContent("");

            var response = new HttpClient().Send(request);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }


        [Fact]
        public void UpdatingWhithoutRightKey401()
        {
            var nameOther = Utils.GetUniqueString();
            var postTask = new HttpClient().PostAsync(
                $"{Utils.BaseUri}users", 
                new JsonContent($"{{\"name\": \"{nameOther}\"}}"));
            postTask.Wait();
            var response = postTask.Result;
            var readTask = response.Content.ReadAsStringAsync();
            readTask.Wait();
            var json = readTask.Result;
            var _userKeyOther = JsonConvert.DeserializeObject<UserKey>(json);

            var request = new HttpRequestMessage(HttpMethod.Put, $"{Utils.BaseUri}users/{_userKey.User.Id}");
            request.Headers.Add("Authorization", $"Bearer {_userKeyOther.DefaultKey}");
            request.Content = new JsonContent("{{ \"name\": \"test\" }}");

            var putResponse = new HttpClient().Send(request);
            Assert.Equal(HttpStatusCode.Unauthorized, putResponse.StatusCode);
        }
    }
}