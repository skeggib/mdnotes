using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace MdNotes.Tests
{
    public class NoteCreateTests
    {
        private readonly User _user;

        public NoteCreateTests()
        {
            var name = Utils.GetUniqueString();
            _user = new HttpClient().Post(
                $"{Utils.BaseUri}users",
                new JsonContent($"{{\"name\": \"{name}\"}}")).Content.DeserializeJson<User>();
        }

        // [Fact]
        // public async Task CanCreateNote()
        // {
        //     var responseCreate = new HttpClient().Post(
        //         $"{Utils.BaseUri}notes?user_id={_user.Id}",
        //         new JsonContent("{ \"title\": \"Title\", \"content\": \"Content\" }"));
        //     Assert.Equal(HttpStatusCode.Created, responseCreate.StatusCode);
        //     var jsonCreate = await responseCreate.Content.ReadAsStringAsync();
        //     var noteCreate = JsonConvert.DeserializeObject<Note>(jsonCreate);
        //     Assert.True(noteCreate.Id >= 0);
        // }

        // [Fact]
        // public void InvalidBodyGives400()
        // {
        //     var response = new HttpClient().Post(
        //         $"{Utils.BaseUri}notes?user_id={_user.Id}",
        //         new JsonContent("{\"invalid\": \"test\"}"));
        //     Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        // }

        // [Fact]
        // public void InvalidUserGives400()
        // {
        //     var response = new HttpClient().Post(
        //         $"{Utils.BaseUri}notes?user_id=9999",
        //         new JsonContent("{ \"title\": \"Title\", \"content\": \"Content\" }"));
        //     Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        // }

        // [Fact]
        // public void MissingUserGives400()
        // {
        //     var response = new HttpClient().Post(
        //         $"{Utils.BaseUri}notes",
        //         new JsonContent("{ \"title\": \"Title\", \"content\": \"Content\" }"));
        //     Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        // }
    }
}
