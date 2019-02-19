using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MdNotes.Tests
{
    public class UserTests
    {
        private string _baseUri => "http://localhost:3000/";

        [Fact]
        public async Task CanCreateUser()
        {
            var name = GetUniqueString();
            var response = await new HttpClient().PostAsync(
                $"{_baseUri}users", 
                new StringContent($"{{\"name\": \"{name}\"}}", 
                Encoding.UTF8, "application/json"));
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var json = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<User>(json);
            Assert.True(user.Id > 0);

            await new HttpClient().DeleteAsync($"{_baseUri}users/{user.Id}");
        }

        [Fact]
        public async Task CanDeleteUser()
        {
            var name = GetUniqueString();
            var response = await new HttpClient().PostAsync(
                $"{_baseUri}users",
                new StringContent($"{{\"name\": \"{name}\"}}",
                Encoding.UTF8, "application/json"));
            var json = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<User>(json);

            var deleteResponse = await new HttpClient().DeleteAsync($"{_baseUri}users/{user.Id}");
            Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);
        }

        [Fact]
        public async Task DeletingNonExistingUserGives404()
        {
            var name = GetUniqueString();
            var response = await new HttpClient().PostAsync(
                $"{_baseUri}users",
                new StringContent($"{{\"name\": \"{name}\"}}",
                Encoding.UTF8, "application/json"));
            var json = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<User>(json);

            await new HttpClient().DeleteAsync($"{_baseUri}users/{user.Id}");
            var deleteResponse = await new HttpClient().DeleteAsync($"{_baseUri}users/{user.Id}");
            Assert.Equal(HttpStatusCode.NotFound, deleteResponse.StatusCode);
        }

        private string GetUniqueString() => DateTime.Now.ToOADate().ToString();
    }
}
