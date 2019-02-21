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
            var response = await new HttpClient().PostAsync(
                $"{Utils.BaseUri}users",
                new JsonContent($"{{\"name\": \"{name}\"}}"));
            var json = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<User>(json);

            var deleteResponse = await new HttpClient().DeleteAsync($"{Utils.BaseUri}users/{user.Id}");
            Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);
        }

        [Fact]
        public async Task DeletingNonExistingUserGives404()
        {
            var name = Utils.GetUniqueString();
            var response = await new HttpClient().PostAsync(
                $"{Utils.BaseUri}users",
                new JsonContent($"{{\"name\": \"{name}\"}}"));
            var json = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<User>(json);

            await new HttpClient().DeleteAsync($"{Utils.BaseUri}users/{user.Id}");
            var deleteResponse = await new HttpClient().DeleteAsync($"{Utils.BaseUri}users/{user.Id}");
            Assert.Equal(HttpStatusCode.NotFound, deleteResponse.StatusCode);
        }
    }
}