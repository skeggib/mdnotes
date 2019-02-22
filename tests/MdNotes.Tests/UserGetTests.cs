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
        // [Fact]
        // public async Task CanGetUser()
        // {
        //     var name = Utils.GetUniqueString();
        //     var responseCreate = await new HttpClient().PostAsync(
        //         $"{Utils.BaseUri}users", 
        //         new JsonContent($"{{\"name\": \"{name}\"}}"));
        //     var id = JsonConvert.DeserializeObject<User>(
        //         await responseCreate.Content.ReadAsStringAsync()
        //     ).Id;

        //     var responseGet = await new HttpClient().GetAsync(
        //         $"{Utils.BaseUri}users/{id}");
        //     var json = await responseGet.Content.ReadAsStringAsync();
        //     var userGet = JsonConvert.DeserializeObject<User>(json);

        //     Assert.Equal(name, userGet.Name);
        // }

        // [Fact]
        // public async Task CanGetAllUsers()
        // {
        //     var responseCreate1 = await new HttpClient().PostAsync(
        //         $"{Utils.BaseUri}users",
        //         new JsonContent($"{{\"name\": \"{Utils.GetUniqueString()}\"}}"));
        //     var user1 = JsonConvert.DeserializeObject<User>(
        //         await responseCreate1.Content.ReadAsStringAsync());

        //     var responseCreate2 = await new HttpClient().PostAsync(
        //         $"{Utils.BaseUri}users",
        //         new JsonContent($"{{\"name\": \"{Utils.GetUniqueString()}\"}}"));
        //     var user2 = JsonConvert.DeserializeObject<User>(
        //         await responseCreate1.Content.ReadAsStringAsync());

        //     var responseGet = await new HttpClient().GetAsync($"{Utils.BaseUri}users");
        //     var users = JsonConvert.DeserializeObject<User[]>(
        //         await responseGet.Content.ReadAsStringAsync());

        //     Assert.True(users.Where(user => user.Id == user1.Id).Count() == 1);
        //     Assert.True(users.Where(user => user.Id == user2.Id).Count() == 1);
        //     Assert.Equal(user1.Name, users.Where(user => user.Id == user1.Id).First().Name);
        //     Assert.Equal(user2.Name, users.Where(user => user.Id == user2.Id).First().Name);
        // }

        // [Fact]
        // public async Task GetingNonExistingUserGives404()
        // {
        //     var response = await new HttpClient().GetAsync(
        //         $"{Utils.BaseUri}users/9999");
        //     Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        // }
    }
}