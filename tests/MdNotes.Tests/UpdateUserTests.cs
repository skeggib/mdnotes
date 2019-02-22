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

        // [Fact]
        // public async Task UpdatingNonExistingUserGives404()
        // {
        //     var request = new HttpRequestMessage(HttpMethod.Put, $"{Utils.BaseUri}users/9999");
        //     request.Headers.Add("Authorization", $"Bearer {_userKey.DefaultKey}");
        //     request.Content = new JsonContent("{{ \"name\": \"test\" }}");

        //     var response = await new HttpClient().PutAsync(
        //         $"{Utils.BaseUri}users/9999",
        //         new JsonContent($"{{ \"name\": \"test\" }}"));
        //     Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        // }


        // [Fact]
        // public async Task UpdatingWhithoutKey401()
        // {
        //     var response = await new HttpClient().PutAsync(
        //         $"{Utils.BaseUri}users/9999",
        //         new JsonContent($"{{ \"name\": \"test\" }}"));
        //     Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        // }

        // [Fact]
        // public async Task InvalidBodyGives400()
        // {
        //     var putResponse = await new HttpClient().PutAsync(
        //         $"{Utils.BaseUri}users/{_userKey.User.Id}",
        //         new JsonContent($"{{ \"invalid\": \"test\" }}"));
        //     Assert.Equal(HttpStatusCode.BadRequest, putResponse.StatusCode);
        // }
    }
}