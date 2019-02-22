using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace MdNotes.Tests
{
    public class NoteCreateTests
    {
        private readonly UserKey _user;

        public NoteCreateTests()
        {
            var name = Utils.GetUniqueString();
            _user = new HttpClient().Post(
                $"{Utils.BaseUri}users",
                new JsonContent($"{{\"name\": \"{name}\"}}")).Content.DeserializeJson<UserKey>();
        }

        [Fact]
        public async Task CanCreateNote()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{Utils.BaseUri}notes");
            request.Content = new JsonContent("{ \"title\": \"Title\", \"content\": \"Content\" }");
            request.Headers.Add("Authorization", $"Bearer {_user.DefaultKey}");
            var responseCreate = new HttpClient().Send(request);
            Assert.Equal(HttpStatusCode.Created, responseCreate.StatusCode);
            var jsonCreate = await responseCreate.Content.ReadAsStringAsync();
            var noteCreate = JsonConvert.DeserializeObject<Note>(jsonCreate);
            Assert.True(noteCreate.Id >= 0);
        }

        [Fact]
        public void InvalidBodyGives400()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{Utils.BaseUri}notes");
            request.Content = new JsonContent("{\"invalid\": \"test\"}");
            request.Headers.Add("Authorization", $"Bearer {_user.DefaultKey}");
            var response = new HttpClient().Send(request);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public void InvalidTokenGives401()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{Utils.BaseUri}notes");
            request.Content = new JsonContent("{\"invalid\": \"test\"}");
            request.Headers.Add("Authorization", $"Bearer 9999");
            var response = new HttpClient().Send(request);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public void MissingTokenGives401()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{Utils.BaseUri}notes");
            request.Content = new JsonContent("{\"invalid\": \"test\"}");
            var response = new HttpClient().Send(request);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
