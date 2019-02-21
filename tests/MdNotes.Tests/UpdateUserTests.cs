using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;

namespace MdNotes.Tests
{
    public class UpdateUserTests
    {
        private User _user;

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
            _user = JsonConvert.DeserializeObject<User>(json);
        }

        [Fact]
        public async Task CanUpdateUser()
        {
            var newName = Utils.GetUniqueString();

            var putResponse = await new HttpClient().PutAsync(
                $"{Utils.BaseUri}users/{_user.Id}",
                new JsonContent($"{{ \"name\": \"{newName}\" }}"));
            Assert.Equal(HttpStatusCode.OK, putResponse.StatusCode);

            var responseGet = await new HttpClient().GetAsync(
                $"{Utils.BaseUri}users/{_user.Id}");
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
        public async Task InvalidBodyGives400()
        {
            var putResponse = await new HttpClient().PutAsync(
                $"{Utils.BaseUri}users/{_user.Id}",
                new JsonContent($"{{ \"invalid\": \"test\" }}"));
            Assert.Equal(HttpStatusCode.BadRequest, putResponse.StatusCode);
        }
    }
}