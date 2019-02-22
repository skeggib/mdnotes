using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MdNotes.Tests
{
    public class UserCreateTests
    {
        [Fact]
        public async Task CanCreateUser()
        {
            var name = Utils.GetUniqueString();
            var response = await new HttpClient().PostAsync(
                $"{Utils.BaseUri}users", 
                new JsonContent($"{{\"name\": \"{name}\"}}"));
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var json = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<UserKey>(json);
            Assert.True(user.User.Id >= 0);
            Assert.True(user.DefaultKey.Length >= 0);
        }

        [Fact]
        public async Task InvalidBodyDataGives400()
        {
            var response = await new HttpClient().PostAsync(
                $"{Utils.BaseUri}users", 
                new JsonContent($"{{\"invalid\": \"test\"}}"));
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task SameUserNameGives409()
        {
            var name = Utils.GetUniqueString();
            var response = await new HttpClient().PostAsync(
                $"{Utils.BaseUri}users", 
                new JsonContent($"{{\"name\": \"{name}\"}}"));
            var json = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<UserKey>(json);
            var responseConflict = await new HttpClient().PostAsync(
                $"{Utils.BaseUri}users", 
                new JsonContent($"{{\"name\": \"{name}\"}}"));
            Assert.Equal(HttpStatusCode.Conflict, responseConflict.StatusCode);
        }
    }
}
